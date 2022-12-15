using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace Company.Function
{
    public static class MyMovieGetItemRating
    {
        [FunctionName("MyMovieGetItemRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] int n)
        {
            string queryString = $"SELECT TOP {n} FROM [dbo].[userdata].itemId;";
            var message = String.Empty;

            using (SqlConnection connection = new SqlConnection("ConnectionString"))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var json = $"\"itemId\": = \"{reader.GetInt32(3)}\"," +
                                       $"\"count\": = \"{reader.GetInt32(5)}\";";

                            var jsonString = "{" + json + "}";
                            message = String.Format(jsonString);
                        }
                    }

                    return (ActionResult)new OkObjectResult(message);
                }
            }
        }
    }
}
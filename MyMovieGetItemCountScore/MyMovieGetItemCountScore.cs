using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;

namespace Company.Function
{
    public static class MyMovieGetItemCountScore
    {
        [FunctionName("MyMovieGetItemCountScore")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] string userName)
        {
            string queryString = $"SELECT COUNT(*) FROM [dbo].[userdata] WHERE userName = '{userName}';";
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
                            message = String.Format(reader.GetInt32(0).ToString());
                        }
                    }

                    return (ActionResult)new OkObjectResult(message);
                }
            }
        }
    }
}

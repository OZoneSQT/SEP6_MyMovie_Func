using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace Company.Function
{
    public static class MyMovieGetItemData
    {
        [FunctionName("MyMovieGetItemData")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] int itemId)
        {
            string queryString = $"SELECT * FROM [dbo].[userdata] WHERE itemId = '{itemId}';";
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
                            var json = $"\"timeStamp\": = \"{reader.GetInt32(0)}\"," +
                                       $"\"userName\": = \"{reader.GetString(1)}\"," +
                                       $"\"listName\": = \"{reader.GetString(2)}\"," +
                                       $"\"itemId\": = \"{reader.GetInt32(3)}\"," +
                                       $"\"comment\": = \"{reader.GetString(4)}\"," +
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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class MyMovieGetItemData
    {
        [FunctionName("MyMovieGetItemData")]
        public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, int itemId)
        {

            string queryString = $"SELECT * FROM [dbo].[userdata] WHERE itemId = '{itemId}';";

            using (SqlConnection connection = new SqlConnection("ConnectionString"))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

            return new OkObjectResult("200");

        }
    }
}

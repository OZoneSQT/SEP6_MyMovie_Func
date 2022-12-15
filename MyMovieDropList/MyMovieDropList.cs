using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;

namespace Company.Function
{
    public static class MyMovieDropList
    {
    [FunctionName("MyMovieDropList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
             string userName, int itemId, string timeStamp)
        {

            string queryString = $"DELETE * FROM [dbo].[userdata] WHERE userName = '{userName}' AND itemId = '{itemId}';";

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

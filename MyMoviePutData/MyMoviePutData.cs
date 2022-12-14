using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class MyMoviePutData
    {
        [FunctionName("MyMoviePutData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "PostFunction")] HttpRequest req, ILogger log,
            string userName, string listName, int itemId, string comment)
        {

            string queryString = $"INSERT INTO dbo.userdata(userName, listName, itemId, comment) VALUES({userName},{listName},{itemId},{comment});";

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

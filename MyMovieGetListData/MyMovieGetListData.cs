using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;

namespace Company.Function
{
    public static class MyMovieGetListData
    {
        [FunctionName("MyMovieGetListData")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] string userName)
        {
            string queryString = $"SELECT * FROM [dbo].[userdata] WHERE userName = '{userName}';";
            var message = String.Empty;
            ArrayList arrayList= new ArrayList();

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
                            if (!arrayList.Contains(reader.GetString(2)))
                            {
                                arrayList.Add(reader.GetString(2));
                            } 
                        }
                    }

                    string msg = "";
                    foreach (string entry in arrayList)
                    {
                        msg = msg + "," + entry;
                    }

                    message = String.Format(msg);

                    return (ActionResult)new OkObjectResult(message);
                }
            }
        }
    }
}
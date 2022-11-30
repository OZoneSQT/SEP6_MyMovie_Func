using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyMovieSearch
{
    public static class SearchMovieAPI
    {
        /**
         * Function to handle a search to via an external api, returns a JSON file as a string
         * Parsing is handled by reciving method
         * 
         * @return string
         */
        [FunctionName("SearchMovieAPI")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, string searchType, string searchString, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed the search request for: {searchString}");

            /**
             * Build search string
             */
            string apiKey = TMDB_KEY; // TMDB API KEY, ignore error it is updated by GitHub Actions on deploy, from stored secret
            string searchQuery = $"https://api.themoviedb.org/3/{searchType}/{searchString}?api_key={apiKey}&language=en-US&external_source=imdb_id";

            /**
             * Send search query to external movie database api
             */
            string result = req.Query[searchQuery];

            /*
             * Handel response from  external movie database api
             */
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            result = result ?? data?.result;

            /**
             * Validate result
             */
            string responseMessage = string.IsNullOrEmpty(result)
                ? "Bad search request."
                : result;

            /**
             * Return search result
             */
            return new OkObjectResult(responseMessage);
        }
    }
}

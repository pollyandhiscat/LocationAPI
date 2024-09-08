using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ServerlessAPI
{
    public class ServeCountryData
    {
        private readonly ILogger<ServeCountryData> _logger;

        public ServeCountryData(ILogger<ServeCountryData> logger)
        {
            _logger = logger;
        }

        [Function("ServeCountryData")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetLocations/{name}")] HttpRequest req, string name)
        {

            _logger.LogInformation($"HTTP {req.Method.ToString()} request recieved.");
           
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://rawcdn.githack.com/kamikazechaser/administrative-divisions-db/master/api/{name}.json");
            Uri address = client.BaseAddress;
            var response = await client.GetAsync(address);

            if (response.IsSuccessStatusCode) {

                var responseObject = await response.Content.ReadAsStringAsync();
                return new OkObjectResult($"Serving country data now...{"\n"}{responseObject.ToString()}");
            }

            else
            {
                return new BadRequestObjectResult("Failed to retrieve country information. Bad request.");
            }

        }
    }
}

using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        private readonly string url;
        private readonly ILogger logger;
        private readonly HttpClient client = new HttpClient();

        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            this.url = url ?? throw new ArgumentNullException(nameof(url));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<List<string>> GetProductAsync(string path)
        {
            logger.LogInfo("Connecting to the Restful server using HTTP");

            List<string> tradesString = null;
            var response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                tradesString = JsonSerializer.Deserialize<List<string>>(jsonString) ?? new List<string>();
                logger.LogInfo("Received trade strings of length = " + tradesString.Count);
            }
            else
            {
                logger.LogWarning("Failed to retrieve data from server. Status: " + response.StatusCode);
            }

            return tradesString ?? new List<string>();
        }

        public IEnumerable<string> GetTradeData()
        {
            var task = Task.Run(() => GetProductAsync(url));
            task.Wait();

            return task.Result;
        }
    }
}
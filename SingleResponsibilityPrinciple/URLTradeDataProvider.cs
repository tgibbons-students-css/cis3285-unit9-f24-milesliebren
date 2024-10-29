using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace SingleResponsibilityPrinciple
{
    public class URLTradeDataProvider : ITradeDataProvider
    {
        private readonly string _url;
        private readonly ILogger _logger;

        public URLTradeDataProvider(string url, ILogger logger)
        {
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<string> GetTradeData()
        {
            var tradeData = new List<string>();

            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(_url).Result;
                    response.EnsureSuccessStatusCode();

                    using (var stream = response.Content.ReadAsStreamAsync().Result)
                    using (var reader = new StreamReader(stream))
                    {
                        string line = "";
                        while (reader.ReadLine() != null)
                        {
                            tradeData.Add(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error reading trade data from URL: {_url}. Exception: {ex.Message}");
                throw new IOException();
            }

            return tradeData;
        }
    }
}

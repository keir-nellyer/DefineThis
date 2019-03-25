using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using DefineThis.Exceptions;
using Newtonsoft.Json.Linq;

namespace DefineThis.Definitions
{
    public class OxfordDictionaryDefiner : IDefiner
    {
        private const string API_BASE_URL = "https://od-api.oxforddictionaries.com/api/v1";
        private const string LANGUAGE = "en";
        private const string REGION = "GB";

        private const string APP_ID = "370838dc";
        private const string APP_KEY = "2ccbeb8ccefc1a1be7d1a77d6d373c69";

        private HttpClient httpClient;

        public OxfordDictionaryDefiner()
        {
            setupHttpClient();
        }

        private void setupHttpClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("app_id", APP_ID);
            httpClient.DefaultRequestHeaders.Add("app_key", APP_KEY);
        }
        
        public IEnumerable<string> GetDefinitions(string phrase)
        {
            var phraseLower = phrase.ToLower();
            var response = httpClient.GetAsync($"{API_BASE_URL}/entries/{LANGUAGE}/{phraseLower}").Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var data = JObject.Parse(json);
                // get all the definitions
                var definitionsJson = ((JArray)data["results"][0]["lexicalEntries"][0]["entries"])
                    .Select(entry => entry["senses"])
                    .SelectMany(senses => senses.Select(sense => sense["definitions"]));
                var definitions = definitionsJson.SelectMany(d => d.ToObject<string[]>()).ToList();
                return definitions;
            }
            else
            {
                // any status code other than 404 not found isn't expected
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw new ApiResponseException($"Request wasn't successful: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
                return Enumerable.Empty<string>();
            }
        }
    }
}
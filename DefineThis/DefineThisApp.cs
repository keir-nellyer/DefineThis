using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DefineThis
{
    public class DefineThisApp
    {
        private const string API_BASE_URL = "https://od-api.oxforddictionaries.com/api/v1";
        private const string LANGUAGE = "en";
        private const string REGION = "GB";

        private const string APP_ID = "370838dc";
        private const string APP_KEY = "2ccbeb8ccefc1a1be7d1a77d6d373c69";

        private static HttpClient httpClient = new HttpClient();

        static DefineThisApp()
        {
            httpClient.DefaultRequestHeaders.Add("app_id", APP_ID);
            httpClient.DefaultRequestHeaders.Add("app_key", APP_KEY);
        }

        public static void Main(string[] args)
        {
            while (true)
            {
                var input = askForInput();

                if (isInputValid(input))
                {
                    string[] definitions;

                    try
                    {
                        var task = getDefinitionAsync(input);
                        definitions = task.Result;
                    }
                    catch (AggregateException ae)
                    {
                        ae.Handle(e =>
                        {
                            if (e is ApiResponseException)
                            {
                                logError(e);
                                return true;
                            }
                            else if (e is HttpRequestException)
                            {
                                logError(e);
                                return true;
                            }
                            return false;
                        });
                        continue;
                    }

                    if (definitions != null)
                    {
                        printArray(definitions);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find a definition for that word.");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("The input was not valid, check that you entered a single word.");
                    Console.WriteLine();
                }
            }
        }

        private static bool isInputValid(string input)
        {
            return !input.Contains(" ");
        }

        private static string askForInput()
        {
            Console.WriteLine("Please enter a word:");
            return Console.ReadLine();
        }

        private static async Task<string[]> getDefinitionAsync(string word)
        {
            var wordId = word.ToLower();
            var response = await httpClient.GetAsync($"{API_BASE_URL}/entries/{LANGUAGE}/{wordId}");

            if (response.IsSuccessStatusCode)
            {
                var data = JObject.Parse(await response.Content.ReadAsStringAsync());
                var definitionsJson = (JArray)data["results"][0]["lexicalEntries"][0]["entries"][0]["senses"][0]["definitions"];
                var definitions = definitionsJson.ToObject<string[]>();
                return definitions;
            }
            else
            {
                // any other than 404 not found isn't expected
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw new ApiResponseException($"Request wasn't successful: {response.StatusCode} - {response.ReasonPhrase}");
                }
                return null;
            }
        }

        private static void printArray(string[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {arr[i]}");
            }
        }

        private static void logError(Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            Console.WriteLine(e);
            Console.WriteLine();
        }
    }
}

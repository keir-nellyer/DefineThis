using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DefineThis
{
    /// <summary>
    /// An application which takes in a word from the console and returns the definition.
    /// </summary>
    public class DefineThisApp
    {
        private const string API_BASE_URL = "https://od-api.oxforddictionaries.com/api/v1";
        private const string LANGUAGE = "en";
        private const string REGION = "GB";

        private const string APP_ID = "370838dc";
        private const string APP_KEY = "2ccbeb8ccefc1a1be7d1a77d6d373c69";

        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Initializes the <see cref="T:DefineThis.DefineThisApp"/> class.
        /// </summary>
        static DefineThisApp()
        {
            httpClient.DefaultRequestHeaders.Add("app_id", APP_ID);
            httpClient.DefaultRequestHeaders.Add("app_key", APP_KEY);
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
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
                                logException(e);
                                return true;
                            }
                            else if (e is HttpRequestException)
                            {
                                logException(e);
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

        /// <summary>
        /// Checks if the input is valid.
        /// </summary>
        /// <returns><c>true</c>, if input was valid, <c>false</c> otherwise.</returns>
        /// <param name="input">The input.</param>
        private static bool isInputValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && !input.Contains(" ");
        }

        /// <summary>
        /// Asks for input.
        /// </summary>
        /// <returns>The input.</returns>
        private static string askForInput()
        {
            Console.WriteLine("Please enter a word:");
            return Console.ReadLine();
        }

        /// <summary>
        /// Gets the definition of a word async.
        /// </summary>
        /// <returns>The definition async.</returns>
        /// <param name="word">The word.</param>
        private static async Task<string[]> getDefinitionAsync(string word)
        {
            var wordId = word.ToLower();
            var response = await httpClient.GetAsync($"{API_BASE_URL}/entries/{LANGUAGE}/{wordId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);
                // get all the definitions
                var definitionsJson = ((JArray)data["results"][0]["lexicalEntries"][0]["entries"])
                    .Select(entry => entry["senses"])
                    .SelectMany(senses => senses.Select(sense => sense["definitions"]));
                var definitions = definitionsJson.SelectMany(d => d.ToObject<string[]>()).ToArray();
                return definitions;
            }
            else
            {
                // any status code other than 404 not found isn't expected
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw new ApiResponseException($"Request wasn't successful: {response.StatusCode} - {response.ReasonPhrase}");
                }
                return null;
            }
        }

        /// <summary>
        /// Prints an array.
        /// </summary>
        /// <param name="arr">The array.</param>
        private static void printArray(string[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {arr[i]}");
            }
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        private static void logException(Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            Console.WriteLine(e);
            Console.WriteLine();
        }
    }
}

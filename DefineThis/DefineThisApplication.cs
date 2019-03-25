using System;
using System.Collections.Generic;
using System.Linq;
using DefineThis.Definitions;

namespace DefineThis
{
    /// <summary>
    /// An application which takes in a word from the console and returns the definition.
    /// </summary>
    public class DefineThisApplication
    {
        private IDefiner definer;

        public DefineThisApplication(IDefiner definer)
        {
            this.definer = definer;
        }

        public void Start()
        {
            while (true)
            {
                var input = AskForInput();

                if (!IsInputValid(input))
                {
                    Console.WriteLine("The input was not valid, check that you entered a single word.");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine("Fetching definition...");
                
                List<string> definitions;
                try
                {
                    definitions = this.definer.GetDefinitions(input).ToList();
                }
                catch (AggregateException ae)
                {
                    ae.Handle(e =>
                    {
                        LogException(e);
                        return true;
                    });

                    continue;
                }

                if (definitions.Any())
                {
                    PrintEnumerable(definitions);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Couldn't find a definition for that word.");
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Checks if the input is valid.
        /// </summary>
        /// <returns><c>true</c>, if input was valid, <c>false</c> otherwise.</returns>
        /// <param name="input">The input.</param>
        private bool IsInputValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && !input.Contains(" ");
        }

        /// <summary>
        /// Asks for input.
        /// </summary>
        /// <returns>The input.</returns>
        private string AskForInput()
        {
            Console.WriteLine("Please enter a word:");
            return Console.ReadLine();
        }

        /// <summary>
        /// Prints an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        private void PrintEnumerable(IEnumerable<string> enumerable)
        {
            var i = 1;

            foreach (var str in enumerable)
            {
                Console.WriteLine($"{i}. {str}");
                i++;
            }
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        private void LogException(Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            Console.WriteLine(e);
            Console.WriteLine();
        }
    }
}

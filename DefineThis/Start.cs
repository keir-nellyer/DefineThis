using System;
using DefineThis.Definitions;
using Microsoft.Extensions.DependencyInjection;

namespace DefineThis
{
    public class Start
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var start = new Start();
            start.Go();
        }

        /// <summary>
        /// Starts up the application.
        /// </summary>
        public void Go()
        {
            var serviceProvider = SetupContainer();
            var app = ActivatorUtilities.CreateInstance<DefineThisApplication>(serviceProvider);
            app.Start();
        }
        
        /// <summary>
        /// Sets up the dependency injection container.
        /// </summary>
        /// <returns>the service provider</returns>
        private IServiceProvider SetupContainer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IDefiner, OxfordDictionaryDefiner>();
            
            return serviceCollection.BuildServiceProvider();
        }
    }
}
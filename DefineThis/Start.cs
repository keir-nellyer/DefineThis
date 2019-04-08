using System;
using DefineThis.Audit;
using DefineThis.Configuration;
using DefineThis.Definitions;
using DefineThis.Persistence;
using DefineThis.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
            serviceCollection.AddTransient<IHistoryReader, HistoryReader>();
            serviceCollection.AddTransient<IHistoryWriter, HistoryWriter>();
            serviceCollection.AddTransient<IHistoryRecorder, HistoryRecorder>();
            serviceCollection.AddTransient<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddTransient<HistoryConfiguration>();
            serviceCollection.AddTransient<IJsonSerializerFactory, JsonSerializerFactory>();
            serviceCollection.AddTransient<JsonSerializer>(provider =>
            {
                var factory = provider.GetService<IJsonSerializerFactory>();
                return factory.Create();
            });
            
            return serviceCollection.BuildServiceProvider();
        }
    }
}
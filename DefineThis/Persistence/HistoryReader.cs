using System.Collections.Generic;
using System.IO;
using DefineThis.Configuration;
using DefineThis.Persistence.Entities;
using Newtonsoft.Json;

namespace DefineThis.Persistence
{
    public class HistoryReader : IHistoryReader
    {
        private HistoryConfiguration _configuration;
        private JsonSerializer _serializer;

        public HistoryReader(HistoryConfiguration configuration, JsonSerializer serializer)
        {
            _configuration = configuration;
            _serializer = serializer;
        }

        public History Get()
        {
            if (!File.Exists(_configuration.HistoryPath))
            {
                return new History()
                {
                    // TODO: This feels wrong
                    Items = new List<HistoryItem>()
                };
            }

            using (var reader = File.OpenText(_configuration.HistoryPath))
            {
                return (History)_serializer.Deserialize(reader, typeof(History));
            }
        }
    }
}
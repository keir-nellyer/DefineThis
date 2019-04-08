using System.IO;
using DefineThis.Configuration;
using DefineThis.Persistence.Entities;
using Newtonsoft.Json;

namespace DefineThis.Persistence
{
    public class HistoryWriter : IHistoryWriter
    {
        private HistoryConfiguration _configuration;
        private IHistoryReader _historyReader;
        private JsonSerializer _serializer;

        public HistoryWriter(HistoryConfiguration configuration, IHistoryReader historyReader, JsonSerializer serializer)
        {
            _configuration = configuration;
            _historyReader = historyReader;
            _serializer = serializer;
        }
        
        public void Write(HistoryItem item)
        {
            var history = _historyReader.Get();
            history.Items.Add(item);

            using (var writer = File.CreateText(this._configuration.HistoryPath))
            {
                this._serializer.Serialize(writer, history);
            }
        }
    }
}
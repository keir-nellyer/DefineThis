using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DefineThis.Persistence.Entities
{
    public class History
    {
        public List<HistoryItem> Items { get; set; }
    }
}
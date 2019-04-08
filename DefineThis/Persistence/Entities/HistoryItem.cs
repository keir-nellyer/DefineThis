using System;

namespace DefineThis.Persistence.Entities
{
    public class HistoryItem
    {
        public string Phrase { get; set; }
        
        public DateTimeOffset OccurredAt { get; set; }
    }
}
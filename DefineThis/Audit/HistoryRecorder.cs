using System;
using DefineThis.Persistence;
using DefineThis.Persistence.Entities;

namespace DefineThis.Audit
{
    public class HistoryRecorder : IHistoryRecorder
    {
        private IHistoryWriter _writer;
        private IDateTimeProvider _dateTimeProvider;

        public HistoryRecorder(IHistoryWriter writer, IDateTimeProvider dateTimeProvider)
        {
            this._writer = writer;
            this._dateTimeProvider = dateTimeProvider;
        }

        public void Record(string phrase)
        {
            var historyItem = new HistoryItem()
            {
                Phrase = phrase,
                OccurredAt = _dateTimeProvider.UtcNow()
            };
            
            _writer.Write(historyItem);
        }
    }
}
using DefineThis.Persistence.Entities;

namespace DefineThis.Persistence
{
    public interface IHistoryWriter
    {
        void Write(HistoryItem item);
    }
}
using DefineThis.Persistence.Entities;

namespace DefineThis.Persistence
{
    public interface IHistoryReader
    {
        History Get();
    }
}
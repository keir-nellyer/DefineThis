using System;

namespace DefineThis.Audit
{
    public interface IHistoryRecorder
    {
        void Record(string phrase);
    }
}
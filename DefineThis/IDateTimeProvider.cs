using System;

namespace DefineThis
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow();
    }
}
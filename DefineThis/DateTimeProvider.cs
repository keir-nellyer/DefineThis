using System;

namespace DefineThis
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
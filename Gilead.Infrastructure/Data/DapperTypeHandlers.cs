using System.Data;
using Dapper;

namespace Gilead.Infrastructure.Data;

internal static class DapperTypeHandlers
{
    private static int registered;

    public static void Register()
    {
        if (Interlocked.Exchange(ref registered, 1) == 1)
        {
            return;
        }

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
    }

    private sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value) =>
            value switch
            {
                DateTime dateTime => DateOnly.FromDateTime(dateTime),
                DateOnly dateOnly => dateOnly,
                string text => DateOnly.Parse(text),
                _ => throw new DataException($"Cannot convert {value.GetType().Name} to DateOnly.")
            };

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.DbType = DbType.Date;
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }
    }

    private sealed class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
    {
        public override TimeOnly Parse(object value) =>
            value switch
            {
                TimeSpan timeSpan => TimeOnly.FromTimeSpan(timeSpan),
                TimeOnly timeOnly => timeOnly,
                DateTime dateTime => TimeOnly.FromDateTime(dateTime),
                string text => TimeOnly.Parse(text),
                _ => throw new DataException($"Cannot convert {value.GetType().Name} to TimeOnly.")
            };

        public override void SetValue(IDbDataParameter parameter, TimeOnly value)
        {
            parameter.DbType = DbType.Time;
            parameter.Value = value.ToTimeSpan();
        }
    }
}

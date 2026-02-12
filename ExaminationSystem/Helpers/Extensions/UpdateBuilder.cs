using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ExaminationSystem.Helpers.Extensions;

public static class UpdateBuilder
{
    public static SetPropertyCalls<T> IfNotNull<T, TValue>(
        this SetPropertyCalls<T> setters,
        TValue? value,
        Expression<Func<T, TValue>> selector)
        where TValue : class
    {
        //if (value != null)
            //setters.SetProperty(selector, value);

        return setters;
    }

    public static SetPropertyCalls<T> IfNontDefault<T, TValue>(
        this SetPropertyCalls<T> setters,
        TValue value,
        Expression<Func<T, TValue>> selector)
        where TValue : struct 
    {
        //if (!EqualityComparer<TValue>.Default.Equals(value, default))
        //    setters.SetProperty(selector, value);

        return setters;

    }
}

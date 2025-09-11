using System.Linq.Expressions;

namespace TsvitFinances.Shared.Extensions;

public static class LinqExtensions
{
    public static IEnumerable<TSource> WhereWhen<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<TSource, bool> branch)
    {
        return condition
            ? source.Where(branch)
            : source;
    }

    public static IEnumerable<TSource> WhereWhenNot<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<TSource, bool> branch)
    {
        return !condition
            ? source.Where(branch)
            : source;
    }

    public static IQueryable<TSource> WhereWhen<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> branch)
    {
        return condition
            ? source.Where(branch)
            : source;
    }

    public static IQueryable<TSource> WhereWhenNot<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> branch)
    {
        return !condition
            ? source.Where(branch)
            : source;
    }
}
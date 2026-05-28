using AutoMapper.QueryableExtensions;

namespace ExaminationSystem.Helpers.Mapping;

public static class AutoMapperHelper
{
    public static IMapper Mapper { get; set; }

    public static T Map<T>(this object sourse)
    {
        return Mapper.Map<T>(sourse);
    }

    public static void Map<TSource, TDestenation>(TSource source, TDestenation destenation)
    {
        Mapper.Map(source, destenation);
    }

    public static IQueryable<TDestination> Project<TDestination>(this IQueryable<object> sourse)
    {
        return sourse.ProjectTo<TDestination>(Mapper.ConfigurationProvider);
    }
}

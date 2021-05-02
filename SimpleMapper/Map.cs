using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleMapper
{
    internal sealed class PropertyTypesCache : Dictionary<Type, PropertyInfo[]>
    {
        private static readonly Lazy<PropertyTypesCache>
            Lazy =
                new Lazy<PropertyTypesCache>
                    (() => new PropertyTypesCache());

        public static PropertyTypesCache Instance => Lazy.Value;
    }

    public static class MappingExtensions
    {
        public static IEnumerable<TOutput> MapCollection<TModel, TOutput>(this IEnumerable<TModel> models, params Action<TModel, TOutput>[] additionalMappings)
            where TOutput : class, new()
            where TModel : class, new()
        {
            var returnDtos = new List<TOutput>();
            foreach (var model in models) returnDtos.Add(model.Map(additionalMappings));

            return returnDtos;
        }

        /// <summary>
        /// Maps a class object of type <typeparamref name="TInput"/> to type of <typeparamref name="TOutput"/> by mapping common type names. Uses an internal memory cache for property names.
        ///
        /// Pass actions to <paramref name="additionalMappings"/> to perform additional mapping functions. These are run sequentially after the initial map.
        /// </summary>
        /// <param name="input">The input class to map from.</param>
        /// <param name="additionalMappings">(optional) Additional/custom mappings to perform on the class. These are run sequentially following the initial conversion.</param>
        /// <typeparam name="TInput">The type of the input class.</typeparam>
        /// <typeparam name="TOutput">The type of the output class to map to.</typeparam>
        /// <returns></returns>
        public static TOutput Map<TInput, TOutput>(this TInput input, params Action<TInput, TOutput>[] additionalMappings)
            where TOutput : class, new()
            where TInput : class, new()
        {
            var fromType = typeof(TInput);
            var toType = typeof(TOutput);
            var sourceProperties = GetProperties<TInput, TOutput>(fromType);
            var destinationProperties = GetProperties<TInput, TOutput>(toType);

            var dto = new TOutput();
            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.SingleOrDefault(x => x.Name == sourceProperty.Name &&
                                                                                     x.PropertyType == sourceProperty.PropertyType);
                if (destinationProperty != null) destinationProperty.SetValue(dto, sourceProperty.GetValue(input));
            }

            if (additionalMappings != null)
                foreach (var additionalMapping in additionalMappings)
                    additionalMapping(input, dto);

            return dto;
        }

        private static PropertyInfo[] GetProperties<TModel, TDto>(Type fromType) where TDto : class, new() where TModel : class, new()
        {
            var typesCache = PropertyTypesCache.Instance;
            PropertyInfo[] sourceProperties;
            //If we've already reflected the type before, don't do it again, just get the results from the type dictionary:
            if (typesCache.ContainsKey(fromType))
            {
                sourceProperties = typesCache[fromType];
            }
            else
            {
                //If this is the first time we've reflected this type, do getProperties(), then add the results to the cache:
                sourceProperties = fromType.GetProperties();
                typesCache[fromType] = sourceProperties;
            }

            return sourceProperties;
        }
    }
}
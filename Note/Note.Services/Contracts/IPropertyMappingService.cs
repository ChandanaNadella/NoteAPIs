namespace Note.Services.Contracts
{
    using Note.API.Common.Helpers;
    using System.Collections.Generic;

    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}

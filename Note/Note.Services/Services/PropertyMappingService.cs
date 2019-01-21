namespace Note.Services
{
    using Note.API.Common.Helpers;
    using Note.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DC = API.DataContracts;
    using R = Repository.Data.Entities;

    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _userPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Username", new PropertyMappingValue(new List<string>() { "Username" } )},
               //{ "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
               { "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
               //{ "FirstName", new PropertyMappingValue(new List<string>() { "FirstName" }) },
               //{ "LastName", new PropertyMappingValue(new List<string>() { "LastName" }) }
            };

        private Dictionary<string, PropertyMappingValue> _subscriberPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
                { "FullName", new PropertyMappingValue(new List<string>() { "FullName"}) }
           };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<DC.Responses.UserCreationResponse, R.User>(_userPropertyMapping));
            propertyMappings.Add(new PropertyMapping<DC.Responses.SubscriberCreationResponse, R.Subscriber>(_subscriberPropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;

        }
    }
}

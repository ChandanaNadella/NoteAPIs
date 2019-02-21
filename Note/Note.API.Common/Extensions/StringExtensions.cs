namespace Note.API.Common.Extensions
{
    using Note.API.Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;

    public static class StringExtensions
    {
        /// <summary>
        /// To get the parameters to orderBy from a common string which may have multiple OrderBy parameters.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="mappingDictionary"></param>

        public static String CreateSortParams(this String source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            string sortString = string.Empty;

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }
            // the orderBy string is separated by ",", so we split it.

            var orderByAfterSplit = orderBy.Split(',');

            int paramCounts = orderByAfterSplit.Count();
            int paramCounter = 0;
            // apply each orderby clause in reverse order - otherwise, the 
            // IQueryable will be ordered in the wrong order
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                ++paramCounter;
                // trim the orderByClause, as it might contain leading 
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var.
                var trimmedOrderByClause = orderByClause.Trim();

                // if the sort option ends with with " desc", we order
                // descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from the orderByClause, so we 
                // get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                // get the PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                int propertyCounts = propertyMappingValue.DestinationProperties.Count();
                int propertyCounter = 0;
                // Run through the property names in reverse
                // so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    ++propertyCounter;
                    // revert sort order if necessary
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    string alias = "o_n";

                    sortString += alias + "."+destinationProperty + (orderDescending ? " desc" : " asc");
                   // var aliasdone = alias + "." + sortString;

                    if(propertyCounts > 1)
                    {
                        if(propertyCounter < propertyCounts)
                        {
                            sortString += ", ";
                        }
                    }
                }

                if (paramCounts > 1)
                {
                    if (paramCounter < paramCounts)
                    {
                        sortString += ", ";
                    }
                }
            }
            return sortString;
        }
    }
}

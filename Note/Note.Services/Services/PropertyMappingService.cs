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

        private Dictionary<string, PropertyMappingValue> _notePropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               { "Id", new PropertyMappingValue(new List<string>() { "note_id" } ) },
              // { "Patient.PatientId", new PropertyMappingValue(new List<string>() { "patient_id" } ) },
               { "CreatedDate", new PropertyMappingValue(new List<string>() { "Date_entered" } ) },
              // { "Provider.ProviderId", new PropertyMappingValue(new List<string>() { "user_id" } ) },
               { "NoteClass", new PropertyMappingValue(new List<string>() { "note_class" } ) },
               { "NoteType", new PropertyMappingValue(new List<string>() { "note_type" } ) },
               { "NoteTypeId", new PropertyMappingValue(new List<string>() { "note_type_id" } ) },
               { "Description", new PropertyMappingValue(new List<string>() { "description" } ) },
               { "Note", new PropertyMappingValue(new List<string>() { "note" } ) },
               { "Color", new PropertyMappingValue(new List<string>() { "color" } ) },
               { "PostProcStatus", new PropertyMappingValue(new List<string>() { "post_proc_status" } ) },
               { "ModifiedDate", new PropertyMappingValue(new List<string>() { "date_modified" } ) },
               { "ModifiedBy", new PropertyMappingValue(new List<string>() { "modified_by" } ) },
               { "LockedEod", new PropertyMappingValue(new List<string>() { "locked_eod" } ) },
               { "Status", new PropertyMappingValue(new List<string>() { "status" } ) },
               { "ToothData", new PropertyMappingValue(new List<string>() { "tooth_data" } ) },
               { "ClaimID", new PropertyMappingValue(new List<string>() { "claim_id" } ) },
               { "StatementYn", new PropertyMappingValue(new List<string>() { "statement_yn" } ) },
               { "RespPartyId", new PropertyMappingValue(new List<string>() { "resp_party_id" } ) },
               { "Tooth", new PropertyMappingValue(new List<string>() { "tooth" } ) },
               { "TranNum", new PropertyMappingValue(new List<string>() { "tran_num" } ) },
               { "ArchiveName", new PropertyMappingValue(new List<string>() { "archive_name" } ) },
               { "ArchivePath", new PropertyMappingValue(new List<string>() { "archive_path" } ) },
               { "ServiceCode", new PropertyMappingValue(new List<string>() { "service_code" } ) },
               { "ClinicID", new PropertyMappingValue(new List<string>() { "practice_id" } ) },
               { "Freshness", new PropertyMappingValue(new List<string>() { "freshness" } ) },
               { "SurfaceDetail", new PropertyMappingValue(new List<string>() { "surface_detail" } ) },
               { "Surface", new PropertyMappingValue(new List<string>() { "surface" } ) },

           };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<DC.OperatoryNotes, R.operatory_notes>(_notePropertyMapping));
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

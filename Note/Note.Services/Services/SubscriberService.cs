namespace Note.Services
{
    using API.Common.Helpers;
    using API.Common.Messages;
    using AutoMapper;
    using Note.API.Common.Extensions;
    using Note.API.DataContracts.Requests;
    using Note.Services.Contracts;
    using Repository.Data;
    using Repository.Data.Entities;
    using System;
    using System.Linq;
    using DC = API.DataContracts;

    public class SubscriberService : ISubscriberService
    {
        private NoteDataContext _context = null;
        private readonly IPropertyMappingService _propertyMappingService;
        public SubscriberService(NoteDataContext context, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }
        public Tuple<bool, string> Create(SubscriberCreationRequest subscriberRequest)
        {
            if (string.IsNullOrWhiteSpace(subscriberRequest.Email))
                return Tuple.Create(false, "Email is required");

            if (_context.Subscriber.Any(x => x.Email == subscriberRequest.Email))
                return Tuple.Create(false, "Email \"" + subscriberRequest.Email + "\" is already taken");

            Subscriber subscriberEntity = new Subscriber()
            {
                FullName = subscriberRequest.FullName,
                Email = subscriberRequest.Email,
                Organization_Clinic_Name = subscriberRequest.Organization_Clinic_Name,
                CreatedDateUTC = DateTime.UtcNow,
                UpdatedDateUTC = DateTime.UtcNow,
                ContactNumber = subscriberRequest.ContactNumber,
                Comments = subscriberRequest.Comments,
                Timezone = TimeZoneInfo.Utc.StandardName,
                IsActive = true,
            };

            _context.Subscriber.Add(subscriberEntity);
            _context.SaveChanges();

            return Tuple.Create(true, "You have been successfully subscribed!");
        }

        public Tuple<PagedList<Subscriber>, bool, string> GetAll(SubscriberResourceParameters pageparams)
        {
            var collectionBeforePaging =
                _context.Subscriber
                .ApplySort(pageparams.OrderBy,
                _propertyMappingService.GetPropertyMapping<DC.Responses.SubscriberCreationResponse, Subscriber>());

            if (!string.IsNullOrEmpty(pageparams.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = pageparams.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.FullName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            var pagedCollection = new PagedList<Subscriber>();//PagedList<Subscriber>.Create(collectionBeforePaging, pageparams.PageNumber, pageparams.PageSize);

            return Tuple.Create(pagedCollection, true, AlertMessages.Subscriber_Success);
        }

        public Tuple<Subscriber, bool, string> GetById(int id)
        {
            var result = _context.Subscriber.SingleOrDefault(x => x.Id == id);
            if (result == null)
                return Tuple.Create(result, false, "No data found.");

            return Tuple.Create(result, true, "Data found.");
        }
    }
}

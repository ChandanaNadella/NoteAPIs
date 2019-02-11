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
    using System.Collections.Generic;
    using System.Linq;
    using DC = API.DataContracts;

    public class NoteService : INoteService
    {
        private NoteDataContext _context = null;
        private readonly IPropertyMappingService _propertyMappingService;

        public NoteService(NoteDataContext context, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

       
        #region ServiceMethods  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageparams"></param>
        /// <returns></returns>
        public IEnumerable<operatory_notes> getNotes(NoteResourceParameter pageparams)
        {
            var operatoryNotes = _context.GetOperatoryNotesByPatientIdByClinicIDByProviderId(pageparams.OperatoryNoteRequest.PatientId, pageparams.OperatoryNoteRequest.ClinicId, pageparams.OperatoryNoteRequest.UserId);

            //var collectionBeforePaging = operatoryNotes.ApplySort(pageparams.OrderBy,
            //    _propertyMappingService.GetPropertyMapping<DC.Responses.UserCreationResponse, User>());

            // var lstNote = _context.GetOperatoryNotesByPatientIdByClinicIDByProviderId(PatientId,ClinicId,ProviderId);

            //var collectionBeforePaging = _context.GetOperatoryNotesByPatientIdByClinicIDByProviderId(pageparams.patientId, pageparams.clinicId, pageparams.providerId)
            //  .ApplySort(pageparams.OrderBy, _propertyMappingService.GetPropertyMapping<DC.OperatoryNotes, operatory_notes>());

            // var collectionBeforePaging = _context.GetOperatoryNotesByPatientIdByClinicIDByProviderId(patientId,clinicId,providerId);



            return operatoryNotes;
            //return _context.order.operatoryNotes.Skip(NoteResourceParameter.PageSize
            //    * (NoteResourceParameter.PageNumber-1))
            //    .Take(NoteResourceParameter.PageSize)
            //    .ToList(); 

        }

        //Insert or Update
        public IEnumerable<operatory_notes> InsertOrUpdateNotes(operatory_notes operatoryNotes, int? autoNoteId)        {

            _context.InsertOrUpdateOperatoryNotes(operatoryNotes, autoNoteId);

            return null;

        }


        public Tuple<PagedList<User>, bool, string> GetAll(UserResourceParameters pageparams)
        {
            var collectionBeforePaging =
                _context.User
                .ApplySort(pageparams.OrderBy,
                _propertyMappingService.GetPropertyMapping<DC.Responses.UserCreationResponse, User>());

            if (pageparams.Clinic > 0)
            {
                // trim & ignore casing
                int clinicForWhereClause = pageparams.Clinic;
                // collectionBeforePaging = collectionBeforePaging
                //  .Where(a => a.ClinicId == clinicForWhereClause);
            }

            if (!string.IsNullOrEmpty(pageparams.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = pageparams.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Username.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            var pagedCollection = PagedList<User>.Create(collectionBeforePaging, pageparams.PageNumber, pageparams.PageSize);

            return Tuple.Create(pagedCollection, true, AlertMessages.User_Success);
        }

        public Tuple<User, bool> GetById(string id)
        {
            return Tuple.Create(_context.User.SingleOrDefault(x => x.Id.Equals(id) && x.IsActive && x.LockoutEnabled), true);
        }

       
       
        #endregion ServiceMethods
    }
}

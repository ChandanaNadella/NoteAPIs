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
        ///  Gets Operatory Notes based on parameters
        /// </summary>
        /// <param name="pageparams"></param>
        /// <returns> Get Operatory Notes data Based on ClinicID,PatientID,ProviderID</returns>
        
        #region Get ByID  
        public PagedList<operatory_notes> getNotes(NoteResourceParameter pageparams)
        {
            var orderBy =
              pageparams.OrderBy
               .CreateSortParams(pageparams.OrderBy,
               _propertyMappingService.GetPropertyMapping<DC.OperatoryNotes, operatory_notes>());

            var operatoryNotes = _context.GetOperatoryNotesByPatientIdByClinicIDByUserId(pageparams.OperatoryNoteRequest.PatientId, pageparams.OperatoryNoteRequest.ClinicId, pageparams.OperatoryNoteRequest.UserId, OrderBy: orderBy, pageSize: pageparams.PageSize, currentPage: pageparams.PageNumber);

            var pagedCollection = PagedList<operatory_notes>.Create(operatoryNotes.Item1, pageparams.PageNumber, pageparams.PageSize, operatoryNotes.Item2);

            return pagedCollection;
        }

        #endregion Get ByID  

        /// <summary>
        /// Insert or updates data into Operatory Notes 
        /// </summary>
        /// <param name="operatoryNotes"></param>
        /// <param name="autoNoteId"></param>

        #region InsertOrUpdateNotes  
        public string InsertOrUpdateNotes(operatory_notes operatoryNotes, int? autoNoteId, string noteType)
        {

           return  _context.InsertOrUpdateOperatoryNotes(operatoryNotes, autoNoteId,noteType);

        }
        #endregion InsertOrUpdateNotes  

        #endregion ServiceMethods
    }
}

namespace Note.Services.Contracts
{
    using Note.API.Common.Helpers;
    using Note.API.DataContracts.Requests;
    using Note.Repository.Data.Entities;
    using System.Collections.Generic;

    public interface INoteService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageparams"></param>
        /// <returns></returns>
        PagedList<operatory_notes> getNotes(NoteResourceParameter pageparams);

        /// <summary>
        /// To Insert or Update data into Operatory Notes
        /// </summary>
        /// <param name="operatoryNotesToUpdate"></param>
        /// <param name="autoNoteId"></param>
        /// <returns></returns>
        IEnumerable<operatory_notes> InsertOrUpdateNotes(operatory_notes operatoryNotesToUpdate, int? autoNoteId, string noteType);


    }
}

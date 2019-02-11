namespace Note.Services.Contracts
{
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
        IEnumerable<operatory_notes> getNotes(NoteResourceParameter pageparams);

        IEnumerable<operatory_notes> InsertOrUpdateNotes(operatory_notes operatoryNotesToUpdate, int? autoNoteId);


    }
}

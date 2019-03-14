namespace Note.API.DataContracts.Requests
{
    public class NoteResourceParameter
    {
        public NoteResourceParameter()
        {
            OperatoryNoteRequest = new OperatoryNotesRequest();
        }
        const int maxPageSize = 40;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public OperatoryNotesRequest OperatoryNoteRequest { get; set; }

        //public string SearchQuery { get; set; }

        public string OrderBy { get; set; } = "CreatedDate desc";

        public string Fields { get; set; }
    }
}
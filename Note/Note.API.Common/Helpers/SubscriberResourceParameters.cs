
namespace Note.API.Common.Helpers
{
    public class SubscriberResourceParameters
    {
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

        public string SearchQuery { get; set; }
        public string OrderBy { get; set; } = "FullName";

        public string Fields { get; set; }
    }
}

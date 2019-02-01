﻿using System.ComponentModel.DataAnnotations;

namespace Note.API.Common.Helpers
{
    public class UserResourceParameters
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

        public int Clinic { get; set; }

        [Required]
        public string patientId { get; set; }
        public string clinicId { get; set; }
        public string providerId { get; set; }
        
        public string SearchQuery { get; set; }
        public string OrderBy { get; set; } = "Name";

        public string Fields { get; set; }
    }
}

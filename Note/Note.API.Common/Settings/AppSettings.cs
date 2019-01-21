using System;
using System.Collections.Generic;
using System.Text;

namespace Note.API.Common.Settings
{
    public class AppSettings
    {
        public API API { get; set; }
        public Swagger Swagger { get; set; }

        public string ConnectionString { get; set; }

        public string Secret { get; set; }
    }

    public class API
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Swagger
    {
        public bool Enabled { get; set; }
    }
}

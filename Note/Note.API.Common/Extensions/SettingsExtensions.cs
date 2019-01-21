using Note.API.Common.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Note.API.Common.Extensions
{
    public static class SettingsExtensions
    {
        public static bool IsValid(this AppSettings data)
        {
            bool result = true;

            if (data == null)
            {
                result = false;
                Debug.WriteLine("AppSettings object is null");
            }

            if (data.Swagger == null)
            {
                result = false;
                Debug.WriteLine("AppSettings does not contain Swagger settings");
            }

            if (data.API == null)
            {
                result = false;
                Debug.WriteLine("AppSettings does not contain API settings");
            }

            if (string.IsNullOrEmpty(data.API.Title))
            {
                result = false;
                Debug.WriteLine("API's Title is empty");
            }

            return result;
        }
    }
}

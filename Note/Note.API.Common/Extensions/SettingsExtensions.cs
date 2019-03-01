namespace Note.API.Common.Extensions
{
    using Note.API.Common.Settings;
    using System.Diagnostics;

    public static class SettingsExtensions
    {
        public static string DBConnectionString { get; set; }
        public static bool IsDBConnected { get; set; }

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

            if (string.IsNullOrEmpty(data.ConnectionString))
            {
                result = false;
                Debug.WriteLine("AppSettings does not contain database connection settings");
            }
            else
            {
                DBConnectionString = data.ConnectionString;
            }

            IsDBConnected = data.IsDatabaseConnected;


            return result;
        }
    }
}

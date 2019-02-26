namespace Note.API.Common.Messages
{
    public static class AlertMessages
    {
        public const string PatientId_Required = "Patient Id is Required Field.";
        public const string ClinicId_Required = "Clinic Id / Practice Id is Required Field.";
        public const string ProviderId_Required = "Provider Id /User Id is Required Field.";
        public const string NoteType_Required = "Note Type is Required Field.";
        public const string NoteId_Required = "Note Id is Required Field.";
        public const string Noteclass_Required = "Note class is Required Field.";

        public const string PatientId_Invalid = "Patient Id should not have more than 5 characters.";
        public const string ProviderId_Invalid ="Provider Id should not have more than 5 characters.";
        public const string NoteType_Invalid = "Note Type should have only one character.";
        public const string Noteclass_Invalid = "Note class should have only one character.";
    }
}

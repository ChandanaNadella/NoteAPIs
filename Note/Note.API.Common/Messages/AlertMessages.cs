namespace Note.API.Common.Messages
{
    public static class AlertMessages
    {
        public const string patientId_Required = "Patient Id is Required Field.";
        public const string clinicId_Required = "Clinic Id / Practice Id is Required Field.";
        public const string providerId_Required = "Provider Id /User Id is Required Field.";
        public const string noteId_Required = "Note Id is Required Field.";
        public const string noteType_Required = "Note Type is Required Field.";
        public const string noteClass_Required = "Please enter the Note Class as 'T'.";
        public const string color_Required = "Color is Required Field.";
        public const string locked_Eod_Required = "Locked Eod is Required Field.";
        public const string claimId_Required = "Claim Id is Required Field.";
        public const string resp_Party_Id_Required = "Resp Party Id is Required Field.";

        public const string clinicId_Range = "Clinic Id / Practice Id is not in range.";
        public const string noteId_Range = "Note Id is not in range.";
        public const string color_Range = "Color is not in range.";
        public const string locked_Eod_Range = "Locked Eod is not in range.";
        public const string claimId_Range = "Claim Id is  not in range.";
        public const string tranNum_Range = "Tran Number is not in range.";



        public const string noteId_Invalid = "Note Id is Invalid.";
        public const string patientId_Invalid = "Patient Id is Invalid.";
        public const string providerId_Invalid = "Provider Id is Invalid.";
        public const string noteclass_Invalid = "Note Class should be of type 'T'.";
        public const string noteType_Invalid = "Note Type is Invalid.";
        public const string color_Invalid = "Color is Invalid.";
        public const string post_proc_status_Invalid = "Post_proc_status should be 'U' or 'v'.";
        public const string locked_Eod_Invalid = "Locked Eod is Invalid.";
        public const string status_Invalid = "Status is Invalid.";
        public const string tooth_Data_Invalid = "Tooth Data is Invalid.";
        public const string claimId_Invalid = "Claim Id is Invalid.";
        public const string statement_Yn_Invalid = "Statement is Invalid.";
        public const string resp_Party_Id_Invalid = "Resp Party Id By is Invalid.";
        public const string tooth_Invalid = "Tooth is Invalid.";
        public const string tranNum_Invalid = "Tran Number is Invalid.";
        public const string archive_Name_Invalid = "Archive Name is Invalid.";
        public const string archive_Path_Invalid = "Archive Path is Invalid.";
        public const string service_Code_Invalid = "Service Code  is Invalid.";
        public const string clinicId_Invalid = "Clinic Id / Practice Id is Invalid.";
        public const string surface_Detail_Invalid = "Surface Detail  is Invalid.";
        public const string surface_Invalid = "Surface is Invalid.";


    }
    //Api Response Error Code and messages
    public static class SuccessResponse
    {
        public const string Code = "200";

        public const string Message = "OK";

    }

    public static class InternalServerError
    {
        public const int Code = 500;
        public const string CodeString = "500";

        public const string Message = "An unexpected error happened ! Please try again later.";
        public const string DBConFailedMessage = "Unable to connect the database.";
    }

    public static class BadRequestResponse
    {

        public const string Code = "400";

        public const string Message = "Bad Request";

    }
    public static class NotFoundResponse
    {
        public const string Code = "404";

        public const string Message = "Not Found";

    }
    public static class NoContentResponse
    {
        public const int Code = 204;
        public const string CodeString = "204";

        public const string Message = "No Content";
        public const string DBConFailedMessage = "No Content.";

    }
    public static class UnprocessableEntityResponse
    {
        public const string Code = "422";

        public const string Message = "Unprocessable Entity";

    }
}
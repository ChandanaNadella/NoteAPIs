namespace Note.API.DataContracts.Requests

{
    using Microsoft.Extensions.Logging;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    public class CustomErrorAttribute : ValidationAttribute
    {
        private ILogger _logger;

        public CustomErrorAttribute(
          ILogger logger)

        {
            _logger = logger;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {

            var required = new RequiredAttribute();
            var valid = required.IsValid(Convert.ToString(value).Trim());

            if (!valid)
            {
                WriteToFileErrors();
                return new ValidationResult(ErrorMessage);
            }
            else
            {

                if (validationContext.MemberName == "ClinicId")
                {

                    var regex = new RegularExpressionAttribute("^[1-9]\\d*$");
                    var validregex = regex.IsValid(value);


                    if (!validregex)
                    {
                        WriteToFileErrors();
                        var MESSAGE = validationContext.DisplayName + " is Invalid";
                        // NLOGGER with mssage
                        return new ValidationResult(MESSAGE);

                    }

                    else
                    {
                        //Int64 integer;
                        // bool validClinicId = Int64.TryParse( Convert.ToString(value), out integer);
                        int validClinicId = Convert.ToInt32(value);


                        if (validClinicId > 65535)
                        {
                            WriteToFileErrors();
                            return new ValidationResult(validationContext.DisplayName + " is Invalid");
                        }
                    }


                }

                else if (validationContext.MemberName == "ProviderId")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_pro = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,3}");
                    var validregex_pro = regex_pro.IsValid(value);
                    if (!validregex_pro)
                    {
                        WriteToFileErrors();
                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }


                }
                else if (validationContext.MemberName == "PatientId" || validationContext.MemberName == "patient_id")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_pat = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,5}");
                    var validregex_pat = regex_pat.IsValid(value);
                    if (!validregex_pat)
                    {
                        WriteToFileErrors();

                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }


                }
                else if (validationContext.MemberName == "note_id")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex = new RegularExpressionAttribute("^[0-9]\\d*$");
                    var validregex = regex.IsValid(value);


                    if (!validregex)
                    {
                        WriteToFileErrors();
                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }

                    else
                    {
                        //Int64 integer;
                        // bool validClinicId = Int64.TryParse( Convert.ToString(value), out integer);
                        Int64 validNoteId = Convert.ToInt64(value);


                        if (validNoteId > 4294967295)
                        {
                            WriteToFileErrors();
                            return new ValidationResult(validationContext.DisplayName + " is Invalid");
                        }
                    }



                }
                else if (validationContext.MemberName == "note_type")
                {

                    var regex_note = new RegularExpressionAttribute("^([a-zA-Z1-9]){1}");
                    var validregex_note = regex_note.IsValid(value);
                    if (!validregex_note)
                    {
                        WriteToFileErrors();

                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }

                }


                else if (validationContext.MemberName == "tooth")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_tooth = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,10}");
                    var validregex_tooth = regex_tooth.IsValid(value);
                    if (!validregex_tooth)
                    {
                        WriteToFileErrors();
                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }


                }
                else if (validationContext.MemberName == "color")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex = new RegularExpressionAttribute("^[0-9]\\d*$");
                    var validregex = regex.IsValid(value);


                    if (!validregex)
                    {
                        WriteToFileErrors();
                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }

                    else
                    {
                        //Int64 integer;
                        // bool validClinicId = Int64.TryParse( Convert.ToString(value), out integer);
                        Int64 validNoteId = Convert.ToInt64(value);


                        if (validNoteId > 4294967295)
                        {
                            WriteToFileErrors();
                            return new ValidationResult(validationContext.DisplayName + " is Invalid");
                        }
                    }



                }
                else if (validationContext.MemberName == "surface")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_surface = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,8}");
                    var validregex_surface = regex_surface.IsValid(value);
                    if (!validregex_surface)
                    {
                        WriteToFileErrors();
                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }


                }
            }
            //if there is no validations error.
            return null;
        }



        public void WriteToFileErrors()
        {

            _logger.LogError(string.Format("Date : {0}, Error Status Code: 400, Error Status Message: Bad Request", DateTime.Now.ToString()));
            _logger.LogError(string.Format("Error Occurred :{0}", ErrorMessage));
            _logger.LogInformation("-----------------------------------------------------------------------------");


        }



    }


}

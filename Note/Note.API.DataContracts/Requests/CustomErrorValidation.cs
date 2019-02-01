using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using System.Collections;

namespace Note.API.DataContracts.Requests
{
    public class CustomErrorAttribute : ValidationAttribute
    {
       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var required = new RequiredAttribute();
            var valid = required.IsValid(Convert.ToString(value).Trim());

            if (!valid)
            {
                //Validation for ClinicId, PatientId and ProviderId not be blank.
                string filePath = @"..\Note.API.Common\ErrorLogs\Error.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine("Error Occurred :{0}", ErrorMessage);
                    writer.WriteLine("Error Status Code: 400, Error Status Message: Bad Request" );
                    writer.WriteLine("-----------------------------------------------------------------------------");
                }

                return new ValidationResult(ErrorMessage);
            }
            else
            {
                //Validation for ClinicId and PatientId to take only positive integer value. 

                if (validationContext.MemberName == "ClinicId")
                {

                    var regex = new RegularExpressionAttribute("^[1-9]\\d*$");
                    var validregex = regex.IsValid(value);
                    if (!validregex)
                    {
                        string filePath = @"..\Note.API.Common\ErrorLogs\Error.txt";
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("Date : " + DateTime.Now.ToString());
                            writer.WriteLine("Error Occurred :{0}", validationContext.DisplayName + " is Invalid");
                            writer.WriteLine("Error Status Code: 400, Error Status Message: Bad Request");
                            writer.WriteLine("-----------------------------------------------------------------------------");
                        }

                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }
                }
                else if (validationContext.MemberName == "ProviderId")
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_pro = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,3}");
                    var validregex_pro = regex_pro.IsValid(value);
                    if (!validregex_pro)
                    {
                        string filePath = @"..\Note.API.Common\ErrorLogs\Error.txt";  
                        
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("Date : " + DateTime.Now.ToString());
                            writer.WriteLine("Error Occurred :{0}", validationContext.DisplayName + " is Invalid");
                            writer.WriteLine("Error Status Code: 400, Error Status Message: Bad Request");
                            writer.WriteLine("-----------------------------------------------------------------------------");
                        }

                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }


                }
                else 
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_pat = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,5}");
                    var validregex_pat = regex_pat.IsValid(value);
                    if (!validregex_pat)
                    {
                        string filePath = @"..\Note.API.Common\ErrorLogs\Error.txt";

                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("Date : " + DateTime.Now.ToString());
                            writer.WriteLine("Error Occurred :{0}", validationContext.DisplayName + " is Invalid");
                            writer.WriteLine("Error Status Code: 400, Error Status Message: Bad Request");
                            writer.WriteLine("-----------------------------------------------------------------------------");
                        }

                        return new ValidationResult(validationContext.DisplayName + " is Invalid");

                    }


                }


            }
            //if there is no validations error.
            return null;
        }
    }
}

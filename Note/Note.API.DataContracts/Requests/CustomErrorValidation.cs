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
                WriteToFileErrors();
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                //int Clinicid = Convert.ToInt32(valid);
                //Validation for ClinicId and PatientId to take only positive integer value. 

                if (validationContext.MemberName == "ClinicId")
                {

                    var regex = new RegularExpressionAttribute("^[1-9]\\d*$");
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
                else 
                {   //Validation for ProviderId to take AlphaNumeric characters and min=1 and max=3. 

                    var regex_pat = new RegularExpressionAttribute("^([a-zA-Z0-9]){1,5}");
                    var validregex_pat = regex_pat.IsValid(value);
                    if (!validregex_pat)
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
            string filePath = @"..\Note.API.Common\ErrorLogs\Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine("Error Occurred :{0}", ErrorMessage);
                writer.WriteLine("Error Status Code: 400, Error Status Message: Bad Request");
                writer.WriteLine("-----------------------------------------------------------------------------");
            }
            
        }

        

    }

    
}

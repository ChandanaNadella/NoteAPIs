using Note.API.DataContracts;
using Note.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Note.Services.Services
{
    public class TokenService : ITokenService
    {
        public string GetToken(TokenDetails tokenDetails)
        {

            // create an EaglesoftSettings object
            Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
            dynamic settings = Activator.CreateInstance(esSettingsType);

            // get product name and token from the controls
            // set the EaglesoftSettings token
            bool tokenIsValid = settings.SetToken(tokenDetails.ProductName, tokenDetails.Token);

            // the return value is true if the token is valid
            if (tokenIsValid)
            {
                // retrieve the connection string available for your database access level
                // send true for the primary database, false for the secondary database
                return settings.GetLegacyConnectionString(true);
            }
            else
            {
                return (@"Invalid token.");
            }

        }


    }

}
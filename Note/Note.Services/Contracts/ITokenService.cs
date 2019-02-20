using Note.API.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Note.Services.Contracts
{
    public interface ITokenService
    {
        string GetToken(TokenDetails tokenDetails);
    }
}

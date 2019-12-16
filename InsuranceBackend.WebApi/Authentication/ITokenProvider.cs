using InsuranceBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Authentication
{
    public interface ITokenProvider
    {
        string CreateToken(SystemUser user, DateTime expiry);
        TokenValidationParameters GetValidationParameters();
    }
}

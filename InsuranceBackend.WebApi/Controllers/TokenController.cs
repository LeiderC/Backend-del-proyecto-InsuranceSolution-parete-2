using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Authentication;
using InsuranceBackend.WebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InsuranceBackend.WebApi.Controllers
{
    [Route("api/token")]
    public class TokenController
    {
        private readonly IUnitOfWork _unitOfWork;
        private ITokenProvider _tokenProvider;
        public TokenController(ITokenProvider tokenProvider, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        public JsonWebToken Post([FromBody]SystemUser userLogin)
        {
            try
            {
                var user = _unitOfWork.User.ValidateUserPassword(userLogin.Login, userLogin.Password);
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }
                string detail = string.Format("Login correcto usuario: {0}", user.FirstName + " " + user.LastName);
                Audit.InsertAudit(_unitOfWork, "F", user.Id, "I", detail);
                return new JsonWebToken
                {
                    Access_Token = _tokenProvider.CreateToken(user, DateTime.UtcNow.AddHours(8)),
                    Expires_in = 480 //minutes
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

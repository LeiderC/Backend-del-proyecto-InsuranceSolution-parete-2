using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            //var user = _unitOfWork.User.ValidateUser(userLogin.Login, userLogin.Password);
            var user = _unitOfWork.User.ValidateUserPassword(userLogin.Login, userLogin.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return new JsonWebToken
            {
                Access_Token = _tokenProvider.CreateToken(user, DateTime.UtcNow.AddHours(8)),
                Expires_in = 480 //minutes
            };
        }
    }
}

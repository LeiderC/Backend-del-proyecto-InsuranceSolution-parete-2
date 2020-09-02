using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Authentication;
using InsuranceBackend.WebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InsuranceBackend.WebApi.Controllers
{
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ITokenProvider _tokenProvider;
        public TokenController(ITokenProvider tokenProvider, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        public IActionResult Post([FromBody] SystemUser userLogin)
        {
            try
            {
                SystemUser user = null;
                if (userLogin.External)
                {
                    ExternalUser externalUser = _unitOfWork.ExternalUser.ValidateUserPassword(userLogin.Login, userLogin.Password);
                    if (externalUser != null)
                    {
                        user = new SystemUser
                        {
                            Id = externalUser.Id,
                            FirstName = externalUser.FirstName,
                            LastName = externalUser.LastName,
                            FullName = externalUser.FirstName + " " + externalUser.LastName,
                            Roles = "external"
                        };
                    }
                }
                else
                {
                    user = _unitOfWork.User.ValidateUserPassword(userLogin.Login, userLogin.Password);
                }
                if (user == null)
                {
                    return StatusCode(500, "Los datos de usuario y contraseña no concuerdan");
                }
                if (!userLogin.External)
                {
                    string detail = string.Format("Login correcto usuario: {0}", user.FirstName + " " + user.LastName);
                    Audit.InsertAudit(_unitOfWork, "F", user.Id, "I", detail);
                }
                return Ok(new JsonWebToken
                {
                    Access_Token = _tokenProvider.CreateToken(user, DateTime.UtcNow.AddHours(8)),
                    Expires_in = 480 //minutes
                });
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

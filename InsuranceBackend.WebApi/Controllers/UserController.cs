using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.DataAccess.Password;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.User.GetAllUsers(false));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserSalesman")]
        public IActionResult GetSalesman()
        {
            try
            {
                IEnumerable<SystemUser> lst = _unitOfWork.User.GetAllUsers(true);
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                SystemUser user = _unitOfWork.User.GetById(id);
                UserProfile profile = _unitOfWork.UserProfile.UserProfileByUser(user.Id);
                user.IdProfile = profile.IdProfile;
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedUser/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedUser(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.User.UserPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("CheckPermissions")]
        public IActionResult CheckPermissions([FromBody]GetCheckPermissions request)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.User.CheckPermissions(int.Parse(idUser), request.Menu, request.Submenu, request.Action));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody]ChangePassword request)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                SystemUser user = _unitOfWork.User.GetById(int.Parse(idUser));
                if (user != null)
                {
                    SystemUser _user = _unitOfWork.User.ValidateUserPassword(user.Login, request.Password);
                    if (_user != null)
                    {
                        // Actualizamos la contraseña
                        _user.Password = request.NewPassword;
                        _user.ChangePassword = false;
                        HashSalt salt = new HashSalt();
                        salt = PasswordUtil.GenerateSaltedHash(32, _user.Password);
                        _user.Password = salt.Hash;
                        _user.Help = salt.Salt;
                        return Ok(_unitOfWork.User.Update(_user));
                    }
                    else
                    {
                        return StatusCode(500, "La clave ingresada no corresponde a la clave del usuario");
                    }
                }
                return StatusCode(500, "No se encuentra el usuario");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]SystemUser user)
        {
            int idUser = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    HashSalt salt = new HashSalt();
                    salt = PasswordUtil.GenerateSaltedHash(32, user.Password);
                    //Hash = password
                    //Salt = help
                    user.Password = salt.Hash;
                    user.Help = salt.Salt;
                    idUser = _unitOfWork.User.Insert(user);
                    //UserProfile
                    UserProfile userProfile = new UserProfile
                    {
                        IdUser = idUser,
                        IdProfile = user.IdProfile
                    };
                    _unitOfWork.UserProfile.Insert(userProfile);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idUser);
        }

        [HttpPut]
        public IActionResult Put([FromBody]SystemUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    HashSalt salt = new HashSalt();
                    salt = PasswordUtil.GenerateSaltedHash(32, user.Password);
                    //Hash = password
                    //Salt = help
                    user.Password = salt.Hash;
                    user.Help = salt.Salt;
                    _unitOfWork.User.Update(user);
                    //UserProfile
                    UserProfile userProfile = _unitOfWork.UserProfile.UserProfileByUser(user.Id);
                    _unitOfWork.UserProfile.Delete(userProfile);
                    userProfile.IdProfile = user.IdProfile;
                    _unitOfWork.UserProfile.Insert(userProfile);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(new { Message = "El usuario se ha actualizado" });
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]SystemUser user)
        {
            if (user.Id > 0)
                return Ok(_unitOfWork.User.Delete(user
                    ));
            else
                return BadRequest();
        }
    }
}

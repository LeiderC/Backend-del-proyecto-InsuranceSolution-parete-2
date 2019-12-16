using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.DataAccess.Password;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
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
                return Ok(_unitOfWork.User.GetAllUsers());
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
                return Ok(_unitOfWork.User.GetById(id));
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
                    UserProfile userProfile = new UserProfile();
                    userProfile.IdUser = idUser;
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
            return Ok(idUser);
        }

        [HttpPut]
        public IActionResult Put([FromBody]SystemUser user)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.User.Update(user))
                {
                    return Ok(new { Message = "El usuario se ha actualizado" });
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
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

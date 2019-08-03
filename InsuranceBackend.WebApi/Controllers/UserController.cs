using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.User.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedUser/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedUser(int page, int rows)
        {
            return Ok(_unitOfWork.User.UserPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.User.Insert(user));
        }

        [HttpPut]
        public IActionResult Put([FromBody]User user)
        {
            if (ModelState.IsValid && _unitOfWork.User.Update(user))
            {
                return Ok(new { Message = "El usuario se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]User user)
        {
            if (user.Id > 0)
                return Ok(_unitOfWork.User.Delete(user
                    ));
            else
                return BadRequest();
        }
    }
}

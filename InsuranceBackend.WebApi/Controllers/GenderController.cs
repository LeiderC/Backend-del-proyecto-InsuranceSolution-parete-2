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
    [Route("api/gender")]
    [Authorize]
    public class GenderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Gender.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedGender/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedGender(int page, int rows)
        {
            return Ok(_unitOfWork.Gender.GenderPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Gender gender)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Gender.Insert(gender));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Gender gender)
        {
            if (ModelState.IsValid && _unitOfWork.Gender.Update(gender))
            {
                return Ok(new { Message = "El genero se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Gender gender)
        {
            if (gender.Id > 0)
                return Ok(_unitOfWork.Gender.Delete(gender
                    ));
            else
                return BadRequest();
        }
    }
}
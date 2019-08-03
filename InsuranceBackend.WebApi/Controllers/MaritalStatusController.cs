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
    [Route("api/maritalStatus")]
    [Authorize]
    public class MaritalStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaritalStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.MaritalStatus.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedMaritalStatus/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedMaritalStatus(int page, int rows)
        {
            return Ok(_unitOfWork.MaritalStatus.MaritalStatusPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]MaritalStatus maritalStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.MaritalStatus.Insert(maritalStatus));
        }

        [HttpPut]
        public IActionResult Put([FromBody]MaritalStatus maritalStatus)
        {
            if (ModelState.IsValid && _unitOfWork.MaritalStatus.Update(maritalStatus))
            {
                return Ok(new { Message = "El estado civil se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]MaritalStatus maritalStatus)
        {
            if (maritalStatus.Id > 0)
                return Ok(_unitOfWork.MaritalStatus.Delete(maritalStatus
                    ));
            else
                return BadRequest();
        }
    }
}
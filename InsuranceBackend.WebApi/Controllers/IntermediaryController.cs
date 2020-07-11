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
    [Route("api/intermediary")]
    [Authorize]
    public class IntermediaryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IntermediaryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Intermediary.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedIntermediary/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedIntermediary(int page, int rows)
        {
            return Ok(_unitOfWork.Intermediary.IntermediaryPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Intermediary intermediary)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Intermediary.Insert(intermediary));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Intermediary intermediary)
        {
            if (ModelState.IsValid && _unitOfWork.Intermediary.Update(intermediary))
            {
                return Ok(new { Message = "El intermediario se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Intermediary intermediary)
        {
            if (intermediary.Id > 0)
                return Ok(_unitOfWork.Intermediary.Delete(intermediary
                    ));
            else
                return BadRequest();
        }
    }
}
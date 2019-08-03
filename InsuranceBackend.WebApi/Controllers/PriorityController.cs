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
    [Route("api/priority")]
    [Authorize]
    public class PriorityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PriorityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Priority.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedPriority/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPriority(int page, int rows)
        {
            return Ok(_unitOfWork.Priority.PriorityPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Priority priority)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Priority.Insert(priority));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Priority priority)
        {
            if (ModelState.IsValid && _unitOfWork.Priority.Update(priority))
            {
                return Ok(new { Message = "El prioridad se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Priority priority)
        {
            if (priority.Id > 0)
                return Ok(_unitOfWork.Priority.Delete(priority
                    ));
            else
                return BadRequest();
        }
    }
}
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
            try
            {
                return Ok(_unitOfWork.Priority.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedPriority/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPriority(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Priority.PriorityPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]Priority priority)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.Priority.Insert(priority));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]Priority priority)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Priority.Update(priority))
            {
                return Ok(new { Message = "Prioridad se ha actualizado" });
            }
            else
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var priority = _unitOfWork.Priority.GetById(id);
                if (priority == null)
                    return NotFound();
                if (_unitOfWork.Priority.Delete(priority))
                    return Ok(new { Message = "Prioridad se ha eliminado" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
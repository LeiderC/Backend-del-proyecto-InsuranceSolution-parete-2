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
    [Route("api/taskAbout")]
    [Authorize]
    public class TaskAboutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskAboutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.TaskAbout.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedTaskAbout/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedTaskAbout(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.TaskAbout.TaskAboutPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]TaskAbout taskAbout)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.TaskAbout.Insert(taskAbout));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]TaskAbout taskAbout)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.TaskAbout.Update(taskAbout))
                {
                    return Ok(new { Message = "Tarea sobre, se ha actualizado" });
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
                var taskAbout = _unitOfWork.TaskAbout.GetById(id);
                if (taskAbout == null)
                    return NotFound();
                if (_unitOfWork.TaskAbout.Delete(taskAbout))
                    return Ok(new { Message = "Tarea sobre se ha eliminado" });
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
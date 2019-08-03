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
            return Ok(_unitOfWork.TaskAbout.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedTaskAbout/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedTaskAbout(int page, int rows)
        {
            return Ok(_unitOfWork.TaskAbout.TaskAboutPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]TaskAbout taskAbout)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.TaskAbout.Insert(taskAbout));
        }

        [HttpPut]
        public IActionResult Put([FromBody]TaskAbout taskAbout)
        {
            if (ModelState.IsValid && _unitOfWork.TaskAbout.Update(taskAbout))
            {
                return Ok(new { Message = "El tarea sobre, se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]TaskAbout taskAbout)
        {
            if (taskAbout.Id > 0)
                return Ok(_unitOfWork.TaskAbout.Delete(taskAbout
                    ));
            else
                return BadRequest();
        }
    }
}
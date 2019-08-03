using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = InsuranceBackend.Models.Task;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/task")]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Task.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedTask/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedTask(int page, int rows)
        {
            return Ok(_unitOfWork.Task.TaskPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Task task)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Task.Insert(task));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Task task)
        {
            if (ModelState.IsValid && _unitOfWork.Task.Update(task))
            {
                return Ok(new { Message = "La tarea se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Task task)
        {
            if (task.Id > 0)
                return Ok(_unitOfWork.Task.Delete(task
                    ));
            else
                return BadRequest();
        }
    }
}

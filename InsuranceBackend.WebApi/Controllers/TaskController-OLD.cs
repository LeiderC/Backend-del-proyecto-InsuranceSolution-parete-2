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
            try
            {
                return Ok(_unitOfWork.Task.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedTask/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedTask(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Task.TaskPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Task task)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                int idTask = _unitOfWork.Task.Insert(task);
                //Debemos insertar en ManagementTask
                ManagementTask mt = new ManagementTask { IdManagement = task.IdManagement, IdTask = idTask };
                return Ok(_unitOfWork.ManagementTask.Insert(mt));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Task task)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Task.Update(task))
                {
                    return Ok(new { Message = "La tarea se ha actualizado" });
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

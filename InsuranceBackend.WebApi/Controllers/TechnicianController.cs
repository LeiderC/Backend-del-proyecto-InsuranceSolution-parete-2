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
    [Route("api/technician")]
    [Authorize]
    public class TechnicianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TechnicianController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Technician.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedTechnician/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedTechnician(int page, int rows)
        {
            return Ok(_unitOfWork.Technician.TechnicianPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Technician technician)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Technician.Insert(technician));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Technician technician)
        {
            if (ModelState.IsValid && _unitOfWork.Technician.Update(technician))
            {
                return Ok(new { Message = "la tecnica se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Technician technician)
        {
            if (technician.Id > 0)
                return Ok(_unitOfWork.Technician.Delete(technician
                    ));
            else
                return BadRequest();
        }
    }
}
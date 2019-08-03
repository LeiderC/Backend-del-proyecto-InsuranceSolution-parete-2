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
    [Route("api/occupation")]
    [Authorize]
    public class OccupationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OccupationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Occupation.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedOccupation/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedOccupation(int page, int rows)
        {
            return Ok(_unitOfWork.Occupation.OccupationPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Occupation occupation)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Occupation.Insert(occupation));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Occupation occupation)
        {
            if (ModelState.IsValid && _unitOfWork.Occupation.Update(occupation))
            {
                return Ok(new { Message = "La ocupacion se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Occupation occupation)
        {
            if (occupation.Id > 0)
                return Ok(_unitOfWork.Occupation.Delete(occupation
                    ));
            else
                return BadRequest();
        }
    }
}
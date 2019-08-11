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
            try
            {
                return Ok(_unitOfWork.Occupation.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedOccupation/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedOccupation(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Occupation.OccupationPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]Occupation occupation)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.Occupation.Insert(occupation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]Occupation occupation)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Occupation.Update(occupation))
            {
                return Ok(new { Message = "La ocupacion se ha actualizado" });
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
                var occupation = _unitOfWork.Occupation.GetById(id);
                if (occupation == null)
                    return NotFound();
                if (_unitOfWork.Occupation.Delete(occupation))
                    return Ok(new { Message = "La ocupacion se ha eliminado" });
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
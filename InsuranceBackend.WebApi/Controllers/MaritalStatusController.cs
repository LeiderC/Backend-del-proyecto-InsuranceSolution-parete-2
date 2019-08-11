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
    [Route("api/maritalStatus")]
    [Authorize]
    public class MaritalStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaritalStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.MaritalStatus.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedMaritalStatus/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedMaritalStatus(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.MaritalStatus.MaritalStatusPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]MaritalStatus maritalStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.MaritalStatus.Insert(maritalStatus));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]MaritalStatus maritalStatus)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.MaritalStatus.Update(maritalStatus))
            {
                return Ok(new { Message = "Estado civil se ha actualizado" });
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
                var maritalStatus = _unitOfWork.MaritalStatus.GetById(id);
                if (maritalStatus == null)
                    return NotFound();
                if (_unitOfWork.MaritalStatus.Delete(maritalStatus))
                    return Ok(new { Message = "Estado se ha eliminado" });
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
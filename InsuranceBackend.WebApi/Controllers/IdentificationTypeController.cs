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
    [Route("api/identificationType")]
    [Authorize]
    public class IdentificationTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IdentificationTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.IdentificationType.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedIdentificationType/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedIdentificationType(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.IdentificationType.IdentificationTypePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]IdentificationType identificationType)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.IdentificationType.Insert(identificationType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]IdentificationType identificationType)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.IdentificationType.Update(identificationType))
            {
                return Ok(new { Message = "Tipo de identificacion se ha actualizado" });
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
                var identificationType = _unitOfWork.IdentificationType.GetById(id);
                if (identificationType == null)
                    return NotFound();
                if (_unitOfWork.IdentificationType.Delete(identificationType))
                    return Ok(new { Message = "Tipo de identificacion se ha eliminado" });
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

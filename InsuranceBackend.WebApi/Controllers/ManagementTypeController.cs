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
    [Route("api/managementType")]
    [Authorize]
    public class ManagementTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagementTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.ManagementType.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.ManagementType.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]ManagementType managementType)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.ManagementType.Insert(managementType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]ManagementType managementType)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.ManagementType.Update(managementType))
            {
                return Ok(new { Message = "Tipo de gestión se ha actualizado" });
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
                var managementType = _unitOfWork.ManagementType.GetById(id);
                if (managementType == null)
                    return NotFound();
                if (_unitOfWork.ManagementType.Delete(managementType))
                    return Ok(new { Message = "Tipo de gestión se ha eliminado" });
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
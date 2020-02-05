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
    [Route("api/managementReason")]
    [Authorize]
    public class ManagementReasonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagementReasonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.ManagementReason.GetList());
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
                return Ok(_unitOfWork.ManagementReason.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedManagementReason/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedManagementReason(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.ManagementReason.ManagementReasonPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]ManagementReason ManagementReason)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.ManagementReason.Insert(ManagementReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]ManagementReason ManagementReason)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.ManagementReason.Update(ManagementReason))
            {
                return Ok(new { Message = "Motivo de gestión se ha actualizado" });
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
                var managementReason = _unitOfWork.ManagementReason.GetById(id);
                if (managementReason == null)
                    return NotFound();
                if (_unitOfWork.ManagementReason.Delete(managementReason))
                    return Ok(new { Message = "Motivo de gestión se ha eliminado" });
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
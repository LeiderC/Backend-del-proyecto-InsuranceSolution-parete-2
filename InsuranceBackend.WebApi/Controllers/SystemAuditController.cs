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
    [Route("api/systemAudit")]
    [Authorize]
    public class SystemAuditController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SystemAuditController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.SystemAudit.GetList());
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
                return Ok(_unitOfWork.SystemAudit.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedSystemAudit/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedSystemAudit(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.SystemAudit.SystemAuditPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]SystemAudit SystemAudit)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.SystemAudit.Insert(SystemAudit));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]SystemAudit SystemAudit)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.SystemAudit.Update(SystemAudit))
                {
                    return Ok(new { Message = "La auditoría se ha actualizado" });
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
                var SystemAudit = _unitOfWork.SystemAudit.GetById(id);
                if (SystemAudit == null)
                    return NotFound();
                if (_unitOfWork.SystemAudit.Delete(SystemAudit))
                    return Ok(new { Message = "La auditoría se ha eliminado" });
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

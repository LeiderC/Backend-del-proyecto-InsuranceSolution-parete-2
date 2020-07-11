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
    [Route("api/businessUnit")]
    [Authorize]
    public class BusinessUnitController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BusinessUnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.BusinessUnit.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedBusinessUnit/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedBusinessUnit(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.BusinessUnit.BusinessUnitPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]BusinessUnit businessUnit)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.BusinessUnit.Insert(businessUnit));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] BusinessUnit businessUnit)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.BusinessUnit.Update(businessUnit))
                {
                    return Ok(new { Message = "Línea de negocio se ha actualizado" });
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
                var domain = _unitOfWork.BusinessUnit.GetById(id);
                if (domain == null)
                    return NotFound();
                if (_unitOfWork.BusinessUnit.Delete(domain))
                    return Ok(new { Message = "La línea de negocio se ha eliminado" });
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
 
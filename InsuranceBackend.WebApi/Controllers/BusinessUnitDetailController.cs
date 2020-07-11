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
    [Route("api/businessUnitDetail")]
    [Authorize]
    public class BusinessUnitDetailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BusinessUnitDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.BusinessUnitDetail.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedBusinessUnitDetail/{page:int}/{rows:int}/{idBusinessUnit:int}")]
        public IActionResult GetPaginatedBusinessUnitDetail(int page, int rows, int idBusinessUnit)
        {
            try
            {
                return Ok(_unitOfWork.BusinessUnitDetail.BusinessUnitDetailPagedList(page, rows, idBusinessUnit));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBusinessUnitDetailBySalesman/{idSalesman:int}")]
        public IActionResult GetBusinessUnitDetailBySalesman(int idSalesman)
        {
            try
            {
                return Ok(_unitOfWork.BusinessUnitDetail.BusinessUnitDetailListsBySalesman(idSalesman));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]BusinessUnitDetail BusinessUnitDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.BusinessUnitDetail.Insert(BusinessUnitDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] BusinessUnitDetail BusinessUnitDetail)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.BusinessUnitDetail.Update(BusinessUnitDetail))
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
                var domain = _unitOfWork.BusinessUnitDetail.GetById(id);
                if (domain == null)
                    return NotFound();
                if (_unitOfWork.BusinessUnitDetail.Delete(domain))
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
 
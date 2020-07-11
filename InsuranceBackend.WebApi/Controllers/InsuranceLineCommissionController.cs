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
    [Route("api/insuranceLineCommission")]
    [Authorize]
    public class InsuranceLineCommissionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceLineCommissionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLineCommission.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedInsuranceLineCommission/{idInsuranceLine:int}/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsuranceLineCommission(int idInsuranceLine, int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLineCommission.InsuranceLineCommissionPagedList(idInsuranceLine, page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]InsuranceLineCommission insuranceLineCommission)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.InsuranceLineCommission.Insert(insuranceLineCommission));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]InsuranceLineCommission insuranceLineCommission)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.InsuranceLineCommission.Update(insuranceLineCommission))
                {
                    return Ok(new { Message = "La comisión pactada se ha actualizado" });
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}/{id2}")]
        public IActionResult Delete(int id, int id2)
        {
            try
            {
                var insuranceLineCommission = _unitOfWork.InsuranceLineCommission.InsuranceLineCommissionSingle(id, id2);
                if (insuranceLineCommission == null)
                    return NotFound();
                if (_unitOfWork.InsuranceLineCommission.Delete(insuranceLineCommission))
                    return Ok(new { Message = "La comisión pactada se ha eliminado" });
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
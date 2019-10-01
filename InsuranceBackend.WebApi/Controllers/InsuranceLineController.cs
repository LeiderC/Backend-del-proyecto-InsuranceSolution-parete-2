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
    [Route("api/insuranceLine")]
    [Authorize]
    public class InsuranceLineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceLineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLine.GetList());
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
                return Ok(_unitOfWork.InsuranceLine.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInsuranceLineCommissionByInsurance/{idInsurance:int}")]
        public IActionResult GetInsuranceLineCommissionByInsurance(int idInsurance)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLine.InsuranceLineCommissionByInsurance(idInsurance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInsuranceLineByInsurance/{idInsurance:int}")]
        public IActionResult GetInsuranceLineByInsurance(int idInsurance)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLine.InsuranceLineByInsurance(idInsurance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedInsuranceLine/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsuranceLine(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLine.InsuranceLinePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]InsuranceLine insuranceLine)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.InsuranceLine.Insert(insuranceLine));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]InsuranceLine insuranceLine)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.InsuranceLine.Update(insuranceLine))
                {
                    return Ok(new { Message = "El ramo se ha actualizado" });
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
                var line = _unitOfWork.InsuranceLine.GetById(id);
                if (line == null)
                    return NotFound();
                if (_unitOfWork.InsuranceLine.Delete(line))
                    return Ok(new { Message = "El ramo se ha eliminado" });
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
 
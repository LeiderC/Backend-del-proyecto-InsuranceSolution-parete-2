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
    [Route("api/insuranceSubline")]
    [Authorize]
    public class InsuranceSublineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceSublineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceSubline.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInsuranceSublineList/{idInsurance:int}/{idInsuranceLine:int}")]
        public IActionResult GetInsuranceSublineList(int idInsurance, int idInsuranceLine)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceSubline.InsuranceSublineList(idInsurance, idInsuranceLine));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedInsuranceSubline/{idInsuranceLine:int}/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsuranceSubline(int idInsuranceLine, int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceSubline.InsuranceSublinePagedList(idInsuranceLine, page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]InsuranceSubline insuranceSubline)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.InsuranceSubline.Insert(insuranceSubline));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]InsuranceSubline insuranceSubline)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.InsuranceSubline.Update(insuranceSubline))
                {
                    return Ok(new { Message = "El subramo se ha actualizado" });
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
                var insuranceSubline = _unitOfWork.InsuranceSubline.GetById(id);
                if (insuranceSubline == null)
                    return NotFound();
                if (_unitOfWork.InsuranceSubline.Delete(insuranceSubline))
                    return Ok(new { Message = "El subramo se ha eliminado" });
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
 
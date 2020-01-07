using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/beneficiary")]
    [Authorize]
    public class BeneficiaryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BeneficiaryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Beneficiary.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetBeneficiaryByIdenditification")]
        public IActionResult GetBeneficiaryByIdenditification([FromBody]GetBeneficiaryByIdentification request)
        {
            try
            {
                var result = _unitOfWork.Beneficiary.BeneficiaryByIdentification(request.Identification, request.IdentificationType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetBeneficiaryListByPolicy")]
        public IActionResult GetBeneficiaryListByPolicy([FromBody]GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Beneficiary.BeneficiaryListByPolicy(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Beneficiary beneficiary)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.Beneficiary.Insert(beneficiary));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Beneficiary beneficiary)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Beneficiary.Update(beneficiary))
                {
                    return Ok(new { Message = "Datos del beneficiario actualizados" });
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
                var beneficiary = _unitOfWork.Beneficiary.GetById(id);
                if (beneficiary == null)
                    return NotFound();
                if (_unitOfWork.Beneficiary.Delete(beneficiary))
                    return Ok(new { Message = "Los datos del beneficiario se han eliminado" });
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
 
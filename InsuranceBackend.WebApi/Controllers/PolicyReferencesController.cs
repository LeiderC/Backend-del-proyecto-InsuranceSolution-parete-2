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
    [Route("api/policyReferences")]
    [Authorize]
    public class PolicyReferencesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyReferencesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.PolicyReferences.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyReferencesListByPolicy")]
        public IActionResult GetPolicyReferencesListByPolicy([FromBody]GetSearchTerm request)
        {
            try
            {
                List<PolicyReferencesList> list = _unitOfWork.PolicyReferences.PolicyReferencesListByBolicy(int.Parse(request.SearchTerm)).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PolicyReferences PolicyReferences)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.PolicyReferences.Insert(PolicyReferences));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]PolicyReferences PolicyReferences)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PolicyReferences.Update(PolicyReferences))
                {
                    return Ok(new { Message = "Datos de la referencia actualizados" });
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
                var PolicyReferences = _unitOfWork.PolicyReferences.GetById(id);
                if (PolicyReferences == null)
                    return NotFound();
                if (_unitOfWork.PolicyReferences.Delete(PolicyReferences))
                    return Ok(new { Message = "Los datos de la referencia se han eliminado" });
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
 
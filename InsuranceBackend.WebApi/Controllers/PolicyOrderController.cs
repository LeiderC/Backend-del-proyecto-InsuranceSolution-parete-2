using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/policyOrder")]
    [Authorize]
    public class PolicyOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyOrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.PolicyOrder.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PolicyOrder policyOrder)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                policyOrder.IdUser = int.Parse(idUser);
                if (policyOrder.IdExternalUser > 0)
                    policyOrder.IdUser = 1;
                policyOrder.CreationDate = DateTime.Now;
                return Ok(_unitOfWork.PolicyOrder.Insert(policyOrder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody] PolicyOrder policyOrder)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PolicyOrder.Update(policyOrder))
                {
                    return Ok(new { Message = "Orden Póliza, se ha actualizado" });
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
                var policyOrder = _unitOfWork.PolicyOrder.GetById(id);
                if (policyOrder == null)
                    return NotFound();
                if (_unitOfWork.PolicyOrder.Delete(policyOrder))
                    return Ok(new { Message = "Orden Póliza se ha eliminado" });
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
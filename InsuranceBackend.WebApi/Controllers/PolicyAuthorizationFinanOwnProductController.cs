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
    [Route("api/policyAuthorizationFinanOwnProduct")]
    [Authorize]
    public class PolicyAuthorizationFinanOwnProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyAuthorizationFinanOwnProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.PolicyAuthorizationFinanOwnProduct.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PolicyAuthorizationFinanOwnProduct policyAuthorization)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                if (!policyAuthorization.CreationDate.HasValue)
                    policyAuthorization.CreationDate = DateTime.Now;
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                policyAuthorization.IdUser = int.Parse(idUser);
                return Ok(_unitOfWork.PolicyAuthorizationFinanOwnProduct.Insert(policyAuthorization));
            }
            catch(System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number.Equals(2601))
                {
                    return StatusCode(500, "No se puede ingresar la autorización mas de una vez");
                }
                return StatusCode(500, "Internal server error: " + sqlex.Message);
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
                var domain = _unitOfWork.PolicyAuthorizationFinanOwnProduct.GetById(id);
                if (domain == null)
                    return NotFound();
                if (_unitOfWork.PolicyAuthorizationFinanOwnProduct.Delete(domain))
                    return Ok(new { Message = "La autorización se ha eliminado" });
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
 
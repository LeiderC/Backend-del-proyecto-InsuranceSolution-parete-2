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
    [Route("api/policyExternalUser")]
    [Authorize]
    public class PolicyExternalUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyExternalUserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult Post([FromBody]PolicyExternalUser policyExternalUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.PolicyExternalUser.Insert(policyExternalUser));
            }
            catch(System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number.Equals(2601))
                {
                    return StatusCode(500, "No se puede ingresar lo mismo mas de una vez");
                }
                return StatusCode(500, "Internal server error: " + sqlex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
 
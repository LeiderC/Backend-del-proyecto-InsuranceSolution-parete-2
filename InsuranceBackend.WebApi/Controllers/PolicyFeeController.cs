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
    [Route("api/policyFee")]
    [Authorize]
    public class PolicyFeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyFeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetPolicyFeeListByPolicy")]
        public IActionResult GetPolicyFeeListByPolicy([FromBody]GetPolicyFee request)
        {
            try
            {
                return Ok(_unitOfWork.PolicyFee.PolicyFeeListByPolicy(request.IdPolicy, request.Paid));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
 
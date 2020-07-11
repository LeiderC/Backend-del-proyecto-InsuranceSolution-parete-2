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
    [Route("api/policyFeeFinancial")]
    [Authorize]
    public class PolicyFeeFinancialController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyFeeFinancialController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetPolicyFeeFinancialListByPolicy")]
        public IActionResult GetPolicyFeeFinancialListByPolicy([FromBody]GetPolicyFee request)
        {
            try
            {
                return Ok(_unitOfWork.PolicyFeeFinancial.PolicyFeeFinancialListByPolicy(request.IdPolicy, request.Paid));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
 
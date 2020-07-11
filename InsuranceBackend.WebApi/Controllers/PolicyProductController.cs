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
    [Route("api/policyProduct")]
    [Authorize]
    public class PolicyProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetPolicyProductListByPolicy")]
        public IActionResult GetPolicyProductListByPolicy([FromBody]GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.PolicyProduct.PolicyProductListByPolicy(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
 
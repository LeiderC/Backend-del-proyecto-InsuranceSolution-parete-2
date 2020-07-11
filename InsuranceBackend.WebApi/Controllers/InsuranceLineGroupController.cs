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
    [Route("api/insuranceLineGroup")]
    [Authorize]
    public class InsuranceLineGroupController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceLineGroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLineGroup.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInsuranceLineGroupList/{idInsuranceLine:int}")]
        public IActionResult GetInsuranceLineGroupList(int idInsuranceLine)
        {
            try
            {
                return Ok(_unitOfWork.InsuranceLineGroup.InsuranceLineGroupList(idInsuranceLine));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
 
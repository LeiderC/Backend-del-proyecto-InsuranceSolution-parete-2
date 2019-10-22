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
    [Route("api/managementPartner")]
    public class ManagementPartnerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagementPartnerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ManagementPartner>> Get()
        {
            return Ok(_unitOfWork.ManagementPartner.GetList());
        }
    }
}
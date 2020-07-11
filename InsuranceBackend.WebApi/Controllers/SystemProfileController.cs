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
    [Route("api/systemProfile")]
    public class SystemProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SystemProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SystemProfile>> Get()
        {
            return Ok(_unitOfWork.SystemProfile.GetList());
        }
    }
}
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
    [Route("api/recordStatus")]
    public class RecordStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public RecordStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RecordStatus>> Get()
        {
            return Ok(_unitOfWork.RecordStatus.GetList());
        }
    }
}
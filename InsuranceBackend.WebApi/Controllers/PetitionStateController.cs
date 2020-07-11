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
    [Route("api/petitionState")]
    public class PetitionStateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PetitionStateController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PetitionState>> Get()
        {
            return Ok(_unitOfWork.PetitionState.GetList());
        }
    }
}
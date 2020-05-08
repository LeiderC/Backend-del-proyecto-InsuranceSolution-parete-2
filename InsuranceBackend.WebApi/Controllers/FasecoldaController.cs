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
    [Route("api/fasecolda")]
    [Authorize]
    public class FasecoldaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FasecoldaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetFasecoldaByCode")]
        public IActionResult GetBeneficiaryByIdenditification([FromBody]Fasecolda fasecolda)
        {
            try
            {
                return Ok(_unitOfWork.Fasecolda.FasecoldaByCode(fasecolda.Code));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetFasecoldaDetailByCodeYear/{year:int}")]
        public IActionResult GetFasecoldaDetailByCodeYear([FromBody]Fasecolda fasecolda, int year)
        {
            try
            {
                return Ok(_unitOfWork.Fasecolda.FasecoldaDetailByCodeYear(fasecolda.Code, year));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
 
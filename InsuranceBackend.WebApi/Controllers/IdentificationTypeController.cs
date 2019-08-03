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
    [Route("api/identificationType")]
    [Authorize]
    public class IdentificationTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IdentificationTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.IdentificationType.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedIdentificationType/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedIdentificationType(int page, int rows)
        {
            return Ok(_unitOfWork.IdentificationType.IdentificationTypePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]IdentificationType identificationType)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.IdentificationType.Insert(identificationType));
        }

        [HttpPut]
        public IActionResult Put([FromBody]IdentificationType identificationType)
        {
            if (ModelState.IsValid && _unitOfWork.IdentificationType.Update(identificationType))
            {
                return Ok(new { Message = "El tipo de identificacion se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]IdentificationType identificationType)
        {
            if (identificationType.Id > 0)
                return Ok(_unitOfWork.IdentificationType.Delete(identificationType
                    ));
            else
                return BadRequest();
        }
    }
}

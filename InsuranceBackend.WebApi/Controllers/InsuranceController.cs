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
    [Route("api/insurance")]
    [Authorize]
    public class InsuranceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Insurance.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedInsurance/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsurance(int page, int rows)
        {
            return Ok(_unitOfWork.Insurance.InsurancePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Insurance insurance)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Insurance.Insert(insurance));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Insurance insurance)
        {
            if (ModelState.IsValid && _unitOfWork.Insurance.Update(insurance))
            {
                return Ok(new { Message = "El seguro se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Insurance insurance)
        {
            if (insurance.Id > 0)
                return Ok(_unitOfWork.Insurance.Delete(insurance
                    ));
            else
                return BadRequest();
        }
    }
}

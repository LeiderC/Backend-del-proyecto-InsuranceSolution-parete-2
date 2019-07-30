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
    [Route("api/insuranceLine")]
    [Authorize]
    public class InsuranceLineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceLineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.InsuranceLine.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedInsuranceLine/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsuranceLine(int page, int rows)
        {
            return Ok(_unitOfWork.InsuranceLine.InsuranceLinePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]InsuranceLine insuranceLine)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.InsuranceLine.Insert(insuranceLine));
        }

        [HttpPut]
        public IActionResult Put([FromBody]InsuranceLine insuranceLine)
        {
            if (ModelState.IsValid && _unitOfWork.InsuranceLine.Update(insuranceLine))
            {
                return Ok(new { Message = "El ramo se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]InsuranceLine insuranceLine)
        {
            if (insuranceLine.Id > 0)
                return Ok(_unitOfWork.InsuranceLine.Delete(insuranceLine
                    ));
            else
                return BadRequest();
        }
    }
}
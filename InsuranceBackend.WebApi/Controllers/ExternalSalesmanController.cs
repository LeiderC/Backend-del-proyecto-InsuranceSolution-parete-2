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
    [Route("api/externalSalesman")]
    [Authorize]
    public class ExternalSalesmanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExternalSalesmanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.ExternalSalesman.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedExternalSalesman/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedExternalSalesman(int page, int rows)
        {
            return Ok(_unitOfWork.ExternalSalesman.ExternalSalesmanPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]ExternalSalesman externalSalesman)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.ExternalSalesman.Insert(externalSalesman));
        }

        [HttpPut]
        public IActionResult Put([FromBody]ExternalSalesman externalSalesman)
        {
            if (ModelState.IsValid && _unitOfWork.ExternalSalesman.Update(externalSalesman))
            {
                return Ok(new { Message = "El vendedor externo se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]ExternalSalesman externalSalesman)
        {
            if (externalSalesman.Id > 0)
                return Ok(_unitOfWork.ExternalSalesman.Delete(externalSalesman
                    ));
            else
                return BadRequest();
        }
    }
}
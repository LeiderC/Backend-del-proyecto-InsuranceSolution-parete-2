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
    [Route("api/salesman")]
    [Authorize]
    public class SalesmanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SalesmanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.Salesman.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Salesman.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedSalesman/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedSalesman(int page, int rows)
        {
            return Ok(_unitOfWork.Salesman.SalesmanPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Salesman salesman)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Salesman.Insert(salesman));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Salesman salesman)
        {
            if (ModelState.IsValid && _unitOfWork.Salesman.Update(salesman))
            {
                return Ok(new { Message = "El vendedor se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Salesman salesman)
        {
            if (salesman.Id > 0)
                return Ok(_unitOfWork.Salesman.Delete(salesman
                    ));
            else
                return BadRequest();
        }
    }
}

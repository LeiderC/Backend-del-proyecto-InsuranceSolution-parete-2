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
    [Route("api/customerType")]
    [Authorize]
    public class CustomerTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.CustomerType.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedCustomerType/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedCustomerType(int page, int rows)
        {
            return Ok(_unitOfWork.CustomerType.CustomerTypePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]CustomerType customerType)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.CustomerType.Insert(customerType));
        }

        [HttpPut]
        public IActionResult Put([FromBody]CustomerType customerType)
        {
            if (ModelState.IsValid && _unitOfWork.CustomerType.Update(customerType))
            {
                return Ok(new { Message = "El tipo de cliente se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]CustomerType customerType)
        {
            if (customerType.Id > 0)
                return Ok(_unitOfWork.CustomerType.Delete(customerType
                    ));
            else
                return BadRequest();
        }
    }
}

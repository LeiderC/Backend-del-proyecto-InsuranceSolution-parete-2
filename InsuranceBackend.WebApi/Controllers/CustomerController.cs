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
    [Route("api/customer")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Customer.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetCustomerByIdentification")]
        public IActionResult GetCustomerByIdentification([FromBody]GetPaginatedSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Customer.CustomerByIdentificationNumber(request.SearchTerm));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaginatedCustomer")]
        public IActionResult GetPaginatedCustomer([FromBody]GetPaginatedSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Customer.CustomerPagedList(request.Page, request.Rows, request.SearchTerm));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.Customer.Insert(customer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Customer customer)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Customer.Update(customer))
                {
                    return Ok(new { Message = "El cliente se ha actualizado" });
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var customer = _unitOfWork.Customer.GetById(id);
                if (customer == null)
                    return NotFound();
                if(_unitOfWork.Customer.Delete(customer))
                    return Ok(new { Message = "Cliente eliminado" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

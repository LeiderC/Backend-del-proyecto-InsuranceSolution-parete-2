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
            try
            {
                return Ok(_unitOfWork.CustomerType.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedCustomerType/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedCustomerType(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.CustomerType.CustomerTypePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]CustomerType customerType)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.CustomerType.Insert(customerType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]CustomerType customerType)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.CustomerType.Update(customerType))
            {
                return Ok(new { Message = "Tipo de cliente se ha actualizado" });
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
                var customerType = _unitOfWork.CustomerType.GetById(id);
                if (customerType == null)
                    return NotFound();
                if (_unitOfWork.CustomerType.Delete(customerType))
                    return Ok(new { Message = "Tipo de cliente se ha eliminado" });
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

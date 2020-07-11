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
    [Route("api/customerBusinessUnit")]
    [Authorize]
    public class CustomerBusinessUnitController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerBusinessUnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.CustomerBusinessUnit.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedCustomerBusinessUnit/{page:int}/{rows:int}/{idCustomer:int}")]
        public IActionResult GetPaginatedCustomerBusinessUnit(int page, int rows, int idCustomer)
        {
            try
            {
                return Ok(_unitOfWork.CustomerBusinessUnit.CustomerBusinessUnitPagedList(page, rows, idCustomer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]CustomerBusinessUnit customerBusinessUnit)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.CustomerBusinessUnit.Insert(customerBusinessUnit));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] CustomerBusinessUnit customerBusinessUnit)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.CustomerBusinessUnit.Update(customerBusinessUnit))
                {
                    return Ok(new { Message = "Línea de negocio se ha actualizado" });
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
                var domain = _unitOfWork.CustomerBusinessUnit.GetById(id);
                if (domain == null)
                    return NotFound();
                if (_unitOfWork.CustomerBusinessUnit.Delete(domain))
                    return Ok(new { Message = "La línea de negocio se ha eliminado" });
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
 
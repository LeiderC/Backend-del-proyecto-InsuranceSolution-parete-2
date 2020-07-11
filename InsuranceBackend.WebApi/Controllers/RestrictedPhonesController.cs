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
    [Route("api/retrictedPhones")]
    [Authorize]
    public class RestrictedPhonesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public RestrictedPhonesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.RestrictedPhones.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]RestrictedPhones RestrictedPhones)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.RestrictedPhones.Insert(RestrictedPhones));
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number.Equals(2601))
                {
                    return StatusCode(500, "No se puede ingresar el mismo número de teléfono mas de una vez");
                }
                return StatusCode(500, "Internal server error: " + sqlex.Message);
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
                var phone = _unitOfWork.RestrictedPhones.GetById(id);
                if (phone == null)
                    return NotFound();
                if (_unitOfWork.RestrictedPhones.Delete(phone))
                    return Ok(new { Message = "El teléfono se ha eliminado" });
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
 
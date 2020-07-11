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
        public ActionResult<IEnumerable<Insurance>> Get()
        {
            try
            {
                return Ok(_unitOfWork.Insurance.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInsuranceByCommission")]
        public ActionResult<IEnumerable<Insurance>> GetInsuranceByCommission()
        {
            try
            {
                return Ok(_unitOfWork.Insurance.InsuranceByCommission());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetInsuranceBySubline")]
        public ActionResult<IEnumerable<Insurance>> GetInsuranceBySubline()
        {
            try
            {
                return Ok(_unitOfWork.Insurance.InsuranceBySubline());
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
            try
            {
                return Ok(_unitOfWork.Insurance.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedInsurance/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsurance(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Insurance.InsurancePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Insurance insurance)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.Insurance.Insert(insurance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Insurance insurance)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Insurance.Update(insurance))
                {
                    return Ok(new { Message = "La aseguradora se ha actualizado" });
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
                var insurance = _unitOfWork.Insurance.GetById(id);
                if(insurance==null)
                    return NotFound();
                if (_unitOfWork.Insurance.Delete(insurance))
                    return Ok(new { Message = "La aseguradora se ha eliminado" });
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

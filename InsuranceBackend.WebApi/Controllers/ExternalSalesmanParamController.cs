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
    [Route("api/externalSalesmanParam")]
    [Authorize]
    public class ExternalSalesmanParamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExternalSalesmanParamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.ExternalSalesmanParam.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedExternalSalesmanParam/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedExternalSalesmanParam(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.ExternalSalesmanParam.ExternalSalesmanParamPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]ExternalSalesmanParam ExternalSalesmanParam)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.ExternalSalesmanParam.Insert(ExternalSalesmanParam));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]ExternalSalesmanParam ExternalSalesmanParam)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.ExternalSalesmanParam.Update(ExternalSalesmanParam))
                {
                    return Ok(new { Message = "La parametrización del comercial se ha actualizado" });
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
                var ExternalSalesmanParam = _unitOfWork.ExternalSalesmanParam.GetById(id);
                if (ExternalSalesmanParam == null)
                    return NotFound();
                if (_unitOfWork.ExternalSalesmanParam.Delete(ExternalSalesmanParam))
                    return Ok(new { Message = "La parametrización del comercial se ha eliminado" });
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
 
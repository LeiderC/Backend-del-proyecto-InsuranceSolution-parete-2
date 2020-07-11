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
    [Route("api/salesmanParam")]
    [Authorize]
    public class SalesmanParamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SalesmanParamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.SalesmanParam.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedSalesmanParam/{page:int}/{rows:int}/{idSalesman:int}")]
        public IActionResult GetPaginatedSalesmanParam(int page, int rows, int idSalesman)
        {
            try
            {
                return Ok(_unitOfWork.SalesmanParam.SalesmanParamPagedList(page, rows, idSalesman));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]SalesmanParam salesmanParam)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.SalesmanParam.Insert(salesmanParam));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]SalesmanParam salesmanParam)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.SalesmanParam.Update(salesmanParam))
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
                var salesmanParam = _unitOfWork.SalesmanParam.GetById(id);
                if (salesmanParam == null)
                    return NotFound();
                if (_unitOfWork.SalesmanParam.Delete(salesmanParam))
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
 
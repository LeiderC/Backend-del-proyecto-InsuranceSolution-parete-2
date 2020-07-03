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
        public IActionResult Get()
        {
            try
            {
                List<ExternalSalesman> lst = _unitOfWork.ExternalSalesman.GetList().ToList();
                return Ok(lst);
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
                    return Ok(_unitOfWork.ExternalSalesman.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedExternalSalesman/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedExternalSalesman(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.ExternalSalesman.ExternalSalesmanPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]ExternalSalesman externalSalesman)
        {
            try
            {
                return Ok(_unitOfWork.ExternalSalesman.Insert(externalSalesman));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            if (!ModelState.IsValid)
                return BadRequest();
        }

        [HttpPut]
        public IActionResult Put([FromBody]ExternalSalesman externalSalesman)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.ExternalSalesman.Update(externalSalesman))
                {
                    return Ok(new { Message = "El vendedor externo se ha actualizado" });
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
                var salesman = _unitOfWork.ExternalSalesman.GetById(id);
                if (salesman == null)
                    return NotFound();
                if (_unitOfWork.ExternalSalesman.Delete(salesman))
                    return Ok(new { Message = "El comercial externo se ha eliminado" });
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
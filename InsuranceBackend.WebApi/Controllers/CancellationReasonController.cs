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
    [Route("api/cancellationReason")]
    [Authorize]
    public class CancellationReasonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CancellationReasonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.CancellationReason.GetList());
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
                return Ok(_unitOfWork.CancellationReason.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedCancellationReason/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedCancellationReason(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.CancellationReason.CancellationReasonPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]CancellationReason CancellationReason)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.CancellationReason.Insert(CancellationReason));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]CancellationReason CancellationReason)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.CancellationReason.Update(CancellationReason))
            {
                return Ok(new { Message = "Motivo de cancelación se ha actualizado" });
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
                var CancellationReason = _unitOfWork.CancellationReason.GetById(id);
                if (CancellationReason == null)
                    return NotFound();
                if (_unitOfWork.CancellationReason.Delete(CancellationReason))
                    return Ok(new { Message = "Motivo de cancelación se ha eliminado" });
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
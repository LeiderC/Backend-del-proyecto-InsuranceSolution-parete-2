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
    [Route("api/interestDue")]
    [Authorize]
    public class InterestDueController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InterestDueController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.InterestDue.GetList());
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
                return Ok(_unitOfWork.InterestDue.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedInterestDue/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInterestDue(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.InterestDue.InterestDuePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("ValidateInterestDue")]
        public IActionResult ValidateInterestDue([FromBody]GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.InterestDue.ValidateInterestDue(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]InterestDue InterestDue)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.InterestDue.Insert(InterestDue));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]InterestDue InterestDue)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.InterestDue.Update(InterestDue))
                {
                    return Ok(new { Message = "Tasa de interés se ha actualizado" });
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
                var InterestDue = _unitOfWork.InterestDue.GetById(id);
                if (InterestDue == null)
                    return NotFound();
                if (_unitOfWork.InterestDue.Delete(InterestDue))
                    return Ok(new { Message = "Tasa de interés se ha eliminado" });
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

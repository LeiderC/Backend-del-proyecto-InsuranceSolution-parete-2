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
    [Route("api/onerous")]
    public class OnerousController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OnerousController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Onerous>> Get()
        {
            return Ok(_unitOfWork.Onerous.GetList());
        }

        [HttpGet]
        [Route("GetPaginatedOnerous/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsurance(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Onerous.OnerousPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Onerous onerous)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.Onerous.Insert(onerous));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]Onerous onerous)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Onerous.Update(onerous))
                {
                    return Ok(new { Message = "El oneroso se ha actualizado" });
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
                var onerous = _unitOfWork.Onerous.GetById(id);
                if(onerous==null)
                    return NotFound();
                if (_unitOfWork.Onerous.Delete(onerous))
                    return Ok(new { Message = "El oneroso se ha eliminado" });
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
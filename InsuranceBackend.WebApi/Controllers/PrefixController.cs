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
    [Route("api/prefix")]
    [Authorize]
    public class PrefixController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PrefixController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Prefix.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedPrefix/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPrefix(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Prefix.PrefixPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]Prefix prefix)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.Prefix.Insert(prefix));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]Prefix prefix)
        {
            try
            {
                    if (ModelState.IsValid && _unitOfWork.Prefix.Update(prefix))
                {
                    return Ok(new { Message = "Prefijo se ha actualizado" });
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
                var prefix = _unitOfWork.Prefix.GetById(id);
                if (prefix == null)
                    return NotFound();
                if (_unitOfWork.Prefix.Delete(prefix))
                    return Ok(new { Message = "Prefijo se ha eliminado" });
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
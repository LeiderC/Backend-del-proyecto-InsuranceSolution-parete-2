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
    [Route("api/state")]
    [Authorize]
    public class StateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StateController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.State.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStateByCountry/{idCountry:int}")]
        public IActionResult GetStateByCountry(int idCountry)
        {
            try
            {
                return Ok(_unitOfWork.State.StateByCountry(idCountry));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedState/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedState(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.State.StatePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]State state)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.State.Insert(state));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]State state)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.State.Update(state))
            {
                return Ok(new { Message = "Estado se ha actualizado" });
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
                var state = _unitOfWork.State.GetById(id);
                if (state == null)
                    return NotFound();
                if (_unitOfWork.State.Delete(state))
                    return Ok(new { Message = "Estado se ha eliminado" });
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

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
            return Ok(_unitOfWork.State.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedState/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedState(int page, int rows)
        {
            return Ok(_unitOfWork.State.StatePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]State state)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.State.Insert(state));
        }

        [HttpPut]
        public IActionResult Put([FromBody]State state)
        {
            if (ModelState.IsValid && _unitOfWork.State.Update(state))
            {
                return Ok(new { Message = "El estado se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]State state)
        {
            if (state.Id > 0)
                return Ok(_unitOfWork.State.Delete(state
                    ));
            else
                return BadRequest();
        }
    }
}

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
    [Route("api/eps")]
    [Authorize]
    public class EPSController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EPSController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.EPS.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedEPS/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedEPS(int page, int rows)
        {
            return Ok(_unitOfWork.EPS.EPSPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]EPS eps)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.EPS.Insert(eps));
        }

        [HttpPut]
        public IActionResult Put([FromBody]EPS eps)
        {
            if (ModelState.IsValid && _unitOfWork.EPS.Update(eps))
            {
                return Ok(new { Message = "La eps se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]EPS eps)
        {
            if (eps.Id > 0)
                return Ok(_unitOfWork.EPS.Delete(eps
                    ));
            else
                return BadRequest();
        }
    }
}
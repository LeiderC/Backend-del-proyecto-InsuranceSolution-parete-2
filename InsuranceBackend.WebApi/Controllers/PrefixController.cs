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
            return Ok(_unitOfWork.Prefix.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedPrefix/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPrefix(int page, int rows)
        {
            return Ok(_unitOfWork.Prefix.PrefixPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Prefix prefix)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Prefix.Insert(prefix));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Prefix prefix)
        {
            if (ModelState.IsValid && _unitOfWork.Prefix.Update(prefix))
            {
                return Ok(new { Message = "El prefijo se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Prefix prefix)
        {
            if (prefix.Id > 0)
                return Ok(_unitOfWork.Prefix.Delete(prefix
                    ));
            else
                return BadRequest();
        }
    }
}
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
    [Route("api/digitalizedFile")]
    [Authorize]
    public class DigitalizedFileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DigitalizedFileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.DigitalizedFile.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedDigitalizedFile/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedDigitalizedFile(int page, int rows)
        {
            return Ok(_unitOfWork.DigitalizedFile.DigitalizedFilePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]DigitalizedFile digitalizedFile)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.DigitalizedFile.Insert(digitalizedFile));
        }

        [HttpPut]
        public IActionResult Put([FromBody]DigitalizedFile digitalizedFile)
        {
            if (ModelState.IsValid && _unitOfWork.DigitalizedFile.Update(digitalizedFile))
            {
                return Ok(new { Message = "El archivo digitalizado se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]DigitalizedFile digitalizedFile)
        {
            if (digitalizedFile.Id > 0)
                return Ok(_unitOfWork.DigitalizedFile.Delete(digitalizedFile
                    ));
            else
                return BadRequest();
        }
    }
}
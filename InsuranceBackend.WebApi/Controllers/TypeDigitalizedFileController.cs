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
    [Route("api/typeDigitalizedFile")]
    [Authorize]
    public class TypeDigitalizedFileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TypeDigitalizedFileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.TypeDigitalizedFile.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedTypeDigitalizedFile/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedTypeDigitalizedFile(int page, int rows)
        {
            return Ok(_unitOfWork.TypeDigitalizedFile.TypeDigitalizedFilePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]TypeDigitalizedFile typeDigitalizedFile)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.TypeDigitalizedFile.Insert(typeDigitalizedFile));
        }

        [HttpPut]
        public IActionResult Put([FromBody]TypeDigitalizedFile typeDigitalizedFile)
        {
            if (ModelState.IsValid && _unitOfWork.TypeDigitalizedFile.Update(typeDigitalizedFile))
            {
                return Ok(new { Message = "El tipo de archivo digitalizado se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]TypeDigitalizedFile typeDigitalizedFile)
        {
            if (typeDigitalizedFile.Id > 0)
                return Ok(_unitOfWork.TypeDigitalizedFile.Delete(typeDigitalizedFile
                    ));
            else
                return BadRequest();
        }
    }
}
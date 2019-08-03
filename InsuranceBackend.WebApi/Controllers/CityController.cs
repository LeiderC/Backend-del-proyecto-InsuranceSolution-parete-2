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
    [Route("api/city")]
    [Authorize]
    public class CityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.City.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedCity/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedCity(int page, int rows)
        {
            return Ok(_unitOfWork.City.CityPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]City city)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.City.Insert(city));
        }

        [HttpPut]
        public IActionResult Put([FromBody]City city)
        {
            if (ModelState.IsValid && _unitOfWork.City.Update(city))
            {
                return Ok(new { Message = "La ciudad se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]City city)
        {
            if (city.Id > 0)
                return Ok(_unitOfWork.City.Delete(city
                    ));
            else
                return BadRequest();
        }
    }
}

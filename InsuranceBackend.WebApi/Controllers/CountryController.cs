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
    [Route("api/country")]
    [Authorize]
    public class CountryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CountryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Country.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedCountry/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedCountry(int page, int rows)
        {
            return Ok(_unitOfWork.Country.CountryPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Country country)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Country.Insert(country));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Country country)
        {
            if (ModelState.IsValid && _unitOfWork.Country.Update(country))
            {
                return Ok(new { Message = "El pais se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Country country)
        {
            if (country.Id > 0)
                return Ok(_unitOfWork.Country.Delete(country
                    ));
            else
                return BadRequest();
        }
    }
}

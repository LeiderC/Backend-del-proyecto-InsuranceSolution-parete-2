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
            try
            {
                return Ok(_unitOfWork.City.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCityByState/{idState:int}")]
        public IActionResult GetCityByState(int idState)
        {
            try
            {
                return Ok(_unitOfWork.City.CityByState(idState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedCity/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedCity(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.City.CityPagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]City city)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.City.Insert(city));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]City city)
        {
            try
            {
                    if (ModelState.IsValid && _unitOfWork.City.Update(city))
                {
                    return Ok(new { Message = "Ciudad se ha actualizado" });
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
                var city = _unitOfWork.City.GetById(id);
                if (city == null)
                    return NotFound();
                if (_unitOfWork.City.Delete(city))
                    return Ok(new { Message = "Ciudad se ha eliminado" });
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

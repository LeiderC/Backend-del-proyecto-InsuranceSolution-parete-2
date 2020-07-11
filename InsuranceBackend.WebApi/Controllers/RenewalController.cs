using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/renewal")]
    [Authorize]
    public class RenewalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public RenewalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                List<Renewal> lst = _unitOfWork.Renewal.GetList().ToList().Where(r => r.IdUser.Equals(int.Parse(idUser))).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Renewal.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRenewalsByUser/{idUser:int}")]
        public IActionResult GetRenewalsByUser(int idUser)
        {
            try
            {
                List<Renewal> lst = _unitOfWork.Renewal.GetList().ToList().Where(r => r.IdUser.Equals(idUser)).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]Renewal Renewal)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.Renewal.Insert(Renewal));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]Renewal Renewal)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Renewal.Update(Renewal))
            {
                return Ok(new { Message = "Renovación se ha actualizado" });
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
                var Renewal = _unitOfWork.Renewal.GetById(id);
                if (Renewal == null)
                    return NotFound();
                if (_unitOfWork.Renewal.Delete(Renewal))
                    return Ok(new { Message = "Renovación se ha eliminado" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDashboardRenewal/{idRenewal:int}")]
        public IActionResult GetDashboardRenewal(int idRenewal)
        {
            try
            {
                return Ok(_unitOfWork.Renewal.DashboardRenewal(idRenewal));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
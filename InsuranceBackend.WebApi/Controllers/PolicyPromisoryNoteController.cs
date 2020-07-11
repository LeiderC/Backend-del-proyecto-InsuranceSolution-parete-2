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
    [Route("api/policyPromisoryNote")]
    [Authorize]
    public class PolicyPromisoryNoteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyPromisoryNoteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.PolicyPromisoryNote.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PolicyPromisoryNote PolicyPromisoryNote)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                PolicyPromisoryNote.IdUser = int.Parse(idUser);
                PolicyPromisoryNote.CreationDate = DateTime.Now;
                return Ok(_unitOfWork.PolicyPromisoryNote.Insert(PolicyPromisoryNote));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]PolicyPromisoryNote PolicyPromisoryNote)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PolicyPromisoryNote.Update(PolicyPromisoryNote))
                {
                    return Ok(new { Message = "Pagaré actualizado" });
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
                var PolicyPromisoryNote = _unitOfWork.PolicyPromisoryNote.GetById(id);
                if (PolicyPromisoryNote == null)
                    return NotFound();
                if (_unitOfWork.PolicyPromisoryNote.Delete(PolicyPromisoryNote))
                    return Ok(new { Message = "Pagaré eliminado" });
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/petition")]
    [Authorize]
    public class PetitionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PetitionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.Petition.GetList());
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
                return Ok(_unitOfWork.Petition.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaginatedPetition")]
        public IActionResult GetPaginatedPetition([FromBody]GetPaginatedPetition request)
        {
            try
            {
                return Ok(_unitOfWork.Petition.PetitionPagedList(request.Page, request.Rows, request.IdCustomer, request.State));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Petition Petition)
        {
            int idPetition = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {

                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    Petition.IdUser = int.Parse(idUser);
                    Settings settings = _unitOfWork.Settings.GetList().FirstOrDefault();
                    settings.PetitionsNumber += 1;
                    _unitOfWork.Settings.Update(settings);
                    Petition.Number = settings.PetitionsNumber;
                    idPetition = _unitOfWork.Petition.Insert(Petition);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idPetition);
        }

        [HttpPut]
        public IActionResult Put([FromBody]Petition Petition)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Petition.Update(Petition))
                {
                    return Ok(new { Message = "La petición se ha actualizado" });
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
                var Petition = _unitOfWork.Petition.GetById(id);
                if (Petition == null)
                    return NotFound();
                if (_unitOfWork.Petition.Delete(Petition))
                    return Ok(new { Message = "La petición se ha eliminado" });
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

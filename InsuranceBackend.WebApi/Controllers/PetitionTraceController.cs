using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/petitionTrace")]
    [Authorize]
    public class PetitionTraceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PetitionTraceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.PetitionTrace.GetList());
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
                return Ok(_unitOfWork.PetitionTrace.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedPetitionTrace/{page:int}/{rows:int}/{idPetition:int}")]
        public IActionResult GetPaginatedPetitionTrace(int page, int rows, int idPetition)
        {
            try
            {
                return Ok(_unitOfWork.PetitionTrace.PetitionTracePagedList(page, rows, idPetition));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PetitionTrace PetitionTrace)
        {
            int idPetitionTrace = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    PetitionTrace.IdUser = int.Parse(idUser);
                    idPetitionTrace = _unitOfWork.PetitionTrace.Insert(PetitionTrace);
                    Petition petition = _unitOfWork.Petition.GetById(PetitionTrace.IdPetition);
                    if (petition.State.Equals("A"))
                        petition.State = "P";
                    _unitOfWork.Petition.Update(petition);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idPetitionTrace);
        }

        [HttpPut]
        public IActionResult Put([FromBody]PetitionTrace PetitionTrace)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PetitionTrace.Update(PetitionTrace))
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
                var PetitionTrace = _unitOfWork.PetitionTrace.GetById(id);
                if (PetitionTrace == null)
                    return NotFound();
                if (_unitOfWork.PetitionTrace.Delete(PetitionTrace))
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

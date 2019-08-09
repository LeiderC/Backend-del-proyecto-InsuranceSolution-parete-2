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
    [Route("api/insuranceLine")]
    [Authorize]
    public class InsuranceLineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InsuranceLineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.InsuranceLine.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedInsuranceLine/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedInsuranceLine(int page, int rows)
        {
            return Ok(_unitOfWork.InsuranceLine.InsuranceLinePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]InsuranceLine insuranceLine)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.InsuranceLine.Insert(insuranceLine));
        }

        [HttpPut]
        public IActionResult Put([FromBody]InsuranceLine insuranceLine)
        {
            if (ModelState.IsValid && _unitOfWork.InsuranceLine.Update(insuranceLine))
            {
                return Ok(new { Message = "El ramo se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var line = _unitOfWork.InsuranceLine.GetById(id);
            if (line == null)
                return NotFound();
            return Ok(_unitOfWork.InsuranceLine.Delete(line));
        }
    }
}
/*try
	{
		var owner = _repository.Owner.GetOwnerById(id);
		if(owner.IsEmptyObject())
		{
			_logger.LogError($"Owner with id: {id}, hasn't been found in db.");
			return NotFound();
		}
 
		_repository.Owner.DeleteOwner(owner);
                _repository.Save();
 
		return NoContent();
	}
	catch (Exception ex)
	{
		_logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
		return StatusCode(500, "Internal server error");
	}
*/
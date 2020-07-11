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
    [Route("api/BranchOffice")]
    [Authorize]
    public class BranchOfficeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BranchOfficeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.BranchOffice.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedBranchOffice/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedBranchOffice(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.BranchOffice.BranchOfficePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]BranchOffice branchOffice)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();
                return Ok(_unitOfWork.BranchOffice.Insert(branchOffice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]BranchOffice branchOffice)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.BranchOffice.Update(branchOffice))
            {
                return Ok(new { Message = "Sucursal se ha actualizado" });
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
                var branchOffice = _unitOfWork.BranchOffice.GetById(id);
                if (branchOffice == null)
                    return NotFound();
                if (_unitOfWork.BranchOffice.Delete(branchOffice))
                    return Ok(new { Message = "Sucursal se ha eliminado" });
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

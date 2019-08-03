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
            return Ok(_unitOfWork.BranchOffice.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedBranchOffice/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedBranchOffice(int page, int rows)
        {
            return Ok(_unitOfWork.BranchOffice.BranchOfficePagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]BranchOffice branchOffice)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.BranchOffice.Insert(branchOffice));
        }

        [HttpPut]
        public IActionResult Put([FromBody]BranchOffice branchOffice)
        {
            if (ModelState.IsValid && _unitOfWork.BranchOffice.Update(branchOffice))
            {
                return Ok(new { Message = "La sucursal se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]BranchOffice branchOffice)
        {
            if (branchOffice.Id > 0)
                return Ok(_unitOfWork.BranchOffice.Delete(branchOffice
                    ));
            else
                return BadRequest();
        }
    }
}

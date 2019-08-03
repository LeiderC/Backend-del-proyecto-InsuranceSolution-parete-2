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
    [Route("api/policy")]
    [Authorize]
    public class PolicyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Policy.GetById(id));
        }

        [HttpGet]
        [Route("GetPaginatedPolicy/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPolicy(int page, int rows)
        {
            return Ok(_unitOfWork.Policy.PolicyPagedList(page, rows));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Policy policy)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_unitOfWork.Policy.Insert(policy));
        }

        [HttpPut]
        public IActionResult Put([FromBody]Policy policy)
        {
            if (ModelState.IsValid && _unitOfWork.Policy.Update(policy))
            {
                return Ok(new { Message = "El politica se ha actualizado" });
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Policy policy)
        {
            if (policy.Id > 0)
                return Ok(_unitOfWork.Policy.Delete(policy
                    ));
            else
                return BadRequest();
        }
    }
}

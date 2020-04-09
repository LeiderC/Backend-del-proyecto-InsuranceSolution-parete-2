using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/subMenuPerm")]
    [Authorize]
    public class SubMenuProfilePermController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubMenuProfilePermController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetSubMenuProfilePermListByUser")]
        public IActionResult GetSubMenuProfilePermListByUser([FromBody]GetSearchTerm request)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.SubMenuProfilePerm.SubMenuProfilePermListByUser(request.SearchTerm, int.Parse(idUser)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
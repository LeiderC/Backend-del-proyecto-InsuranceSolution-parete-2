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
    [Route("api/deleteDocumentPermission")]
    [Authorize]
    public class DeleteDocumentPermissionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteDocumentPermissionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.DeleteDocumentPermission.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDeletePermissionByModule/{module:int}")]
        public IActionResult GetDeletePermissionByModule(int module)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.DeleteDocumentPermission.DeleteDocumentPermissionByUserNModule(int.Parse(idUser), module));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]DeleteDocumentPermission DeleteDocumentPermission)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.DeleteDocumentPermission.Insert(DeleteDocumentPermission));
            }
            catch(System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number.Equals(2601))
                {
                    return StatusCode(500, "No se puede ingresar el mismo permiso mas de una vez");
                }
                return StatusCode(500, "Internal server error: " + sqlex.Message);
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
                var domain = _unitOfWork.DeleteDocumentPermission.GetById(id);
                if (domain == null)
                    return NotFound();
                if (_unitOfWork.DeleteDocumentPermission.Delete(domain))
                    return Ok(new { Message = "El permiso se ha eliminado" });
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
 
 
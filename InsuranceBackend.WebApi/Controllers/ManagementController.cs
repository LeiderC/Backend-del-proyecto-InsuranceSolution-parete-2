using System;
using System.Collections.Generic;
using System.Globalization;
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
    [Route("api/management")]
    [Authorize]
    public class ManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Management.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetManagementByUser/{idRenewal:int}")]
        public IActionResult GetManagementByUser(int idRenewal)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.Management.ManagementByUserList(int.Parse(idUser), idRenewal));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetManagementRenewalByUser")]
        public IActionResult GetManagementRenewalByUser()
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                List<Renewal> lst = _unitOfWork.Renewal.GetList().ToList().Where(r => r.IdUser.Equals(int.Parse(idUser))).ToList();
                Renewal renewal = lst.Where(r => r.RenewalDate.Year == DateTime.Now.Year && r.RenewalDate.Month == DateTime.Now.Month).FirstOrDefault();
                List<ManagementList> managements = new List<ManagementList>();
                if (renewal != null)
                {
                    managements = _unitOfWork.Management.ManagementByUserList(int.Parse(idUser), renewal.Id).ToList();
                }
                return Ok(managements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetManagementCreatedByUser")]
        public IActionResult GetManagementCreatedByUser([FromBody] GetPaginatedManagementByCustomer request)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                List<ManagementList> lst = _unitOfWork.Management.ManagementCreatedByUserList(int.Parse(idUser), request.State).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetManagementReportListByUser/{idUser:int}/{idRenewal:int}/{finished:int}")]
        public IActionResult GetManagementReportListByUser(int idUser, int idRenewal, int finished)
        {
            try
            {
                string currentIdUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                SystemUser user = _unitOfWork.User.GetById(int.Parse(currentIdUser));
                return Ok(_unitOfWork.Management.ManagementReportByUserList(idUser, idRenewal, finished != 0, user.CancelOrders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetManagementReportRenewalListByUser/{idUser:int}/{idRenewal:int}")]
        public IActionResult GetManagementReportRenewalListByUser(int idUser, int idRenewal)
        {
            try
            {
                var result = _unitOfWork.Management.ManagementReportRenewalByUserList(idUser, idRenewal);
                // foreach (var item in result)
                // {
                //     var data = (IDictionary<string, object>)item;
                // }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedManagement/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedManagement(int page, int rows)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.Management.ManagementPagedList(page, rows, int.Parse(idUser)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaginatedManagementByCustomer")]
        public IActionResult GetPaginatedManagementByCustomer([FromBody] GetPaginatedManagementByCustomer request)
        {
            try
            {
                return Ok(_unitOfWork.Management.ManagementByCustomerList(request.Page, request.Rows, request.IdCustomer, request.State));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaginatedManagementExtra/{page:int}/{rows:int}/{idManagementParent:int}")]
        public IActionResult GetPaginatedManagementExtra(int page, int rows, int idManagementParent)
        {
            try
            {
                List<ManagementExtraList> lst = _unitOfWork.Management.ManagementExtraPagedList(page, rows, idManagementParent).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDashboardManagementByUser")]
        public IActionResult GetDashboardManagementByUser()
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.Management.DashboardManagementByUser(int.Parse(idUser)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Management Management)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                int idManagement = _unitOfWork.Management.Insert(Management);
                if (idManagement > 0)
                {
                    if (Management.Hour != null)
                    {
                        DateTime localDate = Management.Hour.Value.ToLocalTime();
                        Management.Hour = localDate;
                    }
                    //Debemos validar si se está guardando una tarea o gestión que tiene una gestión padre
                    if (Management.IsExtra)
                    {
                        int idManagementExtra = _unitOfWork.ManagementExtra.Insert(new ManagementExtra { IdManagement = Management.IdManagementParent, IdManagementExtra = idManagement });
                        //Si la tarea de de cancelación se debe marcar la tarea principal como realizada
                        ManagementReason managementReason = _unitOfWork.ManagementReason.GetList().Where(m => m.Subgroup.Equals("C") && m.Id.Equals(Management.IdManagementReason)).FirstOrDefault();
                        if (managementReason != null)
                        {
                            Management parent = _unitOfWork.Management.GetById(Management.IdManagementParent);
                            if (parent != null)
                            {
                                parent.State = "R";
                                _unitOfWork.Management.Update(parent);
                            }
                        }
                        return Ok(idManagementExtra);

                    }
                    else
                        return Ok(idManagement);
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Management Management)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Management.Update(Management))
                {
                    return Ok(new { Message = "La Gestión/Tarea se ha actualizado" });
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
                var management = _unitOfWork.Management.GetById(id);
                if (management == null)
                    return NotFound();
                if (_unitOfWork.Management.Delete(management))
                    return Ok(new { Message = "La Gestión/Tarea se ha eliminado" });
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
﻿using System;
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
        public IActionResult GetPaginatedManagementByCustomer([FromBody]GetPaginatedManagementByCustomer request)
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
                return Ok(_unitOfWork.Management.ManagementExtraPagedList(page, rows,idManagementParent));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Management Management)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                int idManagement = _unitOfWork.Management.Insert(Management);
                if (idManagement > 0)
                {
                    //Debemos validar si se está guardando una tarea o gestión que tiene una gestión padre
                    if (Management.IsExtra)
                    {
                        return Ok(_unitOfWork.ManagementExtra.Insert(new ManagementExtra { IdManagement = Management.IdManagementParent, IdManagementExtra = idManagement }));
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
        public IActionResult Put([FromBody]Management Management)
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
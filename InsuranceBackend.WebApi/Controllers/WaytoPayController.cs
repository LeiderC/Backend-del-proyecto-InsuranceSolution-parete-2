﻿using System;
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
    [Route("api/waytoPay")]
    [Authorize]
    public class WaytoPayController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public WaytoPayController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.WaytoPay.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetWayToPayByPayType")]
        public IActionResult GetWayToPayByPayType([FromBody] PaymentType paymentType)
        {
            try
            {
                return Ok(_unitOfWork.WaytoPay.GetWaytoPaysByPaymentType(paymentType.Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
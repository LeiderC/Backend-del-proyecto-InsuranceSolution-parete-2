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
    [Route("api/paymentType")]
    [Authorize]
    public class PaymentTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.PaymentType.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetListProvisional")]
        public IActionResult GetListProvisional()
        {
            try
            {
                return Ok(_unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals("PV")));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedPaymentType/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPaymentType(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.PaymentType.PaymentTypePagedList(page, rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaymentTypeByPayMet")]
        public IActionResult GetPaymentTypeByPayMet([FromBody] PaymentMethod paymentMethod)
        {
            try
            {
                return Ok(_unitOfWork.PaymentType.PaymentTypeByPaymentMethod(paymentMethod.Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody]PaymentType PaymentType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.PaymentType.Insert(PaymentType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody]PaymentType PaymentType)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PaymentType.Update(PaymentType))
                {
                    return Ok(new { Message = "El tipo de pago se ha actualizado" });
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
        public IActionResult Delete(string id)
        {
            try
            {
                PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals(id)).FirstOrDefault();
                if (paymentType == null)
                    return NotFound();
                if (_unitOfWork.PaymentType.Delete(paymentType))
                    return Ok(new { Message = "Tipo de pago se ha eliminado" });
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
 
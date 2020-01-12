using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/payment")]
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Payment.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaymentPagedBySearchTerms")]
        public IActionResult GetPolicyCustomerBySearchTerms([FromBody]GetPaginatedPaymentSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Payment.PaymentPagedListSearchTerms(request.PaymentType, request.PaymentNumber, request.IdCustomer, request.IdPolicy, request.Page, request.Rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaymentDetailListByPayment")]
        public IActionResult GetPaymentDetailListByPayment([FromBody]GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Payment.PaymentDetailListByPayment(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PaymentSave Payment)
        {
            int idPayment = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    Payment.Payment.IdUser = int.Parse(idUser);
                    Payment.Payment.DatePayment = DateTime.Now;
                    idPayment = _unitOfWork.Payment.Insert(Payment.Payment);
                    if(Payment.PaymentDetails!=null && Payment.PaymentDetails.Count > 0)
                    {
                        foreach(var item in Payment.PaymentDetails)
                        {
                            item.IdPayment = idPayment;
                            _unitOfWork.PaymentDetail.Insert(item);
                        }
                    }
                    //Actualizamos el consecutivo
                    PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals(Payment.Payment.IdPaymentType)).FirstOrDefault();
                    paymentType.Number = Payment.Payment.Number;
                    _unitOfWork.PaymentType.Update(paymentType);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idPayment);
        }


        [HttpPut]
        public IActionResult Put([FromBody]PaymentSave Payment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _unitOfWork.Payment.Update(Payment.Payment);
                    //Primero debemos eliminar los detalles del pago
                    _unitOfWork.PaymentDetail.DeletePaymentDetailByPayment(Payment.Payment.Id);
                    if (Payment.PaymentDetails != null && Payment.PaymentDetails.Count > 0)
                    {
                        foreach (var item in Payment.PaymentDetails)
                        {
                            item.IdPayment = Payment.Payment.Id;
                            _unitOfWork.PaymentDetail.Insert(item);
                        }
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(new { Message = "El Pago se ha actualizado" });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var Payment = _unitOfWork.Payment.GetById(id);
                if (Payment == null)
                    return NotFound();
                if (_unitOfWork.Payment.Delete(Payment))
                    return Ok(new { Message = "Pago se ha eliminado" });
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
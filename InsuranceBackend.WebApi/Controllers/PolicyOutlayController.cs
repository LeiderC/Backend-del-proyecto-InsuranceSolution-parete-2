using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/policyOutlay")]
    [Authorize]
    public class PolicyOutlayController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyOutlayController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.PolicyOutlay.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PolicyOutlaySave policyOutlaySave)
        {
            int idPolicyOutlay = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    policyOutlaySave.PolicyOutlay.IdUser = int.Parse(idUser);
                    policyOutlaySave.PolicyOutlay.CreationDate = DateTime.Now;
                    idPolicyOutlay = _unitOfWork.PolicyOutlay.Insert(policyOutlaySave.PolicyOutlay);

                    // Actualizamos las cuotas financiadas con el día de pago real
                    IEnumerable<PolicyFeeFinancialList> policyFees = _unitOfWork.PolicyFeeFinancial.PolicyFeeFinancialListByPolicy(policyOutlaySave.PolicyOutlay.IdPolicy, false);
                    foreach (PolicyFeeFinancialList item in policyFees)
                    {
                        if (item.DatePayment.HasValue)
                        {
                            int day = policyOutlaySave.PolicyOutlay.PayDay;
                            int lastDay = DateTime.DaysInMonth(item.DatePayment.Value.Year, item.DatePayment.Value.Month);
                            if (day >= lastDay)
                                day = lastDay;
                            DateTime datePayment = new DateTime(item.DatePayment.Value.Year, item.DatePayment.Value.Month, day);
                            item.DatePayment = datePayment;
                            _unitOfWork.PolicyFeeFinancial.Update(item);
                        }
                    }
                    Policy policy = _unitOfWork.Policy.GetById(policyOutlaySave.PolicyOutlay.IdPolicy);
                    Customer customer = _unitOfWork.Customer.InsuredListByPolicy(policyOutlaySave.PolicyOutlay.IdPolicy).FirstOrDefault();

                    //Debemos generar el recaudo L3 aplicando el desembolso 
                    PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals("L3")).FirstOrDefault();
                    paymentType.Number = paymentType.Number + 1;
                    _unitOfWork.PaymentType.Update(paymentType);
                    Payment payment = new Payment();
                    payment.DateCreated = DateTime.Now;
                    payment.DatePayment = policyOutlaySave.DatePayment;
                    payment.IdPaymentType = "L3";
                    payment.IdUser = int.Parse(idUser);
                    payment.IdCustomer = customer.Id;
                    payment.Number = paymentType.Number;
                    //payment.PaidDestination = "A";
                    payment.State = "A";
                    payment.Observation = "DESEMBOLSO";
                    payment.Total = (float)policy.TotalValue;
                    payment.TotalReceived = (float)policy.TotalValue;
                    payment.TotalValue = (float)policy.TotalValue;
                    int idPayment = _unitOfWork.Payment.Insert(payment);
                    PaymentDetail paymentDetail = new PaymentDetail();
                    paymentDetail.DatePayFinancial = DateTime.Now;
                    paymentDetail.DueInterestValue = 0;
                    paymentDetail.FeeNumber = 1;
                    paymentDetail.IdPayment = idPayment;
                    paymentDetail.IdPolicy = policy.Id;
                    paymentDetail.InitialFee = false;
                    paymentDetail.Value = (float)policy.TotalValue;
                    paymentDetail.ValueOwnProduct = 0;
                    _unitOfWork.PaymentDetail.Insert(paymentDetail);

                    //Debemos guardar el documento digital y asociarlo al recaudo
                    DigitalizedFile digitalizedFile = policyOutlaySave.DigitalizedFile;
                    if (digitalizedFile != null)
                    {
                        digitalizedFile.IdPayment = idPayment;
                        digitalizedFile.Date = DateTime.Now;
                        digitalizedFile.Description = "DESEMBOLSO CREDITO #: " + policyOutlaySave.PolicyOutlay.CreditNumber + ", RECAUDO: L3-" + payment.Number.ToString();
                        _unitOfWork.DigitalizedFile.Insert(digitalizedFile);
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idPolicyOutlay);
        }


        [HttpPut]
        public IActionResult Put([FromBody] PolicyOutlay PolicyOutlay)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PolicyOutlay.Update(PolicyOutlay))
                {
                    return Ok(new { Message = "Desembolso actualizado" });
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
                var PolicyOutlay = _unitOfWork.PolicyOutlay.GetById(id);
                if (PolicyOutlay == null)
                    return NotFound();
                if (_unitOfWork.PolicyOutlay.Delete(PolicyOutlay))
                    return Ok(new { Message = "Desembolso eliminado" });
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
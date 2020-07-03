using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        [HttpGet]
        [Route("GetPaymentListById/{id:int}")]
        public IActionResult GetPaymentListById(int id)
        {
            try
            {
                PaymentList paymentList = _unitOfWork.Payment.PaymentListById(id);
                if (paymentList != null)
                {
                    List<PaymentDetailList> paymentDetailLists = _unitOfWork.Payment.PaymentDetailListByPayment(id).ToList();
                    List<PaymentDetailFinancialList> paymentDetailFinancialLists = _unitOfWork.Payment.PaymentDetailFinancialListByPayment(id).ToList();
                    paymentList.PaymentDetailLists = paymentDetailLists;
                    paymentList.PaymentDetailFinancialLists = paymentDetailFinancialLists;
                }
                return Ok(paymentList);
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
        [Route("GetPaymentDetailListByPolicy")]
        public IActionResult GetPaymentDetailListByPolicy([FromBody]GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Payment.PaymentDetailListByPolicy(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaymentDetailFinancialListByPolicy")]
        public IActionResult GetPaymentDetailFinancialListByPolicy([FromBody] GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Payment.PaymentDetailFinancialListByPolicy(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("RevokePayment")]
        public IActionResult RevokePayment([FromBody] Payment payment)
        {
            int idPayment = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    Payment _payment = _unitOfWork.Payment.GetById(payment.Id);
                    if (_payment == null)
                        return BadRequest("No existe el recaudo ingresado");
                    idPayment = _payment.Id;
                    _payment.State = "R";
                    _payment.ObservationRevoke = payment.ObservationRevoke;
                    _unitOfWork.Payment.Update(_payment);
                    _unitOfWork.PaymentDetail.DeletePaymentDetailByPayment(idPayment);
                    _unitOfWork.PaymentDetailFinancial.DeletePaymentDetailFinancialByPayment(idPayment);
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
                    Payment.Payment.DateCreated = DateTime.Now;
                    idPayment = _unitOfWork.Payment.Insert(Payment.Payment);
                    StringBuilder policyList = new StringBuilder();
                    if (Payment.PaymentDetails != null && Payment.PaymentDetails.Count > 0)
                    {
                        foreach (var item in Payment.PaymentDetails)
                        {
                            if (item.PaidDestination.Equals("A"))
                            {
                                string text = "Póliza #{0} {1} {2} {3} {4}, Cuota: {5}, Valor {6}, Valor Int. Mora {7}";
                                if (policyList.Length > 0)
                                    policyList.Append(" | " + string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                                else
                                    policyList.Append(string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                                PaymentDetail paymentDetail = new PaymentDetail { FeeNumber = item.FeeNumber, IdPayment = idPayment, IdPolicy = item.IdPolicy, Value = item.Value, ValueOwnProduct = item.ValueOwnProduct, DueInterestValue = item.DueInterestValue };
                                _unitOfWork.PaymentDetail.Insert(paymentDetail);
                            }
                            else
                            {
                                string text = "Póliza #{0} {1} {2} {3} {4}, Cuota: {5}, Valor {6}, Valor Int. Mora {7}";
                                if (policyList.Length > 0)
                                    policyList.Append(" | " + string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                                else
                                    policyList.Append(string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                                PaymentDetailFinancial paymentDetail = new PaymentDetailFinancial { FeeNumber = item.FeeNumber, IdPayment = idPayment, IdPolicy = item.IdPolicy, Value = item.Value, DueInterestValue = item.DueInterestValue };
                                _unitOfWork.PaymentDetailFinancial.Insert(paymentDetail);
                            }
                        }
                        //if (Payment.Payment.PaidDestination.Equals("A"))
                        //{
                        //    foreach (var item in Payment.PaymentDetails)
                        //    {
                        //        string text = "Póliza #{0} {1} {2} {3} {4}, Cuota: {5}, Valor {6}, Valor Int. Mora {7}";
                        //        if (policyList.Length > 0)
                        //            policyList.Append(" | " + string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                        //        else
                        //            policyList.Append(string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                        //        PaymentDetail paymentDetail = new PaymentDetail { FeeNumber = item.FeeNumber, IdPayment = idPayment, IdPolicy = item.IdPolicy, Value = item.Value, ValueOwnProduct = item.ValueOwnProduct, DueInterestValue = item.DueInterestValue };
                        //        _unitOfWork.PaymentDetail.Insert(paymentDetail);
                        //    }
                        //}
                        //else
                        //{
                        //    foreach (var item in Payment.PaymentDetails)
                        //    {
                        //        string text = "Póliza #{0} {1} {2} {3} {4}, Cuota: {5}, Valor {6}, Valor Int. Mora {7}";
                        //        if (policyList.Length > 0)
                        //            policyList.Append(" | " + string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                        //        else
                        //            policyList.Append(string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value), String.Format("{0:0,0.0}", item.DueInterestValue)));
                        //        PaymentDetailFinancial paymentDetail = new PaymentDetailFinancial { FeeNumber = item.FeeNumber, IdPayment = idPayment, IdPolicy = item.IdPolicy, Value = item.Value, DueInterestValue = item.DueInterestValue };
                        //        _unitOfWork.PaymentDetailFinancial.Insert(paymentDetail);
                        //    }
                        //}
                    }
                    //Actualizamos el consecutivo
                    PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals(Payment.Payment.IdPaymentType)).FirstOrDefault();
                    paymentType.Number = Payment.Payment.Number;
                    _unitOfWork.PaymentType.Update(paymentType);
                    //Creamos gestión con el recaudo realizado
                    Customer customer = _unitOfWork.Customer.GetById(Payment.Payment.IdCustomer);
                    string customerName = customer.FirstName + (string.IsNullOrEmpty(customer.MiddleName) ? "" : " " + customer.MiddleName) + customer.LastName + (string.IsNullOrEmpty(customer.MiddleLastName) ? "" : " " + customer.MiddleLastName);
                    string textSubject = "Creación Pago {0} # {1}, Total: {2}, Cliente: {3}, Detalle Pago: {4}";
                    string subject = string.Format(textSubject, paymentType.Alias, Payment.Payment.Number, String.Format("{0:0,0.0}", Payment.Payment.TotalValue), customerName, policyList.ToString());
                    Management management = new Management
                    {
                        ManagementType = "G",
                        CreationUser = int.Parse(idUser),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        State = "R",
                        Subject = subject,
                        ManagementPartner = "R",
                        IdCustomer = Payment.Payment.IdCustomer,
                        IsExtra = false,
                        IdPayment = idPayment
                    };
                    _unitOfWork.Management.Insert(management);
                    //Actualizamos el id en los digitales
                    foreach (int id in Payment.Digitals)
                    {
                        DigitalizedFile digitalizedFile = _unitOfWork.DigitalizedFile.GetById(id);
                        digitalizedFile.IdPayment = idPayment;
                        _unitOfWork.DigitalizedFile.Update(digitalizedFile);
                    }
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
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    _unitOfWork.Payment.Update(Payment.Payment);
                    //Primero debemos eliminar los detalles del pago
                    _unitOfWork.PaymentDetail.DeletePaymentDetailByPayment(Payment.Payment.Id);
                    StringBuilder policyList = new StringBuilder();
                    if (Payment.PaymentDetails != null && Payment.PaymentDetails.Count > 0)
                    {
                        foreach (var item in Payment.PaymentDetails)
                        {
                            string text = "Póliza #{0} {1} {2} {3} {4}, Cuota: {5}, Valor {6}";
                            if (policyList.Length > 0)
                                policyList.Append(" | " + string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value)));
                            else
                                policyList.Append(string.Format(text, item.Number, item.MovementShort, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, item.FeeNumber, String.Format("{0:0,0.0}", item.Value)));
                            PaymentDetail paymentDetail = new PaymentDetail { FeeNumber = item.FeeNumber, IdPayment = Payment.Payment.Id, IdPolicy = item.IdPolicy, Value = item.Value };
                            _unitOfWork.PaymentDetail.Insert(paymentDetail);
                        }
                    }
                    //Creamos gestión con el recaudo realizado
                    PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals(Payment.Payment.IdPaymentType)).FirstOrDefault();
                    Customer customer = _unitOfWork.Customer.GetById(Payment.Payment.IdCustomer);
                    string customerName = customer.FirstName + (string.IsNullOrEmpty(customer.MiddleName) ? "" : " " + customer.MiddleName) + customer.LastName + (string.IsNullOrEmpty(customer.MiddleLastName) ? "" : " " + customer.MiddleLastName);
                    string textSubject = "Modificación Pago {0} # {1}, Total: {2}, Cliente: {3}, Detalle Pago: {4}";
                    string subject = string.Format(textSubject, paymentType.Alias, Payment.Payment.Number, String.Format("{0:0,0.0}", Payment.Payment.TotalValue), customerName, policyList.ToString());
                    Management management = new Management
                    {
                        ManagementType = "G",
                        CreationUser = int.Parse(idUser),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        State = "R",
                        Subject = subject,
                        ManagementPartner = "R",
                        IdCustomer = Payment.Payment.IdCustomer,
                        IsExtra = false,
                        IdPayment = Payment.Payment.Id
                    };
                    _unitOfWork.Management.Insert(management);
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

        [HttpPost]
        [Route("GetPaymentDetailReport")]
        public IActionResult GetPaymentDetailReport([FromBody] GetPolicyPaymentThirdParties request)
        {
            try
            {
                return Ok(_unitOfWork.Payment.PaymentDetailReport(request.StartDate, request.EndDate, request.IdSalesman));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
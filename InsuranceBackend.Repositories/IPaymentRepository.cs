using System;
using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        IEnumerable<PaymentList> PaymentPagedListSearchTerms(string paymentType, int paymentNumber, int idCustomer, int idPolicy, int page, int rows);
        IEnumerable<PaymentDetailList> PaymentDetailListByPayment(int idPayment);
        IEnumerable<PaymentDetailProductList> PaymentDetailProductListByPayment(int idPayment);
        IEnumerable<PaymentDetailFinancialList> PaymentDetailFinancialListByPayment(int idPayment);
        IEnumerable<PaymentDetailList> PaymentDetailListByPolicy(int idPolicy);
        IEnumerable<PaymentDetailProductList> PaymentDetailProductListByPolicy(int idPolicy);
        IEnumerable<PaymentDetailFinancialList> PaymentDetailFinancialListByPolicy(int idPolicy);
        PaymentList PaymentListById(int id);
        IEnumerable<PaymentList> PaymentDetailReport(DateTime? startDate, DateTime? endDate, int idSalesman);
        IEnumerable<dynamic> PaymentCashFlow(DateTime startDate, DateTime endDate);
        IEnumerable<PaymentReportList> PaymentItemized(DateTime startDate, DateTime endDate);
    }
}

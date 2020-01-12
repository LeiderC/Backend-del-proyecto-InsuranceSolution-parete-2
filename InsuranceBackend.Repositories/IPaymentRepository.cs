using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        IEnumerable<PaymentList> PaymentPagedListSearchTerms(string paymentType, int paymentNumber, int idCustomer, int idPolicy, int page, int rows);
        IEnumerable<PaymentDetailList> PaymentDetailListByPayment(int idPayment);
        IEnumerable<PaymentDetailList> PaymentDetailListByPolicy(int idPolicy);
    }
}

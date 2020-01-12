using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPaymentDetailRepository : IRepository<PaymentDetail>
    {
        bool DeletePaymentDetailByPayment(int idPayment);
    }
}

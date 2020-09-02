using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPaymentDetailProductRepository : IRepository<PaymentDetailProduct>
    {
        bool DeletePaymentDetailProductByPayment(int idPayment);
    }
}

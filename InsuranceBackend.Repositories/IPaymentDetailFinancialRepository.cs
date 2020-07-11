using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPaymentDetailFinancialRepository : IRepository<PaymentDetailFinancial>
    {
        bool DeletePaymentDetailFinancialByPayment(int idPayment);
    }
}

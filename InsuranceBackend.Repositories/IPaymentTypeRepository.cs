using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPaymentTypeRepository : IRepository<PaymentType>
    {
        IEnumerable<PaymentTypeList> PaymentTypePagedList(int page, int rows);
        IEnumerable<PaymentType> PaymentTypeByPaymentMethod(string paymentMethod);
    }
}

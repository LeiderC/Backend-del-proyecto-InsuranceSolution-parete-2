using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Beneficiary
    {
        public int Id { get; set; }
        public int IdIdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MiddleLastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? IdGender { get; set; }
        [Write(false)]
        public double Percentage { get; set; }
    }
}

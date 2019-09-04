using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int IdUser { get; set; }
        public int? IdTaskAbout { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdPolicy { get; set; }
        public int? IdInsurance { get; set; }
        public int? IdPriority { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AlertDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        [Write(false)]
        public int IdManagement { get; set; }
    }
}

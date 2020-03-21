using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PetitionTrace
    {
        public int Id { get; set; }
        public int IdPetition { get; set; }
        public int IdUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string Detail { get; set; }
    }
}
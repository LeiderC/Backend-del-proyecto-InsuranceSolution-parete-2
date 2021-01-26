using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int IdIdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public int IdCustomerType { get; set; }
        public int? IdExpeditionCountry { get; set; }
        public int? IdExpeditionState { get; set; }
        public int? IdExpeditionCity { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MiddleLastName { get; set; }
        public int? IdPrefix { get; set; }
        public int? IdBirthCountry { get; set; }
        public int? IdBirthState { get; set; }
        public int? IdBirthCity { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? IdGender { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int? IdEPS { get; set; }
        public int SocialClass { get; set; }
        public int? IdMaritalStatus { get; set; }
        public string Phone { get; set; }
        public string Movil { get; set; }
        public string Email { get; set; }
        public int? IdOccupation { get; set; }
        public string Hobbie { get; set; }
        public string ResidenceAddress { get; set; }
        public int? IdResidenceCountry { get; set; }
        public int? IdResidenceState { get; set; }
        public int? IdResidenceCity { get; set; }
        public int? IdSalesman { get; set; }
        public int ChildNumber { get; set; }
        public string WebSite { get; set; }
        public int EmployeeNumber { get; set; }
        public bool Leaflet { get; set; }
        public DateTime? ExpeditionDate { get; set; }
        public bool ShowAll { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? IdUser { get; set; }
    }
}

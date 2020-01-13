using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/customer")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.Customer.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetCustomerByIdentification")]
        public IActionResult GetCustomerByIdentification([FromBody]GetPaginatedSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Customer.CustomerByIdentificationNumber(request.SearchTerm));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetInsuredListByPolicy")]
        public IActionResult GetInsuredListByPolicy([FromBody]GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Customer.InsuredListByPolicy(int.Parse(request.SearchTerm)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaginatedCustomer")]
        public IActionResult GetPaginatedCustomer([FromBody]GetPaginatedSearchTerm request)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                return Ok(_unitOfWork.Customer.CustomerPagedList(request.Page, request.Rows, request.SearchTerm));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            int idCustomer = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    idCustomer = _unitOfWork.Customer.Insert(customer);
                    IdentificationType it = _unitOfWork.IdentificationType.GetById(customer.IdIdentificationType);
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    string fullName = customer.FirstName + (string.IsNullOrEmpty(customer.MiddleName) ? "" : " " + customer.MiddleName) + customer.LastName + (string.IsNullOrEmpty(customer.MiddleLastName) ? "" : " " + customer.MiddleLastName);
                    //Insertamos la gestión realizada
                    Management management = new Management
                    {
                        ManagementType = "G",
                        CreationUser = int.Parse(idUser),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        State = "R",
                        Subject = "SE CREA CLIENTE " + it.Alias + " # " + customer.IdentificationNumber + " " + fullName,
                        ManagementPartner = "C",
                        IdCustomer = idCustomer,
                        IsExtra = false,
                    };
                    _unitOfWork.Management.Insert(management);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idCustomer);
        }

        [HttpPut]
        public IActionResult Put([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _unitOfWork.Customer.Update(customer);
                    IdentificationType it = _unitOfWork.IdentificationType.GetById(customer.IdIdentificationType);
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    string fullName = customer.FirstName + (string.IsNullOrEmpty(customer.MiddleName) ? "" : " " + customer.MiddleName) + customer.LastName + (string.IsNullOrEmpty(customer.MiddleLastName) ? "" : " " + customer.MiddleLastName);
                    //Insertamos la gestión realizada
                    Management management = new Management
                    {
                        ManagementType = "G",
                        CreationUser = int.Parse(idUser),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        State = "R",
                        Subject = "SE MODIFICA CLIENTE " + it.Alias + " # " + customer.IdentificationNumber + " " + fullName,
                        ManagementPartner = "C",
                        IdCustomer = customer.Id,
                        IsExtra = false,
                    };
                    _unitOfWork.Management.Insert(management);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(new { Message = "El cliente se ha actualizado" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var customer = _unitOfWork.Customer.GetById(id);
                if (customer == null)
                    return NotFound();
                if(_unitOfWork.Customer.Delete(customer))
                    return Ok(new { Message = "Cliente eliminado" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

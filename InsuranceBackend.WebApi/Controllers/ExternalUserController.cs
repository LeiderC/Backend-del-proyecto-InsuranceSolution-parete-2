using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.DataAccess.Password;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/externalUser")]
    [Authorize]
    public class ExternalUserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExternalUserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<ExternalUserList> lst = _unitOfWork.ExternalUser.GetExternalUserLists().ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.ExternalUser.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] ExternalUser externalUser)
        {
            int idUser = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    HashSalt salt = new HashSalt();
                    salt = PasswordUtil.GenerateSaltedHash(32, externalUser.Password);
                    externalUser.Password = salt.Hash;
                    externalUser.Help = salt.Salt;
                    idUser = _unitOfWork.ExternalUser.Insert(externalUser);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idUser);
        }

        [HttpPut]
        public IActionResult Put([FromBody] ExternalUser externalUser)
        {
            try
            {
                HashSalt salt = new HashSalt();
                salt = PasswordUtil.GenerateSaltedHash(32, externalUser.Password);
                externalUser.Password = salt.Hash;
                externalUser.Help = salt.Salt;
                if (ModelState.IsValid && _unitOfWork.ExternalUser.Update(externalUser))
                {
                    return Ok(new { Message = "El usuario externo se ha actualizado" });
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var salesman = _unitOfWork.ExternalUser.GetById(id);
                if (salesman == null)
                    return NotFound();
                if (_unitOfWork.ExternalUser.Delete(salesman))
                    return Ok(new { Message = "El usuario externo se ha eliminado" });
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
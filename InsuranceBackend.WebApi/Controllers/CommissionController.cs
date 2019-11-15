using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/commision")]
    [Authorize]
    public class CommissionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommissionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /*[HttpPost]
        [Route("GetVehicleByLicense")]
        public IActionResult GetVehicleByLicense([FromBody]GetSearchTerm request)
        {
            try
            {
                var result = _unitOfWork.Vehicle.VehicleByLicense(request.SearchTerm.ToUpperInvariant());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }*/

        [HttpPost]
        [Route("GetSalesmanCommision")]
        public IActionResult GetSalesmanCommision([FromBody]GetSalesmanCommsisionSearchTerm searchTerm)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
/*[HttpPost]
        [Route("download")]
        public async Task<IActionResult> Download([FromBody]DigitalizedFile digitalizedFile)
        {
            var folderName = Path.Combine("Resources", "DigitalizedFiles");
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var filePath = Path.Combine(uploads, digitalizedFile.FileRoute);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), digitalizedFile.FileName);
        }
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/digitalizedFile")]
    //[Authorize]
    public class DigitalizedFileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DigitalizedFileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "DigitalizedFiles");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileNameOriginal = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string ext = Path.GetExtension(fileNameOriginal);
                    var guid = Guid.NewGuid();
                    var fileName = guid.ToString() + ext;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath, fileNameOriginal, guid, fileName });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error, Error: " + ex.Message);
            }
        }

        [HttpPost]
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

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.DigitalizedFile.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaginatedDigitalizedFile")]
        public IActionResult GetPaginatedDigitalizedFile([FromBody]GetPaginatedDigitalizedFile request)
        {
            try
            {
                return Ok(_unitOfWork.DigitalizedFile.DigitalizedFilePagedList(request.IdCustomer, request.Page, request.Rows, request.IdPolicyOrder, request.IdPolicy));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]DigitalizedFile digitalizedFile)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                return Ok(_unitOfWork.DigitalizedFile.Insert(digitalizedFile));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]DigitalizedFile digitalizedFile)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.DigitalizedFile.Update(digitalizedFile))
                {
                    return Ok(new { Message = "El archivo digitalizado se ha actualizado" });
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
            var df = _unitOfWork.DigitalizedFile.GetById(id);
            if (df == null)
                return NotFound();
            if(_unitOfWork.DigitalizedFile.Delete(df))
                return Ok(new { Message = "El documento se ha eliminado" });
            else
                return BadRequest();
        }

    }
}
 
 
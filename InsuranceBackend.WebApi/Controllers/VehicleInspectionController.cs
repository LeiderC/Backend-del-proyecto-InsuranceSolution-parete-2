using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/vehicleInspection")]
    [Authorize]
    public class VehicleInspectionController : Controller
    {
        Notification.EmailUtils emailUtils = new Notification.EmailUtils();
        private readonly NotificationMetadata _notificationMetadata;
        private readonly IUnitOfWork _unitOfWork;
        public VehicleInspectionController(IUnitOfWork unitOfWork, NotificationMetadata notificationMetadata)
        {
            _unitOfWork = unitOfWork;
            _notificationMetadata = notificationMetadata;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.VehicleInspection.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] VehicleInspectionSave vehicleInspectionSave)
        {
            int idVehicleInspection = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    idVehicleInspection = _unitOfWork.VehicleInspection.Insert(vehicleInspectionSave.VehicleInspection);
                    //Se envía correo
                    Insurance insurance = _unitOfWork.Insurance.GetById(vehicleInspectionSave.VehicleInspection.IdInsurance);
                    IEnumerable<VehicleType> vehicleTypes = _unitOfWork.VehicleType.GetList();
                    VehicleType vehicleType = vehicleTypes.Where(v => v.Id.Equals(vehicleInspectionSave.VehicleInspection.IdVehicleType)).FirstOrDefault();
                    StringBuilder content = new StringBuilder("<span>Buen día</span>");
                    content.Append("<br>");
                    content.Append("<span>Por favor me colabora para cargar la siguiente inspección</span>");
                    content.Append("<br>");
                    content.Append("<span>PLACA: " + vehicleInspectionSave.VehicleInspection.License.ToUpper() + "</span>");
                    content.Append("<br>");
                    content.Append("<span>CEDULA: " + vehicleInspectionSave.VehicleInspection.Identification + "</span>");
                    string fullName = vehicleInspectionSave.VehicleInspection.FirstName + (string.IsNullOrEmpty(vehicleInspectionSave.VehicleInspection.MiddleName) ? "" : " " + vehicleInspectionSave.VehicleInspection.MiddleName) + (string.IsNullOrEmpty(vehicleInspectionSave.VehicleInspection.LastName) ? "" : " " + vehicleInspectionSave.VehicleInspection.LastName) + (string.IsNullOrEmpty(vehicleInspectionSave.VehicleInspection.MiddleLastName) ? "" : " " + vehicleInspectionSave.VehicleInspection.MiddleLastName);
                    content.Append("<br>");
                    content.Append("<span>NOMBRE: " + fullName.ToUpper() + "</span>");
                    content.Append("<br>");
                    content.Append("<span>COMPAÑÍA: " + insurance.Description + "</span>");
                    content.Append("<br>");
                    content.Append("<span>OBSERVACIÓN: " + vehicleInspectionSave.VehicleInspection.Observation + "</span>");
                    content.Append("<br>");
                    content.Append("<span>CENTRO DE INSPECCIÓN: " + insurance.InspectCenter + "</span>");
                    content.Append("<br>");
                    if (vehicleType != null)
                    {
                        content.Append("<span>TIPO DE VEHÍCULO: " + vehicleType.Description + "</span>");
                        content.Append("<br>");
                    }
                    content.Append("<span>Inspección Virtual</span>");
                    content.Append("<br>");
                    content.Append("<span>Gracias</span>");
                    content.Append("<hr>");
                    content.Append("<h3>Para cualquier inquietud favor comunicarse al correo electrónico tecnico@wfe.com.co</h3>");
                    content.Append("<br>");
                    content.Append("<hr>");
                    content.Append("<footer style='text-align: center;'>");
                    content.Append("<strong>GRUPO WFE</strong>");
                    content.Append("<br>");
                    content.Append("<strong>Happy Gigas</strong>");
                    content.Append("<br>");
                    content.Append("</footer>");
                    content.Append("<footer style='text-align: justify;'>");
                    content.Append("<span >Nota: Este mensaje ha sido generado automaticamente. Por favor no lo responda</span>");
                    content.Append("</footer>");
                    string ccName = "Técnico";
                    string cc = "tecnico@wfe.com.co";
                    emailUtils.SendMail(_notificationMetadata, insurance.EmailVehicleInspect, "",
                    vehicleInspectionSave.VehicleInspection.License + " PARA CARGAR INSPECCIÓN",
                    MimeKit.Text.TextFormat.Html, content.ToString(), ccName, cc, vehicleInspectionSave.DigitalizedFiles);
                    //Documentos digitalizados
                    if (vehicleInspectionSave.DigitalizedFiles != null && vehicleInspectionSave.DigitalizedFiles.Count > 0)
                    {
                        foreach (var df in vehicleInspectionSave.DigitalizedFiles)
                        {
                            df.IdVehicleInspection = idVehicleInspection;
                            _unitOfWork.DigitalizedFile.Insert(df);
                        }
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idVehicleInspection);
        }

        [HttpPut]
        public IActionResult Put([FromBody] VehicleInspection vehicleInspection)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.VehicleInspection.Update(vehicleInspection))
                {
                    return Ok(new { Message = "Solicitud de inspección del vehículo actualizados" });
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
                var vehicle = _unitOfWork.VehicleInspection.GetById(id);
                if (vehicle == null)
                    return NotFound();
                if (_unitOfWork.VehicleInspection.Delete(vehicle))
                    return Ok(new { Message = "Solicitud de inspección del vehículo se ha eliminado" });
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

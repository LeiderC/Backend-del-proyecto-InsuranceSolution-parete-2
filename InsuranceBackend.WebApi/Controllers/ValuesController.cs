using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        Notification.EmailUtils emailUtils = new Notification.EmailUtils();
        private readonly IUnitOfWork _unitOfWork;
        NotificationMetadata _notificationMetadata;
        public ValuesController(IUnitOfWork unitOfWork, NotificationMetadata notificationMetadata)
        {
            _unitOfWork = unitOfWork;
            _notificationMetadata = notificationMetadata;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string content = "<h2><strong>Estimado: JUAN JUAN</strong></h2><h2><strong>Tomador/Empresa: COOPERATIVA DE TRANSPORTADORES DE CONCORDIA COONCOR </strong></h2><h3>Se acaba de registrar la siguiente orden:</h3><table style=\"height: 57px;\" width=\"529\"><tbody><tr><td style=\"width: 82.4667px;\"><strong>Placa</strong></td><td style=\"width: 82.5167px;\"><strong>Motor</strong></td><td style=\"width: 82.9333px;\"><strong>Chasis</strong></td><td style=\"width: 80.3667px;\"><strong>Modelo</strong></td><td style=\"width: 80.3667px;\"><strong>Capacidad</strong></td></tr><tr><td style=\"width: 82.4667px;\">XXX000</td><td style=\"width: 82.5167px;\">UJASK0</td><td style=\"width: 82.9333px;\">9819821</td><td style=\"width: 80.3667px;\">2011</td><td style=\"width: 80.3667px;\">5</td></tr></tbody></table><p>&nbsp;</p><table style=\"height: 57px;\" width=\"529\"><tbody><tr><td style=\"width: 82.4667px;\"><strong>Marca</strong></td><td style=\"width: 82.5167px;\"><strong>Clase</strong></td><td style=\"width: 82.9333px;\"><strong>Fasecolda</strong></td></tr><tr><td style=\"width: 82.4667px;\">CHEVROLET</td><td style=\"width: 82.5167px;\">BUSETA</td><td style=\"width: 82.9333px;\">00000</td></tr></tbody></table><h3>P&oacute;lizas:</h3><table><tbody><tr><td><strong>Aseguradora</strong></td><td><strong>Ramo</strong></td><td><strong>Subramo</strong></td><td><strong>Vigencia</strong></td></tr><tr><td>MUNDIAL SEGUROS</td><td>RC COLECTIVAS</td><td>RC CONTRACTUAL</td><td>08/05/2020-08/05/2021</td></tr><tr><td>MUNDIAL SEGUROS</td><td>RC COLECTIVAS</td><td>RC EXTRACONTRACTUAL</td><td>08/05/2020-08/05/2021</td></tr></tbody></table>";
            // emailUtils.SendMail(_notificationMetadata, "falvarezh87@gmail.com", "fabian", "prueba", MimeKit.Text.TextFormat.Html, content);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

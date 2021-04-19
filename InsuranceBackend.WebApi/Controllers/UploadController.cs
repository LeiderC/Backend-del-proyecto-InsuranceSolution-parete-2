using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using ExcelDataReader;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.Operations;
using CsvHelper.Configuration;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/upload")]
    //[Authorize]
    public class UploadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UploadController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadFasecolda")]
        public async Task<IActionResult> UploadFasecolda() //GuiaCodigos_CSV
        {
            List<Fasecolda> fasecoldaList = _unitOfWork.Fasecolda.GetList().ToList();
            int row = 0;
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                if (Request.Form != null)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {
                        BinaryReader b = new BinaryReader(file.OpenReadStream());
                        int count = (int)file.Length;
                        byte[] binData = b.ReadBytes(count);
                        using (MemoryStream stream = new MemoryStream(binData))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                                {
                                    while (csv.Read())
                                    {
                                        if (row > 0)
                                        {
                                            string novedad = csv.GetField(0);
                                            string marca = csv.GetField(1);
                                            string clase = csv.GetField(2);
                                            string codigo = csv.GetField(3);
                                            string referencia1 = csv.GetField(5);
                                            string referencia2 = csv.GetField(6);
                                            string referencia3 = csv.GetField(7);
                                            int.TryParse(csv.GetField(8), out int peso);
                                            string idServicio = csv.GetField(9);
                                            string servicio = csv.GetField(10);
                                            string tipoCaja = csv.GetField(14);
                                            int.TryParse(csv.GetField(15), out int cilindraje);
                                            int.TryParse(csv.GetField(17), out int nroPasajeros);
                                            int.TryParse(csv.GetField(18), out int capCarga);
                                            int.TryParse(csv.GetField(19), out int puertas);
                                            bool.TryParse(csv.GetField(20), out bool aire);
                                            int.TryParse(csv.GetField(21), out int ejes);
                                            string estado = csv.GetField(22);
                                            string combustible = csv.GetField(23);
                                            string transm = csv.GetField(24);
                                            Fasecolda fasecolda = fasecoldaList.Where(f => f.Code.Equals(codigo)).FirstOrDefault();
                                            if (fasecolda == null) //Insertamos el nuevo
                                            {
                                                fasecolda = new Fasecolda
                                                {
                                                    AirConditioning = aire,
                                                    Axes = ejes,
                                                    Brand = marca,
                                                    Class = clase,
                                                    Code = codigo,
                                                    Cylinder = cilindraje,
                                                    Doors = puertas,
                                                    Fuel = combustible,
                                                    GearboxType = tipoCaja,
                                                    IdVehicleService = idServicio,
                                                    LoadingCapacity = capCarga,
                                                    PassengersNumber = nroPasajeros,
                                                    Reference1 = referencia1,
                                                    Reference2 = referencia2,
                                                    Reference3 = referencia3,
                                                    State = estado.Equals("ACTIVO") ? "A" : "I",
                                                    Transmission = transm,
                                                    Weight = peso
                                                };
                                                _unitOfWork.Fasecolda.Insert(fasecolda);
                                            }
                                        }
                                        row += 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error, Error: " + ex.Message + " Fila: " + row);
            }
            return Ok();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadSoat")]
        public async Task<IActionResult> UploadSoat()
        {
            using (var transaction = new TransactionScope())
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                try
                {
                    if (Request.Form != null)
                    {
                        var file = Request.Form.Files[0];
                        string ase = Request.Form["idInsurance"];
                        string ram = Request.Form["idInsuranceLine"];
                        string subr = Request.Form["idInsuranceSubline"];
                        int idInsurance, idInsuranceLine, idInsuranceSubline = 0;
                        idInsurance = ase != null ? int.Parse(ase) : 0;
                        idInsuranceLine = ram != null ? int.Parse(ram) : 0;
                        idInsuranceSubline = subr != null ? int.Parse(subr) : 0;
                        if (file.Length > 0)
                        {
                            BinaryReader b = new BinaryReader(file.OpenReadStream());
                            int count = (int)file.Length;
                            byte[] binData = b.ReadBytes(count);
                            using (MemoryStream stream = new MemoryStream(binData))
                            //using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                            {
                                int row = 0;
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    do
                                    {
                                        while (reader.Read()) //Each ROW
                                        {
                                            if (reader.FieldCount > 0)
                                            {
                                                object firstCell = reader.GetValue(0);
                                                if (firstCell != null)
                                                {
                                                    if (row > 0)
                                                    {
                                                        int cantColumnas = reader.FieldCount;
                                                        if (cantColumnas != 47)
                                                        {
                                                            return BadRequest("La cantidad de columnas no concuerda con la definición de la plantilla");
                                                        }
                                                        string formulario = reader.GetValue(0) != null ? reader.GetValue(0).ToString() : "";
                                                        string fecEmision = reader.GetValue(2) != null ? reader.GetValue(2).ToString() : "";
                                                        DateTime.TryParse(fecEmision, out DateTime dFecEmision);
                                                        string vigDesde = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : "";
                                                        DateTime.TryParse(vigDesde, out DateTime dVigDesde);
                                                        string vigHasta = reader.GetValue(4) != null ? reader.GetValue(4).ToString() : "";
                                                        DateTime.TryParse(vigHasta, out DateTime dVigHasta);
                                                        string pago = reader.GetValue(5) != null ? reader.GetValue(5).ToString() : "";
                                                        double.TryParse(reader.GetValue(6) != null ? reader.GetValue(6).ToString() : "", out double prima);
                                                        double.TryParse(reader.GetValue(7) != null ? reader.GetValue(7).ToString() : "", out double runt);
                                                        double.TryParse(reader.GetValue(8) != null ? reader.GetValue(8).ToString() : "", out double contribucion);
                                                        double.TryParse(reader.GetValue(10) != null ? reader.GetValue(10).ToString() : "", out double total);
                                                        double.TryParse(reader.GetValue(11) != null ? reader.GetValue(11).ToString() : "", out double ingreso);
                                                        string cuenta = reader.GetValue(12) != null ? reader.GetValue(12).ToString() : "";
                                                        string placa = reader.GetValue(13) != null ? reader.GetValue(13).ToString() : "";
                                                        string marca = reader.GetValue(14) != null ? reader.GetValue(14).ToString() : "";
                                                        string linea = reader.GetValue(15) != null ? reader.GetValue(15).ToString() : "";
                                                        string asegurado = reader.GetValue(17) != null ? reader.GetValue(17).ToString() : "";
                                                        string documento = reader.GetValue(18) != null ? reader.GetValue(18).ToString() : "";
                                                        int.TryParse(reader.GetValue(33) != null ? reader.GetValue(33).ToString() : "", out int modelo);
                                                        int.TryParse(reader.GetValue(34) != null ? reader.GetValue(34).ToString() : "", out int cilindraje);
                                                        string codClase = reader.GetValue(35) != null ? reader.GetValue(35).ToString() : "";
                                                        string clase = reader.GetValue(36) != null ? reader.GetValue(36).ToString() : "";
                                                        string codServicio = reader.GetValue(38) != null ? reader.GetValue(38).ToString() : "";
                                                        string servicio = reader.GetValue(39) != null ? reader.GetValue(39).ToString() : "";
                                                        int.TryParse(reader.GetValue(40) != null ? reader.GetValue(40).ToString() : "", out int pasajeros);
                                                        string tipoPago = reader.GetValue(45) != null ? reader.GetValue(45).ToString() : "";
                                                        string tipoRecArchivo = reader.GetValue(46) != null ? reader.GetValue(46).ToString() : "";

                                                        //Primero validamos si existe el cliente
                                                        Customer customer = _unitOfWork.Customer.CustomerByIdentificationNumber(documento);
                                                        int idCustomer = 0;
                                                        if (customer == null) //Si no existe se debe crear
                                                        {
                                                            Customer customerNew = new Customer
                                                            {
                                                                FirstName = asegurado,
                                                                IdCustomerType = 1,
                                                                IdentificationNumber = documento,
                                                                IdIdentificationType = 1,
                                                                Leaflet = false,
                                                                ShowAll = true,
                                                            };
                                                            idCustomer = _unitOfWork.Customer.Insert(customerNew);
                                                        }
                                                        else
                                                        {
                                                            //Si existe y es prospecto se debe modificar
                                                            if (customer.Leaflet)
                                                            {
                                                                customer.Leaflet = false;
                                                                _unitOfWork.Customer.Update(customer);
                                                            }
                                                            idCustomer = customer.Id;
                                                        }
                                                        //Creamos el vehículo si no existe
                                                        int idVehicle = 0;
                                                        Vehicle vehicleExist = _unitOfWork.Vehicle.VehicleByLicense(placa);
                                                        if (vehicleExist != null)
                                                        {
                                                            idVehicle = vehicleExist.Id;
                                                        }
                                                        else
                                                        {
                                                            Vehicle vehicle = new Vehicle
                                                            {
                                                                Brand = marca,
                                                                Class = clase,
                                                                Cylinder = cilindraje,
                                                                License = placa,
                                                                Model = modelo,
                                                                PassengersNumber = pasajeros
                                                            };
                                                            idVehicle = _unitOfWork.Vehicle.Insert(vehicle);
                                                        }
                                                        PolicyOrder policyOrder = new PolicyOrder
                                                        {
                                                            CreationDate = DateTime.Now,
                                                            IdUser = int.Parse(idUser),
                                                            State = "A",
                                                            StateOrder = "A"
                                                        };
                                                        int idPolicyOrder = _unitOfWork.PolicyOrder.Insert(policyOrder);
                                                        //Creamos la póliza
                                                        Policy policyNew = new Policy
                                                        {
                                                            Contribution = contribucion,
                                                            DiscountValue = 0,
                                                            EndDate = dVigHasta,
                                                            ExpiditionDate = dFecEmision,
                                                            FeeNumbers = 1,
                                                            IdInsurance = idInsurance,
                                                            IdInsuranceLine = idInsuranceLine,
                                                            IdInsuranceSubline = idInsuranceSubline,
                                                            IdMovementType = "I",
                                                            IdPaymentMethod = "1",
                                                            IdPolicyHolder = idCustomer,
                                                            IdPolicyState = "1",
                                                            IdPolicyType = "3",
                                                            IdUser = int.Parse(idUser),
                                                            IdVehicle = idVehicle,
                                                            InitialFee = 0,
                                                            Inspected = "I",
                                                            IsAttached = false,
                                                            IsHeader = false,
                                                            IsOrder = false,
                                                            Iva = 0,
                                                            License = placa,
                                                            NetValue = prima,
                                                            Number = formulario,
                                                            PendingRegistration = "N",
                                                            PremiumExtra = 0,
                                                            PremiumValue = prima,
                                                            ReqAuthorization = false,
                                                            ReqAuthorizationDisc = false,
                                                            Runt = runt,
                                                            StartDate = dVigDesde,
                                                            TotalValue = total,
                                                            IdSalesMan = 64
                                                        };
                                                        int idPolicy = _unitOfWork.Policy.Insert(policyNew);
                                                        PolicyOrderDetail policyOrderDetail = new PolicyOrderDetail
                                                        {
                                                            CreationDate = DateTime.Now,
                                                            IdPolicy = idPolicy,
                                                            IdPolicyOrder = idPolicyOrder,
                                                            State = "A"
                                                        };
                                                        _unitOfWork.PolicyOrderDetail.Insert(policyOrderDetail);
                                                        //Asegurados
                                                        PolicyInsured policyInsured = new PolicyInsured
                                                        {
                                                            IdInsured = idCustomer,
                                                            IdPolicy = idPolicy
                                                        };
                                                        _unitOfWork.PolicyInsured.Insert(policyInsured);
                                                        //Beneficiarios
                                                        PolicyBeneficiary policyBeneficiary = new PolicyBeneficiary();
                                                        int idBeneficiary = 0;
                                                        Beneficiary ben = _unitOfWork.Beneficiary.BeneficiaryByIdentification(documento, 1);
                                                        if (ben != null)
                                                            idBeneficiary = ben.Id;
                                                        else
                                                        {
                                                            ben = new Beneficiary
                                                            {
                                                                FirstName = asegurado,
                                                                IdentificationNumber = documento,
                                                                IdIdentificationType = 1
                                                            };
                                                            idBeneficiary = _unitOfWork.Beneficiary.Insert(ben);
                                                        }
                                                        PolicyBeneficiary beneficiary = new PolicyBeneficiary
                                                        {
                                                            IdBeneficiary = idBeneficiary,
                                                            IdPolicy = idPolicy,
                                                            Percentage = 100
                                                        };
                                                        _unitOfWork.PolicyBeneficiary.Insert(beneficiary);
                                                        //Cuotas
                                                        PolicyFee policyFee = new PolicyFee
                                                        {
                                                            Date = dVigDesde,
                                                            DatePayment = dVigDesde,
                                                            IdPolicy = idPolicy,
                                                            Number = 1,
                                                            Value = total
                                                        };
                                                        _unitOfWork.PolicyFee.Insert(policyFee);
                                                        //Recaudos
                                                        // string tipoRec = "";
                                                        // if (cuenta.Equals("cuenta 7370"))
                                                        //     tipoRec = "BC";
                                                        // if (cuenta.Equals("cuenta 8165"))
                                                        //     tipoRec = "B7";
                                                        //PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals("D3")).FirstOrDefault();
                                                        PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals(tipoRecArchivo)).FirstOrDefault();
                                                        //Recaudo
                                                        paymentType.Number += 1;
                                                        _unitOfWork.PaymentType.Update(paymentType);
                                                        String idWayToPay = null;
                                                        List<WaytoPay> waytoPays = _unitOfWork.WaytoPay.GetWaytoPaysByPaymentType(tipoRecArchivo).ToList();
                                                        if (waytoPays.Count == 1)
                                                        {
                                                            idWayToPay = waytoPays[0].Id;
                                                        }
                                                        if (waytoPays.Count > 1)
                                                        {
                                                            idWayToPay = waytoPays.Where(tp => tp.Description.Trim().Equals(tipoPago)).FirstOrDefault().Id;
                                                        }
                                                        Payment payment = new Payment
                                                        {
                                                            IdCustomer = idCustomer,
                                                            DateCreated = DateTime.Now,
                                                            DatePayment = dFecEmision,
                                                            IdUser = int.Parse(idUser),
                                                            IdPaymentType = tipoRecArchivo,
                                                            Number = paymentType.Number,
                                                            //PaidDestination = "A",
                                                            State = "A",
                                                            Total = (float)total,
                                                            TotalReceived = (float)total,
                                                            TotalValue = (float)total,
                                                            IdWaytoPay = idWayToPay,
                                                            Observation = "RECAUDO CARGUE MASIVO"
                                                        };
                                                        int idPayment = _unitOfWork.Payment.Insert(payment);
                                                        PaymentDetail paymentDetail = new PaymentDetail
                                                        {
                                                            DueInterestValue = 0,
                                                            FeeNumber = 1,
                                                            IdPayment = idPayment,
                                                            IdPolicy = idPolicy,
                                                            Value = (float)total,
                                                        };
                                                        _unitOfWork.PaymentDetail.Insert(paymentDetail);
                                                    }
                                                    row += 1;
                                                }
                                            }
                                        }
                                    } while (reader.NextResult()); //Move to NEXT SHEET
                                }
                            }

                            transaction.Complete();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                        return BadRequest();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error, Error: " + ex.Message);
                }
            }
            return Ok();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadColectiva")]
        public async Task<IActionResult> UploadColectiva()
        {
            using (var transaction = new TransactionScope())
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                try
                {
                    if (Request.Form != null)
                    {
                        var file = Request.Form.Files[0];
                        string idPol = Request.Form["idPolicy"];
                        string idPaymentMethod = Request.Form["idPaymentMethod"];
                        string idExternalSalesman = Request.Form["idExternalSalesman"];
                        string invoiceNumber = Request.Form["invoiceNumber"];
                        string idMovementType = Request.Form["idMovementType"];
                        bool isIndividual = false;
                        if (!string.IsNullOrEmpty(Request.Form["individual"]) && Request.Form["individual"] == "1")
                            isIndividual = true;
                        int idPolicyHeader = idPol != null ? int.Parse(idPol) : 0;
                        int? extSalesman = null;
                        if (idExternalSalesman != "0")
                            extSalesman = int.Parse(idExternalSalesman);
                        //Poliza
                        Policy policyHeader = _unitOfWork.Policy.GetById(idPolicyHeader);
                        List<PolicyProductList> productLists = _unitOfWork.PolicyProduct.PolicyProductListByPolicy(policyHeader.Id).ToList();
                        double totalVrPremium = 0, totalIva = 0, totalNetValue = 0, totalValue = 0;
                        if (file.Length > 0)
                        {
                            BinaryReader b = new BinaryReader(file.OpenReadStream());
                            int count = (int)file.Length;
                            byte[] binData = b.ReadBytes(count);
                            using (MemoryStream stream = new MemoryStream(binData))
                            //using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                            {
                                int row = 0;
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    do
                                    {
                                        while (reader.Read()) //Each ROW
                                        {
                                            if (reader.FieldCount > 0)
                                            {
                                                if (row > 0)
                                                {
                                                    //Asegurado
                                                    string cedula = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : "";
                                                    if (!string.IsNullOrEmpty(cedula))
                                                    {
                                                        string tipoCliente = reader.GetValue(4) != null ? reader.GetValue(4).ToString() : "";
                                                        if (tipoCliente == "2")
                                                        {
                                                            if (cedula.Length > 9)
                                                                cedula = cedula.Substring(0, 9);
                                                        }
                                                        string nombre = reader.GetValue(5) != null ? reader.GetValue(5).ToString() : "";
                                                        string segundoNombre = reader.GetValue(6) != null ? reader.GetValue(6).ToString() : "";
                                                        string apellido = reader.GetValue(7) != null ? reader.GetValue(7).ToString() : "";
                                                        string segundoApellido = reader.GetValue(8) != null ? reader.GetValue(8).ToString() : "";
                                                        string sexo = reader.GetValue(9) != null ? reader.GetValue(9).ToString() : "";
                                                        int? sex = null;
                                                        int s = 0;
                                                        if (int.TryParse(sexo, out s))
                                                            sex = s;
                                                        if (sexo != "1" && sexo != "2")
                                                        {
                                                            sexo = null;
                                                            sex = null;
                                                        }
                                                        string direc = reader.GetValue(10) != null ? reader.GetValue(10).ToString() : "";
                                                        string telef = reader.GetValue(11) != null ? reader.GetValue(11).ToString() : "";
                                                        string email = reader.GetValue(12) != null ? reader.GetValue(12).ToString() : "";
                                                        string celular = reader.GetValue(13) != null ? reader.GetValue(13).ToString() : "";
                                                        string fecNacimiento = reader.GetValue(17) != null ? reader.GetValue(17).ToString() : "";
                                                        bool fecNac = DateTime.TryParse(fecNacimiento, out DateTime dFecNacimiento);
                                                        Customer customer = _unitOfWork.Customer.CustomerByIdentificationNumber(cedula);
                                                        int idCustomer = 0;
                                                        if (customer == null)
                                                        {
                                                            customer = new Customer
                                                            {
                                                                BirthDate = fecNac ? (DateTime?)dFecNacimiento : null,
                                                                Email = email,
                                                                FirstName = nombre,
                                                                IdCustomerType = int.Parse(tipoCliente),
                                                                IdentificationNumber = cedula,
                                                                IdGender = sex,
                                                                IdIdentificationType = 1,
                                                                LastName = apellido,
                                                                MiddleLastName = segundoApellido,
                                                                MiddleName = segundoNombre,
                                                                Leaflet = false,
                                                                Movil = celular,
                                                                Phone = telef,
                                                                ResidenceAddress = direc,
                                                                IdSalesman = policyHeader.IdSalesMan
                                                            };
                                                            idCustomer = _unitOfWork.Customer.Insert(customer);
                                                        }
                                                        else
                                                        {
                                                            //Si existe y es prospecto se debe modificar
                                                            if (customer.Leaflet)
                                                            {
                                                                customer.Leaflet = false;
                                                                _unitOfWork.Customer.Update(customer);
                                                            }
                                                            idCustomer = customer.Id;
                                                        }
                                                        //Vehiculo
                                                        string placa = reader.GetValue(45) != null ? reader.GetValue(45).ToString() : "";
                                                        string motor = reader.GetValue(46) != null ? reader.GetValue(46).ToString() : "";
                                                        string chasis = reader.GetValue(47) != null ? reader.GetValue(47).ToString() : "";
                                                        string serv = reader.GetValue(48) != null ? reader.GetValue(48).ToString() : "";
                                                        int.TryParse(reader.GetValue(49) != null ? reader.GetValue(49).ToString() : "", out int modelo);
                                                        int.TryParse(reader.GetValue(50) != null ? reader.GetValue(50).ToString() : "", out int vrccial);
                                                        string color = reader.GetValue(51) != null ? reader.GetValue(51).ToString() : "";
                                                        int.TryParse(reader.GetValue(52) != null ? reader.GetValue(52).ToString() : "", out int nroPasajeros);
                                                        int.TryParse(reader.GetValue(53) != null ? reader.GetValue(53).ToString() : "", out int cilindraje);
                                                        Vehicle vehicle = _unitOfWork.Vehicle.VehicleByLicense(placa);
                                                        int idVehicle = 0;
                                                        if (vehicle == null)
                                                        {
                                                            vehicle = new Vehicle
                                                            {
                                                                Chassis = chasis,
                                                                CommercialValue = vrccial,
                                                                Cylinder = cilindraje,
                                                                Engine = motor,
                                                                IdVehicleService = serv,
                                                                License = placa,
                                                                Model = modelo,
                                                                PassengersNumber = nroPasajeros
                                                            };
                                                            idVehicle = _unitOfWork.Vehicle.Insert(vehicle);
                                                        }
                                                        else
                                                            idVehicle = vehicle.Id;

                                                        int.TryParse(reader.GetValue(33) != null ? reader.GetValue(33).ToString() : "", out int cuotas);
                                                        double.TryParse(reader.GetValue(34) != null ? reader.GetValue(34).ToString() : "", out double prima);
                                                        totalVrPremium = totalVrPremium + prima;
                                                        double.TryParse(reader.GetValue(37) != null ? reader.GetValue(37).ToString() : "", out double iva);
                                                        totalIva = totalIva + iva;
                                                        double.TryParse(reader.GetValue(39) != null ? reader.GetValue(39).ToString() : "", out double total);
                                                        double.TryParse(reader.GetValue(40) != null ? reader.GetValue(40).ToString() : "", out double cuomescte);
                                                        double.TryParse(reader.GetValue(42) != null ? reader.GetValue(42).ToString() : "", out double otrosEx);
                                                        string certificado = reader.GetValue(69) != null ? reader.GetValue(69).ToString() : "";
                                                        Policy polCert = _unitOfWork.Policy.PolicyAttachedLastCertificate(certificado);
                                                        int lastCert = 1;
                                                        if (polCert != null)
                                                        {
                                                            lastCert = polCert.LastCertificate != null ? polCert.LastCertificate.Value + 1 : 1;
                                                            certificado = certificado + "-" + lastCert;
                                                        }
                                                        if (!certificado.Contains("-") && lastCert == 1)
                                                            certificado = certificado + "-" + 1;
                                                        string factura = "", observacion = "", movto = "";
                                                        factura = reader.FieldCount > 70 ? reader.GetValue(70) != null ? reader.GetValue(70).ToString() : "" : "";
                                                        observacion = reader.FieldCount > 71 ? reader.GetValue(71) != null ? reader.GetValue(71).ToString() : "" : "";
                                                        movto = reader.FieldCount > 72 ? reader.GetValue(72) != null ? reader.GetValue(72).ToString() : "" : "";
                                                        PolicyList policyExist = null;
                                                        if (isIndividual)
                                                        {
                                                            switch (movto)
                                                            {
                                                                case "I":
                                                                    idMovementType = "1";
                                                                    break;
                                                                case "R":
                                                                    idMovementType = "2";
                                                                    break;
                                                                case "E":
                                                                    idMovementType = "4";
                                                                    break;
                                                                case "M":
                                                                    idMovementType = "5";
                                                                    break;
                                                            }
                                                            if (idMovementType.Equals("4") || idMovementType.Equals("5"))
                                                            {
                                                                List<PolicyList> list = _unitOfWork.Policy.PolicyCustomerPagedListSearchTerms("C", placa, 0, 0, 0).ToList();
                                                                if (list.Count > 0)
                                                                {
                                                                    list = (from p in list
                                                                            orderby p.ExpiditionDate descending
                                                                            select p).ToList();
                                                                    policyExist = list.FirstOrDefault();
                                                                }
                                                            }
                                                        }
                                                        PolicyOrder policyOrder = new PolicyOrder
                                                        {
                                                            CreationDate = DateTime.Now,
                                                            IdUser = int.Parse(idUser),
                                                            State = "A",
                                                            StateOrder = "A"
                                                        };
                                                        int idPolicyOrder = _unitOfWork.PolicyOrder.Insert(policyOrder);
                                                        //Creamos la póliza
                                                        Policy policyNew = new Policy
                                                        {
                                                            Contribution = 0,
                                                            DiscountValue = 0,
                                                            EndDate = policyHeader.EndDate,
                                                            ExpiditionDate = policyHeader.ExpiditionDate,
                                                            FeeNumbers = cuotas,
                                                            //IdInsurance = policyHeader.IdInsurance,
                                                            //IdInsuranceLine = policyHeader.IdInsuranceLine,
                                                            //IdInsuranceSubline = policyHeader.IdInsuranceSubline,
                                                            IdMovementType = idMovementType,
                                                            IdPaymentMethod = idPaymentMethod,
                                                            IdPolicyState = "1",
                                                            IdPolicyType = policyHeader.IdPolicyType,
                                                            IdUser = int.Parse(idUser),
                                                            IdVehicle = idVehicle,
                                                            InitialFee = policyHeader.InitialFee,
                                                            OwnProducts = policyHeader.OwnProducts,
                                                            Inspected = "I",
                                                            IsAttached = true,
                                                            IsHeader = false,
                                                            IsOrder = false,
                                                            Iva = iva,
                                                            License = placa,
                                                            NetValue = prima,
                                                            PendingRegistration = "N",
                                                            PremiumExtra = 0,
                                                            PremiumValue = prima,
                                                            ReqAuthorization = false,
                                                            ReqAuthorizationDisc = false,
                                                            Runt = 0,
                                                            StartDate = policyHeader.StartDate,
                                                            TotalValue = total,
                                                            IdSalesMan = policyHeader.IdSalesMan,
                                                            Certificate = certificado,
                                                            IdPolicyHeader = policyHeader.Id,
                                                            IdExternalSalesMan = extSalesman,
                                                            LastCertificate = lastCert
                                                        };
                                                        int idPolicy = _unitOfWork.Policy.Insert(policyNew);
                                                        PolicyOrderDetail policyOrderDetail = new PolicyOrderDetail
                                                        {
                                                            CreationDate = DateTime.Now,
                                                            IdPolicy = idPolicy,
                                                            IdPolicyOrder = idPolicyOrder,
                                                            State = "A"
                                                        };
                                                        _unitOfWork.PolicyOrderDetail.Insert(policyOrderDetail);
                                                        //Asegurados
                                                        insured(idCustomer, idPolicy, false);
                                                        //Beneficiarios
                                                        beneficiaries(cedula, nombre, segundoNombre, segundoApellido, apellido, idPolicy, false);
                                                        //Cuotas
                                                        policyFees(policyHeader.StartDate.Value, total, idPolicy, false);
                                                        //Productos Propios
                                                        policyProducts(productLists, idPolicy, false);
                                                        //Debemos guardar la última transacción de certificado de póliza
                                                        PolicyAttachedLast policyAttachedLast = null;
                                                        if (isIndividual && policyExist != null)
                                                            policyAttachedLast = _unitOfWork.PolicyAttachedLast.PolicyAttachedLastByPolicyParent(policyExist.Id);
                                                        if (policyAttachedLast == null)
                                                        {
                                                            policyAttachedLast = new PolicyAttachedLast();
                                                            policyAttachedLast.EndDate = policyNew.EndDate;
                                                            policyAttachedLast.ExpiditionDate = policyNew.ExpiditionDate;
                                                            policyAttachedLast.IdExternalSalesMan = policyNew.IdExternalSalesMan;
                                                            policyAttachedLast.IdExternalUser = policyNew.IdExternalUser;
                                                            policyAttachedLast.IdPolicyHeader = policyNew.IdPolicyHeader;
                                                            policyAttachedLast.IdSalesMan = policyNew.IdSalesMan;
                                                            policyAttachedLast.OwnerIdentification = policyNew.OwnerIdentification;
                                                            policyAttachedLast.OwnerName = policyNew.OwnerName;
                                                            policyAttachedLast.TotalInitialFee = policyNew.TotalInitialFee;
                                                            policyAttachedLast.Id = 0;
                                                        }

                                                        policyAttachedLast.IdPolicyParent = idPolicy;
                                                        policyAttachedLast.Certificate = policyNew.Certificate;
                                                        policyAttachedLast.Contribution = policyNew.Contribution;

                                                        policyAttachedLast.FeeNumbers = policyNew.FeeNumbers;
                                                        policyAttachedLast.FeeValue = policyNew.FeeValue;

                                                        policyAttachedLast.IdFinancial = policyNew.IdFinancial;
                                                        policyAttachedLast.IdFinancialOption = policyNew.IdFinancialOption;
                                                        policyAttachedLast.IdMovementType = policyNew.IdMovementType;
                                                        policyAttachedLast.IdOnerous = policyNew.IdOnerous;
                                                        policyAttachedLast.IdPaymentMethod = policyNew.IdPaymentMethod;

                                                        policyAttachedLast.IdPolicyState = policyNew.IdPolicyState;
                                                        policyAttachedLast.IdUser = int.Parse(idUser);
                                                        policyAttachedLast.IdVehicle = policyNew.IdVehicle;
                                                        policyAttachedLast.InitialFee = policyNew.InitialFee;
                                                        policyAttachedLast.Inspected = policyNew.Inspected;
                                                        policyAttachedLast.InvoiceNumber = policyNew.InvoiceNumber;
                                                        policyAttachedLast.License = policyNew.License;
                                                        policyAttachedLast.Observation = policyNew.Observation;
                                                        policyAttachedLast.OwnProducts = policyNew.OwnProducts;
                                                        policyAttachedLast.Payday = policyNew.Payday;
                                                        policyAttachedLast.PendingRegistration = policyNew.PendingRegistration;
                                                        policyAttachedLast.ReqAuthorization = policyNew.ReqAuthorization;
                                                        policyAttachedLast.ReqAuthorizationFinancOwnProduct = policyNew.ReqAuthorizationFinancOwnProduct;
                                                        policyAttachedLast.StartDate = policyNew.StartDate;
                                                        policyAttachedLast.UpdateDate = DateTime.Now;

                                                        //Valores
                                                        switch (idMovementType)
                                                        {
                                                            case "1":
                                                            case "2":
                                                                policyAttachedLast.Iva = policyNew.Iva;
                                                                policyAttachedLast.NetValue = policyNew.NetValue;
                                                                policyAttachedLast.PremiumExtra = policyNew.PremiumExtra;
                                                                policyAttachedLast.PremiumValue = policyNew.PremiumValue;
                                                                policyAttachedLast.Runt = policyNew.Runt;
                                                                policyAttachedLast.TotalValue = policyNew.TotalValue;
                                                                break;
                                                            case "4":
                                                                policyAttachedLast.Iva = policyAttachedLast.Iva - policyNew.Iva;
                                                                policyAttachedLast.NetValue = policyAttachedLast.NetValue - policyNew.NetValue;
                                                                policyAttachedLast.PremiumExtra = policyAttachedLast.PremiumExtra - policyNew.PremiumExtra;
                                                                policyAttachedLast.PremiumValue = policyAttachedLast.PremiumValue - policyNew.PremiumValue;
                                                                policyAttachedLast.Runt = policyAttachedLast.Runt - policyNew.Runt;
                                                                policyAttachedLast.TotalValue = policyAttachedLast.TotalValue - policyNew.TotalValue;
                                                                break;
                                                            case "5":
                                                                policyAttachedLast.Iva = policyAttachedLast.Iva + policyNew.Iva;
                                                                policyAttachedLast.NetValue = policyAttachedLast.NetValue + policyNew.NetValue;
                                                                policyAttachedLast.PremiumExtra = policyAttachedLast.PremiumExtra + policyNew.PremiumExtra;
                                                                policyAttachedLast.PremiumValue = policyAttachedLast.PremiumValue + policyNew.PremiumValue;
                                                                policyAttachedLast.Runt = policyAttachedLast.Runt + policyNew.Runt;
                                                                policyAttachedLast.TotalValue = policyAttachedLast.TotalValue + policyNew.TotalValue;
                                                                break;
                                                        }

                                                        int idPolicyAttachedLast = policyAttachedLast.Id;
                                                        if (idPolicyAttachedLast > 0)
                                                            _unitOfWork.PolicyAttachedLast.Update(policyAttachedLast);
                                                        else
                                                            idPolicyAttachedLast = _unitOfWork.PolicyAttachedLast.Insert(policyAttachedLast);

                                                        policyNew.Id = idPolicy;
                                                        policyNew.IdPolicyAttachedLast = idPolicyAttachedLast;
                                                        _unitOfWork.Policy.Update(policyNew);

                                                        //Asegurados
                                                        insured(idCustomer, idPolicyAttachedLast, true);
                                                        //Beneficiarios
                                                        beneficiaries(cedula, nombre, segundoNombre, segundoApellido, apellido, idPolicyAttachedLast, true);
                                                        //Cuotas
                                                        policyFees(policyHeader.StartDate.Value, total, idPolicyAttachedLast, true);
                                                        //Productos Propios
                                                        policyProducts(productLists, idPolicyAttachedLast, true);
                                                    }
                                                }
                                                row += 1;
                                            }
                                        }
                                    } while (reader.NextResult()); //Move to NEXT SHEET
                                }
                            }
                            if (!isIndividual)
                            {
                                totalNetValue = totalVrPremium + totalIva;
                                totalValue = totalNetValue;
                                //Invoice Number
                                PolicyInvoice policyInvoice = new PolicyInvoice
                                {
                                    IdPolicy = policyHeader.Id,
                                    IdPaymentMethod = idPaymentMethod,
                                    InvoiceNumber = invoiceNumber,
                                    PremiumValue = totalVrPremium,
                                    Iva = totalIva,
                                    NetValue = totalNetValue,
                                    TotalValue = totalValue
                                };
                                _unitOfWork.PolicyInvoice.Insert(policyInvoice);
                            }
                            transaction.Complete();
                        }
                        else
                            return BadRequest();
                    }
                    else
                        return BadRequest();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error, Error: " + ex.Message);
                }
            }
            return Ok();
        }

        private void insured(int idCustomer, int idPolicy, bool isAttached)
        {
            if (isAttached)
            {
                _unitOfWork.PolicyAttachedLastInsured.DeletePolicyInsuredByPolicy(idPolicy);
                PolicyAttachedLastInsured policyInsuredLast = new PolicyAttachedLastInsured
                {
                    IdInsured = idCustomer,
                    IdPolicy = idPolicy
                };
                _unitOfWork.PolicyAttachedLastInsured.Insert(policyInsuredLast);
            }
            else
            {
                _unitOfWork.PolicyInsured.DeletePolicyInsuredByPolicy(idPolicy);
                PolicyInsured policyInsured = new PolicyInsured
                {
                    IdInsured = idCustomer,
                    IdPolicy = idPolicy
                };
                _unitOfWork.PolicyInsured.Insert(policyInsured);
            }
        }

        private void beneficiaries(string cedula, string nombre, string segundoNombre, string apellido, string segundoApellido, int idPolicy, bool isAttached)
        {
            if (isAttached)
            {
                _unitOfWork.PolicyAttachedLastBeneficiary.DeletePolicyBeneficiaryByPolicy(idPolicy);
                PolicyBeneficiary policyBeneficiary = new PolicyBeneficiary();
                int idBeneficiary = 0;
                Beneficiary ben = _unitOfWork.Beneficiary.BeneficiaryByIdentification(cedula, 1);
                if (ben != null)
                    idBeneficiary = ben.Id;
                else
                {
                    string nombreCompleto = nombre + (string.IsNullOrEmpty(segundoNombre) ? "" : " " + segundoNombre) + " " + apellido + (string.IsNullOrEmpty(segundoApellido) ? "" : " " + segundoApellido);
                    ben = new Beneficiary
                    {
                        FirstName = nombreCompleto,
                        IdentificationNumber = cedula,
                        IdIdentificationType = 1
                    };
                    idBeneficiary = _unitOfWork.Beneficiary.Insert(ben);
                }
                PolicyAttachedLastBeneficiary beneficiaryLast = new PolicyAttachedLastBeneficiary
                {
                    IdBeneficiary = idBeneficiary,
                    IdPolicy = idPolicy,
                    Percentage = 100
                };
                _unitOfWork.PolicyAttachedLastBeneficiary.Insert(beneficiaryLast);
            }
            else
            {
                _unitOfWork.PolicyBeneficiary.DeletePolicyBeneficiaryByPolicy(idPolicy);
                PolicyBeneficiary policyBeneficiary = new PolicyBeneficiary();
                int idBeneficiary = 0;
                Beneficiary ben = _unitOfWork.Beneficiary.BeneficiaryByIdentification(cedula, 1);
                if (ben != null)
                    idBeneficiary = ben.Id;
                else
                {
                    string nombreCompleto = nombre + (string.IsNullOrEmpty(segundoNombre) ? "" : " " + segundoNombre) + " " + apellido + (string.IsNullOrEmpty(segundoApellido) ? "" : " " + segundoApellido);
                    ben = new Beneficiary
                    {
                        FirstName = nombreCompleto,
                        IdentificationNumber = cedula,
                        IdIdentificationType = 1
                    };
                    idBeneficiary = _unitOfWork.Beneficiary.Insert(ben);
                }
                PolicyBeneficiary beneficiary = new PolicyBeneficiary
                {
                    IdBeneficiary = idBeneficiary,
                    IdPolicy = idPolicy,
                    Percentage = 100
                };
                _unitOfWork.PolicyBeneficiary.Insert(beneficiary);
            }
        }

        private void policyFees(DateTime startDate, double total, int idPolicy, bool isAttached)
        {
            if (isAttached)
            {
                _unitOfWork.PolicyAttachedLastFee.DeleteFeeByPolicy(idPolicy);
                PolicyAttachedLastFee policyFeeLast = new PolicyAttachedLastFee
                {
                    Date = startDate,
                    DatePayment = startDate,
                    IdPolicy = idPolicy,
                    Number = 1,
                    Value = total
                };
                _unitOfWork.PolicyAttachedLastFee.Insert(policyFeeLast);
            }
            else
            {
                _unitOfWork.PolicyFee.DeleteFeeByPolicy(idPolicy);
                PolicyFee policyFee = new PolicyFee
                {
                    Date = startDate,
                    DatePayment = startDate,
                    IdPolicy = idPolicy,
                    Number = 1,
                    Value = total
                };
                _unitOfWork.PolicyFee.Insert(policyFee);
            }
        }

        private void policyProducts(List<PolicyProductList> productLists, int idPolicy, bool isAttached)
        {
            if (isAttached)
            {
                _unitOfWork.PolicyAttachedLastProduct.DeletePolicyProductByPolicy(idPolicy);
                foreach (var pp in productLists)
                {
                    PolicyAttachedLastProduct policyProductLast = new PolicyAttachedLastProduct
                    {
                        IdPolicy = idPolicy,
                        Authorization = pp.Authorization,
                        ExtraValue = pp.ExtraValue,
                        FeeNumber = pp.FeeNumber,
                        FeeValue = pp.FeeValue,
                        IdProduct = pp.IdProduct,
                        IVA = pp.IVA,
                        TotalValue = pp.TotalValue,
                        Value = pp.Value
                    };
                    _unitOfWork.PolicyAttachedLastProduct.Insert(policyProductLast);
                }
            }
            else
            {
                _unitOfWork.PolicyProduct.DeletePolicyProductByPolicy(idPolicy);
                foreach (var pp in productLists)
                {
                    PolicyProduct policyProduct = new PolicyProduct
                    {
                        IdPolicy = idPolicy,
                        Authorization = pp.Authorization,
                        ExtraValue = pp.ExtraValue,
                        FeeNumber = pp.FeeNumber,
                        FeeValue = pp.FeeValue,
                        IdProduct = pp.IdProduct,
                        IVA = pp.IVA,
                        TotalValue = pp.TotalValue,
                        Value = pp.Value
                    };
                    _unitOfWork.PolicyProduct.Insert(policyProduct);
                }
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadProspect")]
        public async Task<IActionResult> UploadProspect()
        {
            using (var transaction = new TransactionScope())
            {
                string cedula = "";
                try
                {
                    if (Request.Form != null)
                    {
                        var file = Request.Form.Files[0];
                        if (file.Length > 0)
                        {
                            List<Salesman> salesmanList = _unitOfWork.Salesman.GetList().ToList();
                            List<IdentificationType> identificationTypes = _unitOfWork.IdentificationType.GetList().ToList();
                            BinaryReader b = new BinaryReader(file.OpenReadStream());
                            int count = (int)file.Length;
                            byte[] binData = b.ReadBytes(count);
                            using (MemoryStream stream = new MemoryStream(binData))
                            {
                                int row = 0;
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    do
                                    {
                                        while (reader.Read()) //Each ROW
                                        {
                                            if (reader.FieldCount > 0)
                                            {
                                                if (row > 0)
                                                {
                                                    cedula = reader.GetValue(0) != null ? reader.GetValue(0).ToString() : "";
                                                    if (!String.IsNullOrEmpty(cedula))
                                                    {
                                                        string tipoCliente = reader.GetValue(1) != null ? reader.GetValue(1).ToString() : "";
                                                        if (tipoCliente == "2")
                                                        {
                                                            if (cedula.Length > 9)
                                                                cedula = cedula.Substring(0, 9);
                                                        }
                                                        string tipoDoc = reader.GetValue(2) != null ? reader.GetValue(2).ToString() : "";
                                                        string nombres = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : "";
                                                        string apellidos = reader.GetValue(4) != null ? reader.GetValue(4).ToString() : "";
                                                        string email = reader.GetValue(5) != null ? reader.GetValue(5).ToString() : "";
                                                        string telefono = reader.GetValue(6) != null ? reader.GetValue(6).ToString() : "";
                                                        string celular = reader.GetValue(7) != null ? reader.GetValue(7).ToString() : "";
                                                        string direc = reader.GetValue(8) != null ? reader.GetValue(8).ToString() : "";
                                                        string fecNacimiento = reader.GetValue(9) != null ? reader.GetValue(9).ToString() : "";
                                                        bool fecNac = DateTime.TryParse(fecNacimiento, out DateTime dFecNacimiento);
                                                        string comercial = reader.GetValue(10) != null ? reader.GetValue(10).ToString() : "";
                                                        Salesman salesman = salesmanList.Where(s => s.Short.Equals(comercial)).FirstOrDefault();
                                                        if (salesman == null)
                                                        {
                                                            return BadRequest("No existe comercial " + comercial);
                                                        }
                                                        List<BusinessUnitDetailList> bud = _unitOfWork.BusinessUnitDetail.BusinessUnitDetailListsBySalesman(salesman.Id).ToList();
                                                        // Validamos si el cliente existe
                                                        Customer customer = _unitOfWork.Customer.CustomerByIdentificationNumber(cedula);
                                                        // Si el cliente existe validamos si el comercial ya esta asignado sino lo esta se debe asignar
                                                        string primerNombre = "", segundoNombre = "";
                                                        string primerApellido = "", segundoApellido = "";
                                                        string[] nombreArr = nombres.Split(' ');
                                                        if(nombreArr.Length==2) {
                                                            primerNombre = nombreArr[0];
                                                            segundoNombre = nombreArr[1];
                                                        } else {
                                                            primerNombre = nombres;
                                                        }
                                                        string[] apellidoArr = apellidos.Split(' ');
                                                        if(apellidoArr.Length==2) {
                                                            primerApellido = apellidoArr[0];
                                                            segundoApellido = apellidoArr[1];
                                                        } else {
                                                            primerApellido = apellidos;
                                                        }
                                                        if (customer != null)
                                                        {
                                                            int year = DateTime.Now.Year;
                                                            List<CustomerBusinessUnitList> lstcbus = _unitOfWork.CustomerBusinessUnit.CustomerBusinessUnitListByCustomer(customer.Id).ToList();
                                                            CustomerBusinessUnitList customerBusinessUnit = lstcbus.Where(c => c.IdSalesman == salesman.Id && c.Year == year.ToString()).FirstOrDefault();
                                                            if (customerBusinessUnit == null) // Si no esta asignado el comercial se debe asignar
                                                            {
                                                                CustomerBusinessUnit customerBusinessUnitNew = new CustomerBusinessUnit
                                                                {
                                                                    IdBusinessUnitDetail = bud[0].Id,
                                                                    IdCustomer = customer.Id,
                                                                    State = "A",
                                                                    Year = year.ToString()
                                                                };
                                                                _unitOfWork.CustomerBusinessUnit.Insert(customerBusinessUnitNew);
                                                            }
                                                            //Actualizamos los datos del cliente
                                                            customer.Email = email;
                                                            customer.FirstName = primerNombre;
                                                            customer.MiddleName = segundoNombre;
                                                            customer.IdSalesman = salesman.Id;
                                                            customer.LastName = primerApellido;
                                                            customer.MiddleLastName= segundoApellido;
                                                            customer.Movil = celular;
                                                            customer.Phone = telefono;
                                                            customer.ResidenceAddress = direc;
                                                            _unitOfWork.Customer.Update(customer);
                                                        }
                                                        else // Debemos crear el cliente
                                                        {
                                                            IdentificationType it = identificationTypes.Where(i => i.Alias.Equals(tipoDoc)).FirstOrDefault();
                                                            customer = new Customer
                                                            {
                                                                Email = email,
                                                                FirstName = primerNombre,
                                                                MiddleName = segundoNombre,
                                                                IdCustomerType = int.Parse(tipoCliente),
                                                                IdentificationNumber = cedula,
                                                                IdIdentificationType = it.Id,
                                                                IdSalesman = salesman.Id,
                                                                LastName = primerApellido,
                                                                MiddleLastName= segundoApellido,
                                                                Leaflet = true,
                                                                Movil = celular,
                                                                Phone = telefono,
                                                                ResidenceAddress = direc,
                                                                ShowAll = false
                                                            };
                                                            if (fecNac)
                                                                customer.BirthDate = dFecNacimiento;
                                                            int idCustomer = _unitOfWork.Customer.Insert(customer);
                                                            // Debemos agregar la linea de negocio
                                                            CustomerBusinessUnit customerBusinessUnitNew = new CustomerBusinessUnit
                                                            {
                                                                IdBusinessUnitDetail = bud[0].Id,
                                                                IdCustomer = idCustomer,
                                                                State = "A",
                                                                Year = DateTime.Now.Year.ToString()
                                                            };
                                                            _unitOfWork.CustomerBusinessUnit.Insert(customerBusinessUnitNew);
                                                        }
                                                    }
                                                }
                                                row += 1;
                                            }
                                        }
                                    } while (reader.NextResult());
                                }
                            }
                            transaction.Complete();
                        }
                        else
                            return BadRequest();
                    }
                    else
                        return BadRequest();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error, Error: " + ex.Message + ", Cedula: " + cedula);
                }
            }
            return Ok();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadRenovacion")]
        public async Task<IActionResult> UploadRenovacion()
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    if (Request.Form != null)
                    {
                        var file = Request.Form.Files[0];
                        string salesman = Request.Form["idUser"];
                        string date = Request.Form["date"];
                        int idSalesman = salesman != null ? int.Parse(salesman) : 0;
                        DateTime dateRenewal = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        dateRenewal = new DateTime(dateRenewal.Year, dateRenewal.Month, 1);
                        int days = DateTime.DaysInMonth(dateRenewal.Year, dateRenewal.Month);
                        DateTime dateRenewalEnd = new DateTime(dateRenewal.Year, dateRenewal.Month, days);
                        List<Renewal> lstRenewal = _unitOfWork.Renewal.RenewalByUser(idSalesman).ToList();
                        SystemUser user = _unitOfWork.User.GetById(idSalesman);
                        Settings settings = _unitOfWork.Settings.GetList().FirstOrDefault();
                        int renewalNumber = settings.RenewalNumber + 1;
                        if (file.Length > 0)
                        {
                            BinaryReader b = new BinaryReader(file.OpenReadStream());
                            int count = (int)file.Length;
                            byte[] binData = b.ReadBytes(count);
                            using (MemoryStream stream = new MemoryStream(binData))
                            //using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                            {
                                int row = 0;
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    //Primero debemos crear o trar la renovación a procesar
                                    Renewal renewal = lstRenewal.Where(r => r.RenewalDate.Year.Equals(dateRenewal.Year) && r.RenewalDate.Month.Equals(dateRenewal.Month)).FirstOrDefault();
                                    int idRenewal = renewal == null ? 0 : renewal.Id;
                                    double TotalPrima = 0;
                                    if (renewal == null)
                                    {
                                        StringBuilder description = new StringBuilder("RENOVACIÓN ");
                                        description.Append(dateRenewal.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper());
                                        description.Append(" " + dateRenewal.ToString("yyyy", CultureInfo.CreateSpecificCulture("es")).ToUpper());
                                        description.Append(" " + user.FirstName + " " + user.LastName);
                                        renewal = new Renewal
                                        {
                                            CreationDate = DateTime.Now,
                                            Description = description.ToString(),
                                            IdUser = idSalesman,
                                            Number = renewalNumber,
                                            RenewalDate = dateRenewal
                                        };
                                        idRenewal = _unitOfWork.Renewal.Insert(renewal);
                                        settings.RenewalNumber = renewalNumber;
                                        _unitOfWork.Settings.Update(settings);
                                    }
                                    do
                                    {
                                        while (reader.Read()) //Each ROW
                                        {
                                            if (reader.FieldCount > 0)
                                            {
                                                object firstCell = reader.GetValue(0);
                                                if (firstCell != null)
                                                {
                                                    if (row > 0)
                                                    {
                                                        string poliza = reader.GetValue(1) != null ? reader.GetValue(1).ToString() : "";
                                                        string ordenRef = reader.GetValue(2) != null ? reader.GetValue(2).ToString() : "";
                                                        string aseguradora = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : "";
                                                        string ramo = reader.GetValue(4) != null ? reader.GetValue(4).ToString() : "";
                                                        string placa = reader.GetValue(5) != null ? reader.GetValue(5).ToString() : "";
                                                        string vigDesde = reader.GetValue(7) != null ? reader.GetValue(7).ToString() : "";
                                                        DateTime.TryParse(vigDesde, out DateTime dVigDesde);
                                                        string vigHasta = reader.GetValue(8) != null ? reader.GetValue(8).ToString() : "";
                                                        DateTime.TryParse(vigHasta, out DateTime dVigHasta);
                                                        string nitToma = reader.GetValue(10) != null ? reader.GetValue(10).ToString() : "";
                                                        string tomador = reader.GetValue(11) != null ? reader.GetValue(11).ToString() : "";
                                                        string mov = reader.GetValue(15) != null ? reader.GetValue(15).ToString() : "";
                                                        string cedula = reader.GetValue(16) != null ? reader.GetValue(16).ToString() : "";
                                                        string benef = reader.GetValue(18) != null ? reader.GetValue(18).ToString() : "";
                                                        double.TryParse(reader.GetValue(19) != null ? reader.GetValue(19).ToString() : "", out double vrCuoIni);
                                                        double.TryParse(reader.GetValue(29) != null ? reader.GetValue(29).ToString() : "", out double vrNeto);
                                                        TotalPrima = TotalPrima + vrNeto;
                                                        double.TryParse(reader.GetValue(30) != null ? reader.GetValue(30).ToString() : "", out double vrAsegurado);
                                                        int.TryParse(reader.GetValue(31) != null ? reader.GetValue(31).ToString() : "", out int plazoCte);
                                                        double.TryParse(reader.GetValue(32) != null ? reader.GetValue(32).ToString() : "", out double cuoMesCte);
                                                        StringBuilder subject = new StringBuilder("HACER RENOVACIÓN CON LA PLACA: ");
                                                        subject.Append(placa);
                                                        subject.Append(", TOMADOR: NIT: " + nitToma + " - " + tomador);
                                                        subject.Append(", VIG DESDE: " + dVigDesde.ToString("dd/MM/yyyy"));
                                                        subject.Append(", VIG HASTA: " + dVigHasta.ToString("dd/MM/yyyy"));
                                                        subject.Append(", POLIZA: " + poliza);
                                                        subject.Append(", ORDEN/REF: " + ordenRef);
                                                        subject.Append(", ASEGURADORA: " + aseguradora);
                                                        subject.Append(", RAMO: " + ramo);
                                                        subject.Append(", VR CUOTA INI: " + vrCuoIni);
                                                        subject.Append(", VR NETO: " + vrNeto);
                                                        subject.Append(", VR ASEGURADO: " + vrAsegurado);
                                                        subject.Append(", PLAZO CTE: " + plazoCte);
                                                        subject.Append(", VR CUOTA: " + cuoMesCte);
                                                        subject.Append(", BENEFICIARIO: " + benef);
                                                        Customer customer = _unitOfWork.Customer.CustomerByIdentificationNumber(cedula);
                                                        if (customer == null && cedula.Length > 9)
                                                        {
                                                            cedula = cedula.Substring(0, 9);
                                                            customer = _unitOfWork.Customer.CustomerByIdentificationNumber(cedula);
                                                        }
                                                        if (customer == null)
                                                        {
                                                            return BadRequest("El prospecto/cliente con cédula :" + cedula + ", no existe en la base de datos");
                                                        }
                                                        Management management = new Management
                                                        {
                                                            ManagementType = "T",
                                                            IdCustomer = customer.Id,
                                                            CreationUser = int.Parse(idUser),
                                                            StartDate = dateRenewal,
                                                            EndDate = dVigHasta,
                                                            State = "P",
                                                            DelegatedUser = idSalesman,
                                                            Subject = subject.ToString(),
                                                            ManagementPartner = "C",
                                                            IsExtra = false,
                                                            IsRenewal = true,
                                                            IdRenewal = idRenewal
                                                        };
                                                        _unitOfWork.Management.Insert(management);
                                                    }
                                                }
                                                row += 1;
                                            }
                                        }
                                    } while (reader.NextResult());

                                    renewal.Quantity = row;
                                    renewal.VrPrima = TotalPrima;
                                    _unitOfWork.Renewal.Update(renewal);
                                }
                            }
                            transaction.Complete();
                        }
                        else
                        {
                            return BadRequest("EL archivo se encuentra vacío");
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error, Error: " + ex.Message);
                }
            }
            return Ok();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadInclusion")]
        public async Task<IActionResult> UploadInclusion()
        {
            using (var transaction = new TransactionScope())
            {

            }
            return Ok();
        }


    }
}
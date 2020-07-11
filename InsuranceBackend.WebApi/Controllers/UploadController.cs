using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
                                                        string tipoRec = "";
                                                        if (cuenta.Equals("cuenta 7370"))
                                                            tipoRec = "BC";
                                                        if (cuenta.Equals("cuenta 8165"))
                                                            tipoRec = "B7";
                                                        PaymentType paymentType = _unitOfWork.PaymentType.GetList().Where(p => p.Id.Equals("D3")).FirstOrDefault();                                                        //Recaudo
                                                        paymentType.Number += 1;
                                                        _unitOfWork.PaymentType.Update(paymentType);
                                                        String idWayToPay = "";
                                                        List<WaytoPay> waytoPays = _unitOfWork.WaytoPay.GetWaytoPaysByPaymentType(tipoRec).ToList();
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
                                                            IdPaymentType = tipoRec,
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
                        int idPolicyHeader = idPol != null ? int.Parse(idPol) : 0;
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
                                                    string tipoCliente = reader.GetValue(4) != null ? reader.GetValue(4).ToString() : "";
                                                    string nombre = reader.GetValue(5) != null ? reader.GetValue(5).ToString() : "";
                                                    string segundoNombre = reader.GetValue(6) != null ? reader.GetValue(6).ToString() : "";
                                                    string apellido = reader.GetValue(7) != null ? reader.GetValue(7).ToString() : "";
                                                    string segundoApellido = reader.GetValue(8) != null ? reader.GetValue(8).ToString() : "";
                                                    string sexo = reader.GetValue(9) != null ? reader.GetValue(9).ToString() : "";
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
                                                            IdGender = int.Parse(sexo),
                                                            IdIdentificationType = 1,
                                                            LastName = apellido,
                                                            MiddleLastName = segundoApellido,
                                                            MiddleName = segundoNombre,
                                                            Leaflet = false,
                                                            Movil = celular,
                                                            Phone = telef,
                                                            ResidenceAddress = direc
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

                                                    //Poliza
                                                    Policy policyHeader = _unitOfWork.Policy.GetById(idPolicyHeader);
                                                    int.TryParse(reader.GetValue(33) != null ? reader.GetValue(33).ToString() : "", out int cuotas);
                                                    double.TryParse(reader.GetValue(34) != null ? reader.GetValue(34).ToString() : "", out double prima);
                                                    double.TryParse(reader.GetValue(37) != null ? reader.GetValue(37).ToString() : "", out double iva);
                                                    double.TryParse(reader.GetValue(39) != null ? reader.GetValue(39).ToString() : "", out double total);
                                                    double.TryParse(reader.GetValue(40) != null ? reader.GetValue(40).ToString() : "", out double cuomescte);
                                                    double.TryParse(reader.GetValue(42) != null ? reader.GetValue(42).ToString() : "", out double otrosEx);
                                                    string certificado = reader.GetValue(69) != null ? reader.GetValue(69).ToString() : "";
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
                                                        IdMovementType = "I",
                                                        IdPaymentMethod = "1", //Preguntar
                                                        //IdPolicyHolder = policyHeader.IdPolicyHolder,
                                                        IdPolicyState = "1",
                                                        IdPolicyType = policyHeader.IdPolicyType,
                                                        IdUser = int.Parse(idUser),
                                                        IdVehicle = idVehicle,
                                                        InitialFee = 0,
                                                        Inspected = "I",
                                                        IsAttached = true,
                                                        IsHeader = false,
                                                        IsOrder = false,
                                                        Iva = iva,
                                                        License = placa,
                                                        NetValue = prima,
                                                        //Number = policyHeader.Number,
                                                        PendingRegistration = "N",
                                                        PremiumExtra = 0,
                                                        PremiumValue = prima,
                                                        ReqAuthorization = false,
                                                        ReqAuthorizationDisc = false,
                                                        Runt = 0,
                                                        StartDate = policyHeader.StartDate,
                                                        TotalValue = total,
                                                        IdSalesMan = 64, //Preguntar
                                                        Certificate = certificado,
                                                        IdPolicyHeader = policyHeader.Id
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
                                                    //Cuotas
                                                    PolicyFee policyFee = new PolicyFee
                                                    {
                                                        Date = policyHeader.StartDate.Value,
                                                        DatePayment = policyHeader.StartDate.Value,
                                                        IdPolicy = idPolicy,
                                                        Number = 1,
                                                        Value = total
                                                    };
                                                }
                                                row += 1;
                                            }
                                        }
                                    } while (reader.NextResult()); //Move to NEXT SHEET
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
                    return StatusCode(500, "Internal server error, Error: " + ex.Message);
                }
            }
            return Ok();
        }
    }
}
 
 
 
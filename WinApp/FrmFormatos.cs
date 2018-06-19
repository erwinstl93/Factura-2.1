using System;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Forms;
using WinApp.API;
using WinApp.Comun.Dto.Intercambio;
using WinApp.Comun.Dto.Modelos;
using WinApp.Firmado;
using WinApp.Properties;
using WinApp.Servicio;
using WinApp.Servicio.Soap;

namespace WinApp
{
    public partial class FrmFormatos : PlantillaBase
    {
        #region Variables Privadas
        private readonly DocumentoElectronico _documento;
        #endregion

        #region Propiedades
        public string RutaArchivo { get; set; }
        public string IdDocumento { get; set; }
        #endregion

        #region Constructores
        public FrmFormatos()
        {
            InitializeComponent();
            _documento = new DocumentoElectronico();
        }

        public FrmFormatos(DocumentoElectronico documento)
        {
            InitializeComponent();
            _documento = new DocumentoElectronico();
        }
        #endregion

        #region Metodos de llendo de datos

        private void DatosBoleta()
        {
            var dtsEmisor = new Contribuyente()
            {
                NroDocumento = "20525411401",
                TipoDocumento = "6",
                Direccion = "MZA. 228 LOTE. 06 ZONA INDUSTRIAL  PIURA - PIURA - PIURA",
                Departamento = "PIURA",
                Provincia = "PIURA",
                Distrito = "PIURA",
                NombreLegal = "PIURAMAQ S.R.L.",
                NombreComercial = "",
                Ubigeo = "200101"

            };

            var dtsReceptor = new Contribuyente()
            {
                NroDocumento = "47230861",
                TipoDocumento = "1",
                NombreLegal = "MEJIA MOSCOL JUAN JOSE",
                NombreComercial = "",
                Direccion = "JR. TUMBES NRO. 100 CENTRO PIURA (A 1 CUADRA DE AV. LIBERTAD CON BOLOGNESI)"
            };

            var dtsItems = new DetalleDocumento
            {
                Id = 1,
                Cantidad = 2000,
                UnidadMedida = "NIU",
                CodigoItem = "COD001",
                Descripcion = "PRODUCTO PRUEBA",
                PrecioUnitario =21.92m,
                PrecioReferencial =25.86m,
                TipoPrecio = "01",
                TipoImpuesto = "10",
                OtroImpuesto  = 0,
                Descuento = 0,
                Suma = 2000 * 21.92m, //_detalle.PrecioUnitario * _detalle.Cantidad
                Impuesto = (2000 * 21.92m) * _documento.CalculoIgv, //_detalle.Suma * _documento.CalculoIgv
                ImpuestoSelectivo = 0, //_detalle.Suma * _documento.CalculoIsc;
                TotalVenta = (2000 * 21.92m) - 0 //_detalle.Suma - _detalle.Descuento
            };

            _documento.IdDocumento = "B001-00000001";
            _documento.TipoDocumento = "03";
            _documento.Emisor = dtsEmisor;
            _documento.Receptor = dtsReceptor;
            _documento.FechaEmision = DateTime.Today.ToShortDateString();
            _documento.Moneda = "PEN";
            _documento.TipoOperacion = "0101";
            //Agregamos Detalle
            _documento.Items.Add(dtsItems);
            CalcularTotales();
        }
       
        private void DatosFactura()
        {
            var dtsEmisor = new Contribuyente()
            {
                NroDocumento = "20525411401",
                TipoDocumento = "6",
                Direccion = "MZA. 228 LOTE. 06 ZONA INDUSTRIAL  PIURA - PIURA - PIURA",
                Departamento = "PIURA",
                Provincia = "PIURA",
                Distrito = "PIURA",
                NombreLegal = "PIURAMAQ S.R.L.",
                NombreComercial = "",
                Ubigeo = "200101"

            };

            var dtsReceptor = new Contribuyente()
            {
                NroDocumento = "10472308616",
                TipoDocumento = "6",
                NombreLegal = "MEJIA MOSCOL JUAN JOSE",
                NombreComercial = "",
                Direccion = "JR. TUMBES NRO. 100 CENTRO PIURA (A 1 CUADRA DE AV. LIBERTAD CON BOLOGNESI)"
            };

            var dtsItems = new DetalleDocumento
            {
                Id = 1,
                Cantidad = 2000,
                UnidadMedida = "NIU",
                CodigoItem = "COD001",
                Descripcion = "PRODUCTO PRUEBA",
                PrecioUnitario = 21.92m,
                PrecioReferencial = 25.86m,
                TipoPrecio = "01",
                TipoImpuesto = "10",
                OtroImpuesto = 0,
                Descuento = 0,
                Suma = 2000 * 21.92m, //_detalle.PrecioUnitario * _detalle.Cantidad
                Impuesto = (2000 * 21.92m) * _documento.CalculoIgv, //_detalle.Suma * _documento.CalculoIgv
                ImpuestoSelectivo = 0, //_detalle.Suma * _documento.CalculoIsc;
                TotalVenta = (2000 * 21.92m) - 0 //_detalle.Suma - _detalle.Descuento
            };

            _documento.IdDocumento = "F001-00000001";
            _documento.TipoDocumento = "01";
            _documento.Emisor = dtsEmisor;
            _documento.Receptor = dtsReceptor;
            _documento.FechaEmision = DateTime.Today.ToShortDateString();
            _documento.Moneda = "PEN";
            _documento.TipoOperacion = "0101"; //Venta interna
            //Agregamos Detalle
            _documento.Items.Add(dtsItems);
            CalcularTotales();
        }

        private void DatosNCredito()
        {
            var dtsEmisor = new Contribuyente()
            {
                NroDocumento = "20525411401",
                TipoDocumento = "6",
                Direccion = "MZA. 228 LOTE. 06 ZONA INDUSTRIAL  PIURA - PIURA - PIURA",
                Departamento = "PIURA",
                Provincia = "PIURA",
                Distrito = "PIURA",
                NombreLegal = "PIURAMAQ S.R.L.",
                NombreComercial = "",
                Ubigeo = "200101"

            };

            var dtsReceptor = new Contribuyente()
            {
                NroDocumento = "10472308616",
                TipoDocumento = "06",
                NombreLegal = "MEJIA MOSCOL JUAN JOSE",
                NombreComercial = "",
                Direccion = "JR. TUMBES NRO. 100 CENTRO PIURA (A 1 CUADRA DE AV. LIBERTAD CON BOLOGNESI)"
            };

            var dtsItems = new DetalleDocumento
            {
                Id = 1,
                Cantidad = 2000,
                UnidadMedida = "NIU",
                CodigoItem = "COD001",
                Descripcion = "PRODUCTO PRUEBA",
                PrecioUnitario = 21.92m,
                PrecioReferencial = 25.86m,
                TipoPrecio = "01",
                TipoImpuesto = "10",
                OtroImpuesto = 0,
                Descuento = 0,
                Suma = 2000 * 21.92m, //_detalle.PrecioUnitario * _detalle.Cantidad
                Impuesto = (2000 * 21.92m) * _documento.CalculoIgv, //_detalle.Suma * _documento.CalculoIgv
                ImpuestoSelectivo = 0, //_detalle.Suma * _documento.CalculoIsc;
                TotalVenta = (2000 * 21.92m) - 0 //_detalle.Suma - _detalle.Descuento
            };

            _documento.IdDocumento = "FNC1-00000001";
            _documento.TipoDocumento = "07";
            _documento.Emisor = dtsEmisor;
            _documento.Receptor = dtsReceptor;
            _documento.FechaEmision = DateTime.Today.ToShortDateString();
            _documento.Moneda = "PEN";
            _documento.TipoOperacion = "01";
            //Agregamos Detalle
            _documento.Items.Add(dtsItems);
            CalcularTotales();

            //Nota de Credito
            var dtsDocumentoRelacionado = new DocumentoRelacionado
            {
                NroDocumento = "F001-00000001",
                TipoDocumento = "01"
            };
            _documento.Relacionados.Add(dtsDocumentoRelacionado);

            var dtsDiscrepancia = new Discrepancia
            {
                NroReferencia = "F001-00000001",
                Tipo = "01",
                Descripcion = "Nota de Credito del usuario admin"
            };
            _documento.Discrepancias.Add(dtsDiscrepancia);
        }

        private void DatosNDebito()
        {
            var dtsEmisor = new Contribuyente()
            {
                NroDocumento = "20525911129",
                TipoDocumento = "06",
                NombreLegal = "PUBLIARTE S.R.L",
                NombreComercial = "",
                Direccion = "JR. CALLAO NRO. 778 INT. 001 (AL LADO DE TALLER MECANICO)",
                Departamento = "PIURA",
                Provincia = "PIURA",
                Distrito = "PIURA",
                Ubigeo = "190101",
                Urbanizacion = ""

            };

            var dtsReceptor = new Contribuyente()
            {
                NroDocumento = "10472308616",
                TipoDocumento = "06",
                NombreLegal = "MEJIA MOSCOL JUAN JOSE",
                NombreComercial = "",
                Direccion = "JR. TUMBES NRO. 100 CENTRO PIURA (A 1 CUADRA DE AV. LIBERTAD CON BOLOGNESI)"
            };

            var dtsItems = new DetalleDocumento
            {
                Id = 1,
                Cantidad = 1,
                UnidadMedida = "NIU",
                CodigoItem = "COD001",
                Descripcion = "PRODUCTO PRUEBA",
                PrecioUnitario = 10.50m,
                PrecioReferencial = 0,
                TipoPrecio = "01",
                TipoImpuesto = "10",
                OtroImpuesto = 0,
                Descuento = 0,
                Suma = 1 * 10.50m, //_detalle.PrecioUnitario * _detalle.Cantidad
                Impuesto = (1 * 10.50m) * _documento.CalculoIgv, //_detalle.Suma * _documento.CalculoIgv
                ImpuestoSelectivo = 0, //_detalle.Suma * _documento.CalculoIsc;
                TotalVenta = (1 * 10.50m) - 0 //_detalle.Suma - _detalle.Descuento
            };

            _documento.IdDocumento = "ND01-00000001";
            _documento.TipoDocumento = "08";
            _documento.Emisor = dtsEmisor;
            _documento.Receptor = dtsReceptor;
            _documento.FechaEmision = DateTime.Today.ToShortDateString();
            _documento.Moneda = "PEN";
            _documento.TipoOperacion = "01";
            //Agregamos Detalle
            _documento.Items.Add(dtsItems);
            CalcularTotales();

            //Nota de Credito
            var dtsDocumentoRelacionado = new DocumentoRelacionado
            {
                NroDocumento = "F001-00000001",
                TipoDocumento = "01"
            };
            _documento.Relacionados.Add(dtsDocumentoRelacionado);

            var dtsDiscrepancia = new Discrepancia
            {
                NroReferencia = "F001-00000001",
                Tipo = "01",
                Descripcion = "Nota de Debito del usuario admin"
            };
            _documento.Discrepancias.Add(dtsDiscrepancia);
        }
        #endregion

        #region Metodos Privados

        private void CalcularTotales()
        {
            // Realizamos los cálculos respectivos.
            _documento.TotalIgv = _documento.Items.Sum(d => d.Impuesto);
            _documento.TotalIsc = _documento.Items.Sum(d => d.ImpuestoSelectivo);
            _documento.TotalOtrosTributos = _documento.Items.Sum(d => d.OtroImpuesto);

            _documento.Gravadas = _documento.Items
                .Where(d => d.TipoImpuesto.StartsWith("1"))
                .Sum(d => d.Suma);

            _documento.Exoneradas = _documento.Items
                .Where(d => d.TipoImpuesto.Contains("20"))
                .Sum(d => d.Suma);

            _documento.Inafectas = _documento.Items
                .Where(d => d.TipoImpuesto.StartsWith("3") || d.TipoImpuesto.Contains("40"))
                .Sum(d => d.Suma);

            _documento.Gratuitas = _documento.Items
                .Where(d => d.TipoImpuesto.Contains("21"))
                .Sum(d => d.Suma);
            _documento.LineCountNumeric = Convert.ToString(_documento.Items.Count());
            // Cuando existe ISC se debe recalcular el IGV.
            if (_documento.TotalIsc > 0)
            {
                _documento.TotalIgv = (_documento.Gravadas + _documento.TotalIsc) * _documento.CalculoIgv;
                // Se recalcula nuevamente el Total de Venta.
            }

            _documento.TotalVenta = _documento.Gravadas + _documento.Exoneradas + _documento.Inafectas +
                                    _documento.TotalIgv + _documento.TotalIsc + _documento.TotalOtrosTributos;

        }

        private void Hablar()
        {
            if (string.IsNullOrEmpty(txtResult.Text)) return;
            var synth = new SpeechSynthesizer();

            synth.SetOutputToDefaultAudioDevice();
            synth.SpeakAsync(txtResult.Text);
        }
        #endregion

        #region Generadores
        private async void BtnFactura_Click(object sender, System.EventArgs e)
        {
            DatosFactura();

            ISerializador serializador = new Serializador();
            DocumentoResponse response = new DocumentoResponse
            {
                Exito = false
            };
            response = await new GenerarFactura(serializador).Post(_documento);

            if (!response.Exito)
                throw new ApplicationException(response.MensajeError);

            RutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"{_documento.IdDocumento}.xml");

            File.WriteAllBytes(RutaArchivo, Convert.FromBase64String(response.TramaXmlSinFirma));
        }

        private async void BtnBoleta_Click(object sender, EventArgs e)
        {
            DatosBoleta();

            ISerializador serializador = new Serializador();
            DocumentoResponse response = new DocumentoResponse
            {
                Exito = false
            };
            response = await new GenerarFactura(serializador).Post(_documento);

            if (!response.Exito)
                throw new ApplicationException(response.MensajeError);

            RutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"{_documento.IdDocumento}.xml");

            File.WriteAllBytes(RutaArchivo, Convert.FromBase64String(response.TramaXmlSinFirma));
        }

        private async void BtnNotaCredito_Click(object sender, EventArgs e)
        {
            DatosNCredito();
            ISerializador serializador = new Serializador();
            DocumentoResponse response = new DocumentoResponse
            {
                Exito = false
            };
            response = await new GenerarNotaCredito(serializador).Post(_documento);

            if (!response.Exito)
                throw new ApplicationException(response.MensajeError);

            RutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"{_documento.IdDocumento}.xml");

            File.WriteAllBytes(RutaArchivo, Convert.FromBase64String(response.TramaXmlSinFirma));
        }

        private async void BtnNotaDebito_Click(object sender, EventArgs e)
        {
            DatosNDebito();
            ISerializador serializador = new Serializador();
            DocumentoResponse response = new DocumentoResponse
            {
                Exito = false
            };
            response = await new GenerarNotaDedito(serializador).Post(_documento);

            if (!response.Exito)
                throw new ApplicationException(response.MensajeError);

            RutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"{_documento.IdDocumento}.xml");

            File.WriteAllBytes(RutaArchivo, Convert.FromBase64String(response.TramaXmlSinFirma));
        }
        #endregion

        private async void BtnFirmar_Click(object sender, EventArgs e)
        {
            try
            {
               // _documento.IdDocumento = "F001-00000001"; //Probar

                if (string.IsNullOrEmpty(_documento.IdDocumento))
                throw new InvalidOperationException("La Serie y el Correlativo no pueden estar vacíos");

                var tramaXmlSinFirma = Convert.ToBase64String(File.ReadAllBytes(RutaArchivo)); //Original
                //var tramaXmlSinFirma = Convert.ToBase64String(File.ReadAllBytes(@"D:\Valle\XML_SF\F001-00000001.xml"));
                
                var firmadoRequest = new FirmadoRequest
                {
                    TramaXmlSinFirma = tramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes(@"D:\Valle\certificado\cervallespot.pfx")),
                    PasswordCertificado = "#1DEFRDE32W",
                    UnSoloNodoExtension = false //rbRetenciones.Checked || rbResumen.Checked
                };

                ICertificador certificador = new Certificador();
                var respuestaFirmado = await new Firmar(certificador).Post(firmadoRequest);

                if (!respuestaFirmado.Exito)
                    throw new ApplicationException(respuestaFirmado.MensajeError);


                RutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $"CF_{_documento.IdDocumento}.xml");

                File.WriteAllBytes(RutaArchivo, Convert.FromBase64String(respuestaFirmado.TramaXmlFirmado));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void EnvDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                var fileCF = Convert.ToBase64String(File.ReadAllBytes(RutaArchivo));

                var enviarDocumentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = "20525411401",
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService",
                    IdDocumento = _documento.IdDocumento,
                    TipoDocumento = _documento.TipoDocumento,
                    TramaXmlFirmado = fileCF
                };

                ISerializador serializador = new Serializador();
                IServicioSunatDocumentos servicioSunatDocumentos = new ServicioSunatDocumentos();

                RespuestaComunConArchivo respuestaEnvio;
                respuestaEnvio = await new EnviarDocumento(serializador, servicioSunatDocumentos).Post(enviarDocumentoRequest);

                var rpta = (EnviarDocumentoResponse)respuestaEnvio;
                txtResult.Text = $@"{Resources.procesoCorrecto}{Environment.NewLine}{rpta.MensajeRespuesta} siendo {DateTime.Now}";
                try
                {
                    if (rpta.Exito && !string.IsNullOrEmpty(rpta.TramaZipCdr))
                    {
                        File.WriteAllBytes($"{Program.CarpetaXml}\\{respuestaEnvio.NombreArchivo}.xml",
                            Convert.FromBase64String(fileCF));

                        File.WriteAllBytes($"{Program.CarpetaCdr}\\R-{respuestaEnvio.NombreArchivo}.zip",
                            Convert.FromBase64String(rpta.TramaZipCdr));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (!respuestaEnvio.Exito)
                    throw new ApplicationException(respuestaEnvio.MensajeError);
                Hablar();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}

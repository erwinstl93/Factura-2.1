﻿using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Speech.Synthesis;
using System.Windows.Forms;
using WinApp.API;
using WinApp.Comun.Dto.Intercambio;
using WinApp.Comun.Dto.Modelos;
using WinApp.Datos;
using WinApp.Entidades;
using WinApp.Firmado;
using WinApp.Properties;
using WinApp.Servicio;
using WinApp.Servicio.Soap;

namespace WinApp
{
    public partial class FrmEnviarSunat : PlantillaBase
    {
        #region Variables Privadas

        private FrmDocumento _frmDocumento;
        private readonly HttpClient _client;

        #endregion Variables Privadas

        #region Constructor

        public FrmEnviarSunat()
        {
            InitializeComponent();

            _client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlOpenInvoicePeruApi"]) };

            Load += (s, e) =>
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    using (var ctx = new OpenInvoicePeruDb())
                    {
                        direccionSunatBindingSource.DataSource = ctx.DireccionesSunat.ToList();
                        direccionSunatBindingSource.ResetBindings(false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.InnerException?.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            };
        }

        #endregion Constructor

        #region Botones de Busqueda

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = Resources.seleccionXml;
                    ofd.Filter = Resources.formatosXml;
                    ofd.FilterIndex = 1;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtSource.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnBrowseCert_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = Resources.seleccioneCertificado;
                    ofd.Filter = Resources.formatosCertificado;
                    ofd.FilterIndex = 1;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtRutaCertificado.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Botones de Busqueda

        #region LLamadas Asincronas

        private async void BtnGen_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string codigoTipoDoc;
                switch (cboTipoDoc.SelectedIndex)
                {
                    case 0:
                        codigoTipoDoc = "01";
                        break;

                    case 1:
                        codigoTipoDoc = "03";
                        break;

                    case 2:
                        codigoTipoDoc = "07";
                        break;

                    case 3:
                        codigoTipoDoc = "08";
                        break;

                    case 4:
                        codigoTipoDoc = "20";
                        break;

                    case 5:
                        codigoTipoDoc = "40";
                        break;

                    case 6:
                        codigoTipoDoc = "RC";
                        break;

                    case 7:
                        codigoTipoDoc = "RA";
                        break;

                    case 8:
                        codigoTipoDoc = "09";
                        break;

                    default:
                        codigoTipoDoc = "01";
                        break;
                }

                if (string.IsNullOrEmpty(txtSerieCorrelativo.Text))
                    throw new InvalidOperationException("La Serie y el Correlativo no pueden estar vacíos");

                var tramaXmlSinFirma = Convert.ToBase64String(File.ReadAllBytes(txtSource.Text));

                var firmadoRequest = new FirmadoRequest
                {
                    TramaXmlSinFirma = tramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes(txtRutaCertificado.Text)),
                    PasswordCertificado = txtPassCertificado.Text,
                    UnSoloNodoExtension = rbRetenciones.Checked || rbResumen.Checked
                };

                ICertificador certificador = new Certificador();
                var respuestaFirmado = await new Firmar(certificador).Post(firmadoRequest);

                if (!respuestaFirmado.Exito)
                    throw new ApplicationException(respuestaFirmado.MensajeError);

                var enviarDocumentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = txtNroRuc.Text,
                    UsuarioSol = txtUsuarioSol.Text,
                    ClaveSol = txtClaveSol.Text,
                    EndPointUrl = ValorSeleccionado(),
                    IdDocumento = txtSerieCorrelativo.Text,
                    TipoDocumento = codigoTipoDoc,
                    TramaXmlFirmado = respuestaFirmado.TramaXmlFirmado
                };

                ISerializador serializador = new Serializador();
                IServicioSunatDocumentos servicioSunatDocumentos = new ServicioSunatDocumentos();

                RespuestaComunConArchivo respuestaEnvio;

                if (rbResumen.Checked && codigoTipoDoc != "09")
                {
                    respuestaEnvio = await new EnviarResumen(serializador, servicioSunatDocumentos).Post(enviarDocumentoRequest);
                }
                else
                {
                    respuestaEnvio = await new EnviarDocumento(serializador, servicioSunatDocumentos).Post(enviarDocumentoRequest);
                }
                
                if (!rbResumen.Checked)
                {
                    var rpta = (EnviarDocumentoResponse)respuestaEnvio;
                    txtResult.Text = $@"{Resources.procesoCorrecto}{Environment.NewLine}{rpta.MensajeRespuesta} siendo las {DateTime.Now}";
                    try
                    {
                        if (rpta.Exito && !string.IsNullOrEmpty(rpta.TramaZipCdr))
                        {
                            File.WriteAllBytes($"{Program.CarpetaXml}\\{respuestaEnvio.NombreArchivo}.xml",
                                Convert.FromBase64String(respuestaFirmado.TramaXmlFirmado));

                            File.WriteAllBytes($"{Program.CarpetaCdr}\\R-{respuestaEnvio.NombreArchivo}.zip",
                                Convert.FromBase64String(rpta.TramaZipCdr));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    var rpta = (EnviarResumenResponse)respuestaEnvio;
                    txtResult.Text = $@"{Resources.procesoCorrecto}{Environment.NewLine}{rpta.NroTicket}";
                }

                if (!respuestaEnvio.Exito)
                    throw new ApplicationException(respuestaEnvio.MensajeError);

                if (chkVoz.Checked)
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

        private async void BtnGetStatus_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                using (var frm = new FrmTicket())
                {
                    if (frm.ShowDialog() != DialogResult.OK) return;
                    if (string.IsNullOrEmpty(frm.txtNroTicket.Text)) return;

                    var consultaTicketRequest = new ConsultaTicketRequest
                    {
                        Ruc = txtNroRuc.Text,
                        UsuarioSol = txtUsuarioSol.Text,
                        ClaveSol = txtClaveSol.Text,
                        EndPointUrl = ValorSeleccionado(),
                        IdDocumento = txtSerieCorrelativo.Text,
                        NroTicket = frm.txtNroTicket.Text
                    };

                    ISerializador serializador = new Serializador();
                    IServicioSunatDocumentos servicioSunatDocumentos = new ServicioSunatDocumentos();
                    EnviarDocumentoResponse respuestaEnvio = await new ConsultarTicket(servicioSunatDocumentos, serializador).Post(consultaTicketRequest);

                    if (!respuestaEnvio.Exito || !string.IsNullOrEmpty(respuestaEnvio.MensajeError))
                        throw new InvalidOperationException(respuestaEnvio.MensajeError);

                    File.WriteAllBytes($"{Program.CarpetaCdr}\\R-{respuestaEnvio.NombreArchivo}.zip",
                        Convert.FromBase64String(respuestaEnvio.TramaZipCdr));

                    txtResult.Text = $@"{Resources.procesoCorrecto}{Environment.NewLine}{respuestaEnvio.MensajeRespuesta}";

                    if (chkVoz.Checked)
                        Hablar();
                }
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

        #endregion LLamadas Asincronas

        #region Generacion de XML

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (_frmDocumento == null)
                {
                    if (string.IsNullOrEmpty(txtNroRuc.Text))
                        _frmDocumento = new FrmDocumento();
                    else
                    {
                        var documento = new DocumentoElectronico
                        {
                            Emisor = { NroDocumento = txtNroRuc.Text },
                            FechaEmision = DateTime.Today.ToShortDateString()
                        };
                        _frmDocumento = new FrmDocumento(documento);
                    }
                }
                var rpta = _frmDocumento.ShowDialog(this);

                if (rpta != DialogResult.OK) return;

                txtSource.Text = _frmDocumento.RutaArchivo;
                txtSerieCorrelativo.Text = _frmDocumento.IdDocumento;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion Generacion de XML

        #region Metodos Privados

        private void Hablar()
        {
            if (string.IsNullOrEmpty(txtResult.Text)) return;
            var synth = new SpeechSynthesizer();

            synth.SetOutputToDefaultAudioDevice();
            synth.SpeakAsync(txtResult.Text);
        }

        private string ValorSeleccionado()
        {
            var direccionSunat = direccionSunatBindingSource.Current as DireccionSunat;
            return direccionSunat == null ? string.Empty : direccionSunat.Descripcion;
        }

        #endregion Metodos Privados
    }
}
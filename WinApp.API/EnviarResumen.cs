using System;
using System.Threading.Tasks;
using WinApp.Comun.Dto.Intercambio;
using WinApp.Firmado;
using WinApp.Servicio;

namespace WinApp.API
{
    public class EnviarResumen
    {
        private readonly ISerializador _serializador;
        private readonly IServicioSunatDocumentos _servicioSunatDocumentos;

        public EnviarResumen(ISerializador serializador, IServicioSunatDocumentos servicioSunatDocumentos)
        {
            _serializador = serializador;
            _servicioSunatDocumentos = servicioSunatDocumentos;
        }

        public async Task<EnviarResumenResponse> Post(EnviarDocumentoRequest request)
        {
            var response = new EnviarResumenResponse();
            var nombreArchivo = $"{request.Ruc}-{request.IdDocumento}";

            try
            {
                var tramaZip = await _serializador.GenerarZip(request.TramaXmlFirmado, nombreArchivo);

                _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                {
                    Ruc = request.Ruc,
                    UserName = request.UsuarioSol,
                    Password = request.ClaveSol,
                    EndPointUrl = request.EndPointUrl
                });

                var resultado = _servicioSunatDocumentos.EnviarResumen(new DocumentoSunat
                {
                    NombreArchivo = $"{nombreArchivo}.zip",
                    TramaXml = tramaZip
                });

                if (resultado.Exito)
                {
                    response.NroTicket = resultado.NumeroTicket;
                    response.Exito = true;
                    response.NombreArchivo = nombreArchivo;
                }
                else
                {
                    response.MensajeError = resultado.MensajeError;
                    response.Exito = false;
                }
            }
            catch (Exception ex)
            {
                response.MensajeError = ex.Message;
                response.Pila = ex.StackTrace;
                response.Exito = false;
            }

            return response;
        }
    }
}

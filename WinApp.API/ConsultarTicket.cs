using System;
using System.Threading.Tasks;
using WinApp.Comun.Dto.Intercambio;
using WinApp.Firmado;
using WinApp.Servicio;

namespace WinApp.API
{
    public class ConsultarTicket
    {
        private readonly IServicioSunatDocumentos _servicioSunatDocumentos;
        private readonly ISerializador _serializador;

        public ConsultarTicket(IServicioSunatDocumentos servicioSunatDocumentos, ISerializador serializador)
        {
            _servicioSunatDocumentos = servicioSunatDocumentos;
            _serializador = serializador;
        }

        public async Task<EnviarDocumentoResponse> Post(ConsultaTicketRequest request)
        {
            var response = new EnviarDocumentoResponse();

            try
            {
                _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                {
                    Ruc = request.Ruc,
                    UserName = request.UsuarioSol,
                    Password = request.ClaveSol,
                    EndPointUrl = request.EndPointUrl
                });

                var resultado = _servicioSunatDocumentos.ConsultarTicket(request.NroTicket);

                if (!resultado.Exito)
                {
                    response.Exito = false;
                    response.MensajeError = resultado.MensajeError;
                }
                else
                    response = await _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion);
            }
            catch (Exception ex)
            {
                response.MensajeError = ex.Source == "DotNetZip" ? "El Ticket no existe" : ex.Message;
                response.Pila = ex.StackTrace;
                response.Exito = false;
            }

            return response;
        }
    }
}

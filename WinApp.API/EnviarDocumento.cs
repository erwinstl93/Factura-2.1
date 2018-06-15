using System.Threading.Tasks;
using WinApp.Comun.Dto.Intercambio;
using WinApp.Firmado;
using WinApp.Servicio;

namespace WinApp.API
{
    public class EnviarDocumento
    {
        private readonly ISerializador _serializador;
        private readonly IServicioSunatDocumentos _servicioSunatDocumentos;

        public EnviarDocumento(ISerializador serializador, IServicioSunatDocumentos servicioSunatDocumentos)
        {
            _serializador = serializador;
            _servicioSunatDocumentos = servicioSunatDocumentos;
        }

        public async Task<EnviarDocumentoResponse> Post(EnviarDocumentoRequest request)
        {
            var response = new EnviarDocumentoResponse();
            var nombreArchivo = $"{request.Ruc}-{request.TipoDocumento}-{request.IdDocumento}";
            var tramaZip = await _serializador.GenerarZip(request.TramaXmlFirmado, nombreArchivo);

            _servicioSunatDocumentos.Inicializar(new ParametrosConexion
            {
                Ruc = request.Ruc,
                UserName = request.UsuarioSol,
                Password = request.ClaveSol,
                EndPointUrl = request.EndPointUrl
            });

            var resultado = _servicioSunatDocumentos.EnviarDocumento(new DocumentoSunat
            {
                TramaXml = tramaZip,
                NombreArchivo = $"{nombreArchivo}.zip"
            });

            if (!resultado.Exito)
            {
                response.Exito = false;
                response.MensajeError = resultado.MensajeError;
            }
            else
            {
                response = await _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion);
                // Quitamos la R y la extensión devueltas por el Servicio.
                response.NombreArchivo = nombreArchivo;
            }

            return response;
        }
    }
}

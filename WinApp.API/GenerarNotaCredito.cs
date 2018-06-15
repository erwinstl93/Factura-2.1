using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using WinApp.Comun.Dto.Intercambio;
using WinApp.Comun.Dto.Modelos;
using WinApp.Firmado;
using WinApp.Xml;

namespace WinApp.API
{
    public class GenerarNotaCredito
    {
        private readonly IDocumentoXml _documentoXml;
        private readonly ISerializador _serializador;

        public GenerarNotaCredito(ISerializador serializador)
        {
            _serializador = serializador;
            _documentoXml = _documentoXml = UnityConfig.GetConfiguredContainer()
                .Resolve<IDocumentoXml>(GetType().Name);
        }

        public async Task<DocumentoResponse> Post(DocumentoElectronico documento)
        {
            var response = new DocumentoResponse();
            try
            {
                var notaCredito = _documentoXml.Generar(documento);
                response.TramaXmlSinFirma = await _serializador.GenerarXml(notaCredito);
                response.Exito = true;
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

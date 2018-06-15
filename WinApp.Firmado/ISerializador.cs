using System.Threading.Tasks;
using WinApp.Comun;
using WinApp.Comun.Dto.Intercambio;

namespace WinApp.Firmado
{
    public interface ISerializador
    {
        Task<string> GenerarXml<T>(T objectToSerialize) where T : IEstructuraXml;
        Task<string> GenerarZip(string tramaXml, string nombreArchivo);
        Task<EnviarDocumentoResponse> GenerarDocumentoRespuesta(string constanciaRecepcion);
    }
}

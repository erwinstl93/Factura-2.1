using System.Threading.Tasks;
using WinApp.Comun.Dto.Intercambio;

namespace WinApp.Firmado
{
    public interface ICertificador
    {
        Task<FirmadoResponse> FirmarXml(FirmadoRequest request);
    }
}

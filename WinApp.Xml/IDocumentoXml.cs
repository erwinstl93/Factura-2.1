using WinApp.Comun;
using WinApp.Comun.Dto.Contratos;

namespace WinApp.Xml
{
    public interface IDocumentoXml
    {
        IEstructuraXml Generar(IDocumentoElectronico request);
    }
}

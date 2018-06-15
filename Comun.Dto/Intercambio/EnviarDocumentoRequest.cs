using Newtonsoft.Json;

namespace WinApp.Comun.Dto.Intercambio
{
    public class EnviarDocumentoRequest : EnvioDocumentoComun
    {
        [JsonProperty(Required = Required.Always)]
        public string TramaXmlFirmado { get; set; }
    }
}

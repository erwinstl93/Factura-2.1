using Newtonsoft.Json;

namespace WinApp.Comun.Dto.Modelos
{
    public class Discrepancia
    {
        [JsonProperty(Required = Required.Always)]
        public string NroReferencia { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Tipo { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Descripcion { get; set; }
    }
}

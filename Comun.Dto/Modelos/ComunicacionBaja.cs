using Newtonsoft.Json;
using System.Collections.Generic;

namespace WinApp.Comun.Dto.Modelos
{
    public class ComunicacionBaja : DocumentoResumen
    {
        [JsonProperty(Required = Required.Always)]
        public List<DocumentoBaja> Bajas { get; set; }
    }
}

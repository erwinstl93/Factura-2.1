using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinApp.Entidades
{
    public class DocumentoRelacionado : EntidadBase
    {
        public int IdCabeceraDocumento { get; set; }

        [ForeignKey(nameof(IdCabeceraDocumento))]
        public CabeceraDocumento CabeceraDocumento { get; set; }

        [Required]
        [MaxLength(15)]
        public string NroDocumento { get; set; }

        [Required]
        [MaxLength(2)]
        public string TipoDocumento { get; set; }
    }
}

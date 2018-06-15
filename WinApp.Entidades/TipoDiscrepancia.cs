using System.ComponentModel.DataAnnotations.Schema;

namespace WinApp.Entidades
{
    public class TipoDiscrepancia : TipoValorBase
    {
        public int IdTipoDocumento { get; set; }

        [ForeignKey(nameof(IdTipoDocumento))]
        public TipoDocumento TipoDocumento { get; set; }
    }
}

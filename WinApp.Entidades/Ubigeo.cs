using System.ComponentModel.DataAnnotations;

namespace WinApp.Entidades
{
    public class Ubigeo : EntidadBase
    {
        [Required]
        [MaxLength(6)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(250)]
        public string Descripcion { get; set; }

    }
}

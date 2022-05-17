using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class Package
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de envoltorio")]
        [MaxLength(60, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String WrapperType { get; set; }

        [Display(Name = "Alto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Height { get; set; }

        [Display(Name = "Largo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Long { get; set; }

        [Display(Name = "Ancho")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Widt { get; set; }

        [Display(Name = "¿Delicado?")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public bool Delicate { get; set; }

        [Display(Name = "Contenido del paquete")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Contain { get; set; }

        public User User { get; set; }
    }
}

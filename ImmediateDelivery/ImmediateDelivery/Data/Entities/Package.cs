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
        public int Width { get; set; }

        [Display(Name = "¿Delicado?")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public bool Delicate { get; set; }

        [Display(Name = "Contenido del paquete")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Contain { get; set; }

        [Display(Name = "Tipo paquete")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public PackageType PackageType { get; set; }

        public User User { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(35, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FullNameRecipient { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DocRecipient { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PhoneNumber { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string AddressRecipient { get; set; }

        public ICollection<SendDetail> SendDetails { get; set; }

    }
}
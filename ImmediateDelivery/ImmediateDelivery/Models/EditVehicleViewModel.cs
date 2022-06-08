using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class EditVehicleViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Placa")]
        [MaxLength(8, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Plaque { get; set; }

        [Display(Name = "Marca del Vehículo")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string BrandVehicle { get; set; }

        [Display(Name = "Color del Vehículo")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Color { get; set; }

    }
}

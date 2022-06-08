using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class VehicleType
    {

        public int Id { get; set; }

        [Display(Name = "Tipo de vehículo")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Type { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class NeighborhoodViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Barrio/Vereda")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }
        public int CityId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(80, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        public City City { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}

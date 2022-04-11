using ImmediateDelivery.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(80, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        public int NeighborhoodId { get; set; }


    }
}

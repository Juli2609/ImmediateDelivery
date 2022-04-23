using ImmediateDelivery.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(80, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }
        public int StateId { get; set; }


    }
}

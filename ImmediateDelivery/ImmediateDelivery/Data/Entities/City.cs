using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

    }
}

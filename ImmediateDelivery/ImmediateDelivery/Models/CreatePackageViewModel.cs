using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class CreatePackageViewModel : EditPackageViewModel
    {
        [Display(Name = "Tipo de paquete")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de paquete.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PackageTypeId { get; set; }

        public IEnumerable<SelectListItem> PackageTypes { get; set; }

        public int UserId { get; set; }

    }
}

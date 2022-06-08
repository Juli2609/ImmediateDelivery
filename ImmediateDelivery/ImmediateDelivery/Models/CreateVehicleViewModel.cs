using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Models
{
    public class CreateVehicleViewModel : EditVehicleViewModel
    {
        [Display(Name = "Tipo de vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de paquete.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int VehicleTypeId { get; set; }

        public IEnumerable<SelectListItem> VehicleTypes { get; set; }

        public string UserId { get; set; }

    }
}

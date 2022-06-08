
using System.ComponentModel.DataAnnotations;


namespace ImmediateDelivery.Data.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }


        [Display(Name = "Placa")] 
        [MaxLength(8, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Plaque { get; set; }

        [Display(Name = "Tipo de Vehículo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public VehicleType VehicleType { get; set; }

        [Display(Name = "Marca del Vehículo")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string BrandVehicle { get; set; }

        [Display(Name = "Color del Vehículo")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Color { get; set; }

        //[Display(Name = "Documentos del  Vehículo")]
        //public Guid DocumentsId { get; set; }

        //[Display(Name = "Documentos del  Vehículo")]
        //public string DocumentsFullPath => DocumentsId == Guid.Empty
        //    ? $"Sin archivo"
        //    : $"https://shoppingzulu.blob.core.windows.net/users/{DocumentsId}";

        //[Display(Name = "Foto")]
        //public Guid? ImageId { get; set; }

        //[Display(Name = "Foto")]
        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? $"https://localhost:7033/images/noimage.png"
        //    : $"https://shoppingzulu.blob.core.windows.net/products/{ImageId}";

        public User User { get; set; }

    }
}

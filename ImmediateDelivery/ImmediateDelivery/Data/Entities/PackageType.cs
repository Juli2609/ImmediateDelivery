using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class PackageType
    {
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        public Package Package { get; set; }
        public int PackageId { get; set; }
    }
}

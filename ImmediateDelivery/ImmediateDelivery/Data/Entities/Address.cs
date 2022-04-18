using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Neighborhood Neighborhood { get; set; }

        public ICollection<User> Users { get; set; }    
    }
}

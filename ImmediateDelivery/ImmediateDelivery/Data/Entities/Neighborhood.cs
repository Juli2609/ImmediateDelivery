using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ImmediateDelivery.Data.Entities
{
    public class Neighborhood
    {
        public int Id { get; set; }

        [Display(Name = "Barrio/Vereda")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }
       
        [JsonIgnore]
        public City City { get; set; }

        public ICollection<User> Users { get; set; }


    }
}

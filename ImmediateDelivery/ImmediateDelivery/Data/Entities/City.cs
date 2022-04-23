using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ImmediateDelivery.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        [JsonIgnore]
        public State State { get; set; }
        public ICollection<Neighborhood> Neighborhoods { get; set; }

        [Display(Name = "Barrio/Vereda")]
        public int NeighborhoodsNumber => Neighborhoods == null ? 0 : Neighborhoods.Count;

       
    }
}

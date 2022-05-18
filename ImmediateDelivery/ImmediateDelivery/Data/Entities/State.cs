using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Departamento")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudad")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

        [Display(Name = "Barrios/Veredas")]
        public int NeighborhoodsNumber => Cities == null ? 0 : Cities.Sum(c => c.NeighborhoodsNumber);



    }
}

using System.ComponentModel.DataAnnotations;

namespace ImmediateDelivery.Data.Entities
{
    public class Neighborhood
    {
        public int Id { get; set; }

        [Display(Name = "Barrio/Vereda")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }
        public City City { get; set; }

        public ICollection<Address> Addresses { get; set; }
       
        [Display(Name = "Direcciones")]
        public int AddressesNumber => Addresses == null ? 0 : Addresses.Count;
    }
}

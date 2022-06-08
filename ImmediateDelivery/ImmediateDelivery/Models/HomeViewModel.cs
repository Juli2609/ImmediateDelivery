using ImmediateDelivery.Data.Entities;

namespace ImmediateDelivery.Models
{
    public class HomeViewModel
    {
        public ICollection<User> Messengers { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public Vehicle Plaque { get; set; }
    }

}

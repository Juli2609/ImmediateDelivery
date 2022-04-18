using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Enums;
using ImmediateDelivery.Helpers;

namespace ImmediateDelivery.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCitiesAsync();
            await CheckRolesSAsync();
            await CheckUserAsync("1010", "Juliana", "Arroyave", "Juli@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

        }

        private async Task<User> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesSAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());

        }

        private async Task CheckCitiesAsync()
        {
            if (!_context.Cities.Any())
            {
                _context.Cities.Add(new City
                {
                    Name = "Bello",

                    Neighborhoods = new List<Neighborhood>()
                      {
                          new Neighborhood()
                          {
                              Name = "Camacol"
                          },
                          new Neighborhood() {Name = "Mirador"},
                          new Neighborhood() {Name = "Trapiche"},
                          new Neighborhood() {Name = "La cumbre"},
                          new Neighborhood() {Name = "Quitasol"},
                          new Neighborhood() {Name = "Niquia"}
                      }

                });
                _context.Cities.Add(new City { Name = "Envigado" });
                _context.Cities.Add(new City { Name = "Medellín" });
                _context.Cities.Add(new City { Name = "Barbosa" });
                _context.Cities.Add(new City { Name = "ItagÜí" });
                _context.Cities.Add(new City { Name = "Caldas" });
                _context.Cities.Add(new City { Name = "Girardota" });
                _context.Cities.Add(new City { Name = "Copacabana" });
                _context.Cities.Add(new City { Name = "Sabaneta" });
                _context.Cities.Add(new City { Name = "La Estrella" });

            }
            await _context.SaveChangesAsync();
        }
    }
}


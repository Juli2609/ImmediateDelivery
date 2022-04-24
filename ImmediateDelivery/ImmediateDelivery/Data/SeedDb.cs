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
            await CheckStatesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juliana", "Arroyave", "Juli@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("2020", "Mariana", "Raigosa", "Mari@yopmail.com", "311 322 2046", "Calle Sol Calle Luna", UserType.User);

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
                    Neighborhood = _context.Neighborhoods.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckStatesAsync()
        {
            if (!_context.States.Any())
            {
                _context.States.Add(new State
                {
                    Name = "Antioquia",

                    Cities = new List<City>()
                    {
                        new City()
                        {
                         Name = "Bello",

                           Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood(){Name = "Camacol"},
                              new Neighborhood() {Name = "Mirador"},
                              new Neighborhood() {Name = "Trapiche"},
                              new Neighborhood() {Name = "La cumbre"},
                              new Neighborhood() {Name = "Quitasol"},
                              new Neighborhood() {Name = "Niquia"}
                           }

                        },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Medellín" },
                        new City() { Name = "Barbosa" },
                        new City() { Name = "ItagÜí" },
                        new City() { Name = "Caldas" },
                        new City() { Name = "Girardota" },
                        new City() { Name = "Copacabana" },
                        new City() { Name = "Sabaneta" },
                        new City() { Name = "La Estrella" }
                    }
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}


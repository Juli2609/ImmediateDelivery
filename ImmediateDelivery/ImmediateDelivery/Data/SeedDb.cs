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
            await CheckUserAsync("3030", "Kang", "Song", "Kang@yopmail.com", "333 387 8765", "Calle 34 # 6 - 4 ", UserType.Messenger);

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
                              new Neighborhood() {Name = "Camacol"},
                              new Neighborhood() {Name = "Mirador"},
                              new Neighborhood() {Name = "Trapiche"},
                              new Neighborhood() {Name = "Quitasol"},
                              new Neighborhood() {Name = "Niquia"}
                           }

                        },
                        new City()
                        {
                            Name = "Envigado",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "San Lucas"},
                              new Neighborhood() {Name = "Zuñiga"},
                              new Neighborhood() {Name = "Las Antillas"},
                              new Neighborhood() {Name = "La Paz"},
                              new Neighborhood() {Name = "El Salado"},
                           }
                        },
                        new City()
                        {
                            Name = "Medellín",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "Castilla"},
                              new Neighborhood() {Name = "Adalucia"},
                              new Neighborhood() {Name = "Poblado"},
                              new Neighborhood() {Name = "Laureles"},
                              new Neighborhood() {Name = "Aranjuez"},
                           }
                        },
                        new City()
                        {
                            Name = "Barbosa",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "Altamira"},
                              new Neighborhood() {Name = "Isaza"},
                              new Neighborhood() {Name = "El Viento"},
                              new Neighborhood() {Name = "El Tigre"},
                              new Neighborhood() {Name = "La cejita"},
                           }
                        },
                        new City()
                        {
                            Name = "ItagÜí",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "La Maria"},
                              new Neighborhood() {Name = "Los Olivares"},
                              new Neighborhood() {Name = "El Progreso"},
                              new Neighborhood() {Name = "Ditaires"},
                              new Neighborhood() {Name = "El Ajizal"},
                           }
                        },
                        new City()
                        {
                            Name = "Caldas",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "La Pradera"},
                              new Neighborhood() {Name = "Olaya Herrera"},
                              new Neighborhood() {Name = "Cristo Rey"},
                              new Neighborhood() {Name = "Los Cerezos"},
                              new Neighborhood() {Name = "La Docena"},
                           }
                        },
                        new City()
                        {
                            Name = "Girardota",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "El Llano"},
                              new Neighborhood() {Name = "Monte Carlo"},
                              new Neighborhood() {Name = "San Jose"},
                              new Neighborhood() {Name = "Guyacanes"},
                              new Neighborhood() {Name = "El Naranjal"},
                           }
                        },
                        new City()
                        {
                            Name = "Copacabana",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "Machado"},
                              new Neighborhood() {Name = "El Pedregal"},
                              new Neighborhood() {Name = "La Misericordia"},
                              new Neighborhood() {Name = "La Azulita"},
                              new Neighborhood() {Name = "Villa Nueva"},
                           }
                        },
                        new City()
                        {
                            Name = "Sabaneta",

                            Neighborhoods = new List<Neighborhood>()
                           {
                              new Neighborhood() {Name = "Las Lomitas"},
                              new Neighborhood() {Name = "La Doctora"},
                              new Neighborhood() {Name = "Las Casitas"},
                              new Neighborhood() {Name = "Tres Esquinas"},
                              new Neighborhood() {Name = "Los Arias"},
                           }
                        },
                        new City()
                        {
                            Name = "La Estrella",

                            Neighborhoods = new List<Neighborhood>()
                            {
                              new Neighborhood() {Name = "El Dorado"},
                              new Neighborhood() {Name = "Monterrey"},
                              new Neighborhood() {Name = "Las Brisas"},
                              new Neighborhood() {Name = "Escobar"},
                              new Neighborhood() {Name = "Horizontes"},
                            }
                        }
                    }
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}

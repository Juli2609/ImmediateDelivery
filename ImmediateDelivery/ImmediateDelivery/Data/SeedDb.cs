using ImmediateDelivery.Data.Entities;

namespace ImmediateDelivery.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCitiesAsync();
        }

         private async Task CheckCitiesAsync()
          {
              if(!_context.Cities.Any())
              {
                  _context.Cities.Add(new City { 
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
        
    
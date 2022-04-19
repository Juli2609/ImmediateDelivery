using ImmediateDelivery.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImmediateDelivery.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboAddressesAsync(int neighborhoodId)
        {
            List<SelectListItem> list = await _context.Addresses
                .Where(s => s.Neighborhood.Id == neighborhoodId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione una dirección...", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync()
        {
            List<SelectListItem> list = await _context.Cities.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
              .OrderBy(c => c.Text)
              .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione una ciudad...", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboNeighborhodsAsync(int cityId)
        {
            List<SelectListItem> list = await _context.Neighborhoods
                .Where(s => s.City.Id == cityId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un Barrio/Vereda...", Value = "0" });
            return list;
        }
    }
}

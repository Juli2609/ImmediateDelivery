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
        
        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync()
        {
            List<SelectListItem> list = await _context.States.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
              .OrderBy(c => c.Text)
              .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un Departamento...", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities
                .Where(s => s.State.Id == stateId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione una ciudad...", Value = "0" });
            return list;

        }
        public async Task<IEnumerable<SelectListItem>> GetComboNeighborhoodsAsync(int cityId)
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

        public async Task<IEnumerable<SelectListItem>> GetComboPackageTypesAsync()
        {
                List<SelectListItem> list = await _context.PackageTypes.Select(c => new SelectListItem
                {
                    Text = c.Description,
                    Value = c.Id.ToString()
                })
                 .OrderBy(c => c.Text)
                 .ToListAsync();

                list.Insert(0, new SelectListItem { Text = "[Seleccione un Tipo de paquete...", Value = "0" });
                return list;
            
        }

        public async Task<IEnumerable<SelectListItem>> GetComboVehicleTypesAsync()
        {
            List<SelectListItem> list = await _context.VehicleTypes.Select(c => new SelectListItem
            {
                Text = c.Type,
                Value = c.Id.ToString()
            })
               .OrderBy(c => c.Text)
               .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un Tipo de vehículo...", Value = "0" });
            return list;
        }
    }
}

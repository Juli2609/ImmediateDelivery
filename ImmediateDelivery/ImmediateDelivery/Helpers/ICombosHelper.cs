using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImmediateDelivery.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboStatesAsync();
        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int StateId);
        Task<IEnumerable<SelectListItem>> GetComboNeighborhoodsAsync(int cityId);

        

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImmediateDelivery.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync();

        Task<IEnumerable<SelectListItem>> GetComboNeighborhodsAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboAddressesAsync(int stateId);

    }
}

using ImmediateDelivery.Common;
using ImmediateDelivery.Models;

namespace ImmediateDelivery.Helpers
{
    public interface IHistoryHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
        Task<Response> CancelOrderAsync(int id);
    }
}

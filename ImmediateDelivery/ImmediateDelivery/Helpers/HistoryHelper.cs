using ImmediateDelivery.Common;
using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Enums;
using ImmediateDelivery.Models;
using Microsoft.EntityFrameworkCore;

namespace ImmediateDelivery.Helpers
{
    public class HistoryHelper : IHistoryHelper
    {
        private readonly DataContext _context;

        public HistoryHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Response> CancelOrderAsync(int id)
        {
            Send send = await _context.Sends
                .Include(s => s.SendDetails)
                .ThenInclude(sd => sd.Package)
                .FirstOrDefaultAsync(s => s.Id == id);

            foreach (SendDetail sendDetail in send.SendDetails)
            {
                Package package = await _context.Packages.FindAsync(sendDetail.Package.Id);
            }

            send.HistoryStatus = HistoryStatus.Cancelado;
            await _context.SaveChangesAsync();
            return new Response { IsSuccess = true };
        }

        public async Task<Response> ProcessOrderAsync(ShowCartViewModel model)
        {
            Response response = await CheckInventoryAsync(model);
            if (!response.IsSuccess)
            {
                return response;
            }

            Send send = new()
            {
                Date = DateTime.UtcNow,
                User = model.User,
                Remarks = model.Remarks,
                SendDetails = new List<SendDetail>(),
                HistoryStatus = HistoryStatus.Nuevo
            };

            foreach (TemporalSend? item in model.TemporalSends)
            {
 
                send.SendDetails.Add(new SendDetail
                {
                    Package = item.Package,
                    Remarks = item.Remarks,
                });

                Package package = await _context.Packages.FindAsync(item.Package.Id);
                if (package != null)
                {
                    _context.Packages.Update(package);
                }

                _context.TemporalSends.Remove(item);
            }

            _context.Sends.Add(send);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<Response> CheckInventoryAsync(ShowCartViewModel model)
        {
            Response response = new() { IsSuccess = true };
            foreach (TemporalSend? item in model.TemporalSends)
            {
                Package package = await _context.Packages.FindAsync(item.Package.Id);
                if (package == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El paquete {item.Package.FullNameRecipient}, no está en el sistema";
                    return response;
                }       
            }
            return response;
        }
    }

}


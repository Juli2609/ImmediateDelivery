using ImmediateDelivery.Common;
using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Enums;
using ImmediateDelivery.Helpers;
using ImmediateDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace ImmediateDelivery.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IHistoryHelper _historyHelper;
        private readonly IUserHelper _userHelper;

        public HistoriesController(DataContext context, IFlashMessage flashMessage, IHistoryHelper historyHelper, IUserHelper userHelper)
        {
            _context = context;
            _flashMessage = flashMessage;
            _historyHelper = historyHelper;
            _userHelper = userHelper;
        }

       
        public async Task<IActionResult> Index()
        {

            return View(await _context.Sends
                .Include(s => s.User)
                .Include(s => s.SendDetails)
                .ThenInclude(sd => sd.Package)
                .ToListAsync());
        }

        [Authorize(Roles = "Messenger")]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends.FindAsync(id);
            if (send == null)
            {
                return NotFound();
            }

            if (send.HistoryStatus != HistoryStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden Confirmar envíos que estén en estado 'nuevo'.");
            }
            else
            {
                send.HistoryStatus = HistoryStatus.Confirmado;
                _context.Sends.Update(send);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del envío ha sido cambiado a 'confirmado'.");
            }

            return RedirectToAction(nameof(DetailsSends), new { Id = send.Id });
        }

        [Authorize(Roles = "Messenger")]
        public async Task<IActionResult> OffCast(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends.FindAsync(id);
            if (send == null)
            {
                return NotFound();
            }
            if (send.HistoryStatus != HistoryStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden Rechazar envíos que estén en estado 'nuevo'.");
            }
            else
            {
                send.HistoryStatus = HistoryStatus.Rechazado;
                _context.Sends.Update(send);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del envío ha sido cambiado a 'rechazado'.");
            }

            return RedirectToAction(nameof(DetailsSends), new { Id = send.Id });
        }


        [Authorize(Roles = "Messenger")]
        public async Task<IActionResult> PickedUp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends.FindAsync(id);
            if (send == null)
            {
                return NotFound();
            }
            if (send.HistoryStatus != HistoryStatus.Confirmado)
            {
                _flashMessage.Danger("Solo se pueden Recoger envíos que estén en estado 'confirmado'.");
            }
            else
            {
                send.HistoryStatus = HistoryStatus.Recogido;
                _context.Sends.Update(send);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del envío ha sido cambiado a 'recogido'.");
            }

            return RedirectToAction(nameof(DetailsSends), new { Id = send.Id });
        }

        [Authorize(Roles = "Messenger")]
        public async Task<IActionResult> InRute(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends.FindAsync(id);
            if (send == null)
            {
                return NotFound();
            }
            if (send.HistoryStatus != HistoryStatus.Recogido)
            {
                _flashMessage.Danger("Solo se pueden estar en Ruta envíos que estén en estado 'recogido'.");
            }
            else
            {
                send.HistoryStatus = HistoryStatus.Recogido;
                _context.Sends.Update(send);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del envío ha sido cambiado a 'En Ruta'.");
            }

            return RedirectToAction(nameof(DetailsSends), new { Id = send.Id });
        }

        [Authorize(Roles = "Messenger")]
        public async Task<IActionResult> Delivered(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends.FindAsync(id);
            if (send == null)
            {
                return NotFound();
            }
            if (send.HistoryStatus != HistoryStatus.En_Ruta)
            {
                _flashMessage.Danger("Solo se pueden ser Entregado envíos que estén en estado 'en_ruta'.");
            }
            else
            {
                send.HistoryStatus = HistoryStatus.Entregado;
                _context.Sends.Update(send);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del envío ha sido cambiado a 'Entregado'.");
            }

            return RedirectToAction(nameof(DetailsSends), new { Id = send.Id });
        }

        [Authorize(Roles = "Messenger")]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends.FindAsync(id);
            if (send == null)
            {
                return NotFound();
            }
            if (send.HistoryStatus != HistoryStatus.Cancelado)
            {
                _flashMessage.Danger("No se puede cancelar un envío que esté en estado 'cancelado'.");
            }
            else
            {
                send.HistoryStatus = HistoryStatus.Cancelado;
                _context.Sends.Update(send);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del envío ha sido cambiado a 'Cancelado'.");
            }

            return RedirectToAction(nameof(DetailsSends), new { Id = send.Id });
        }

        public async Task<IActionResult> DetailsSends(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends
                .Include(s => s.User)
                .Include(s => s.SendDetails)
                .ThenInclude(sd => sd.Package)
                .ThenInclude(p => p.PackageType)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (send == null)
            {
                return NotFound();
            }

            return View(send);
        }



        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Package package = await _context.Packages
        //        .Include(p => p.PackageType)
        //        .FirstOrDefaultAsync(p => p.Id == id);
        //    if (package == null)
        //    {
        //        return NotFound();
        //    }

        //    AddPackageToCartViewModel model = new()
        //    {
        //        Id = package.Id,
        //        PackageType = package.PackageType,
        //        WrapperType = package.WrapperType,
        //        Long = package.Long,
        //        Height = package.Height,
        //        Delicate = package.Delicate,
        //        Contain = package.Contain,
        //        FullNameRecipient = package.FullNameRecipient,
        //        DocRecipient = package.DocRecipient,
        //        PhoneNumber = package.PhoneNumber,
        //        AddressRecipient = package.AddressRecipient,

        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Details(AddPackageToCartViewModel model)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    Package package = await _context.Packages.FindAsync(model.Id);
        //    if (package == null)
        //    {
        //        return NotFound();
        //    }

        //    User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    TemporalSend temporalSend = new()
        //    {
        //        Package = package,
        //        Remarks = model.Remarks,
        //        User = user
        //    };

        //    _context.TemporalSends.Add(temporalSend);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> MyPackages()
        //{

        //    return View(await _context.Sends
        //        .Include(s => s.User)
        //        .Include(s => s.SendDetails)
        //        .ThenInclude(sd => sd.Package)
        //        .Where(s => s.User.UserName == User.Identity.Name)
        //        .ToListAsync());
        //}


        //public async Task<IActionResult> MyDetails(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Send send = await _context.Sends
        //        .Include(s => s.User)
        //        .Include(s => s.SendDetails)
        //        .ThenInclude(s => s.Package)
        //        .ThenInclude(s => s.PackageType)
        //        .FirstOrDefaultAsync(s => s.Id == id);
        //    if (send == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(send);
        //}

        //[Authorize]
        //public async Task<IActionResult> ShowCart()
        //{
        //    User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    List<TemporalSend>? temporalSends = await _context.TemporalSends
        //        .Include(ts => ts.Package)
        //        .ThenInclude(p => p.PackageType)
        //        .Where(ts => ts.User.Id == user.Id)
        //        .ToListAsync();

        //    ShowCartViewModel model = new()
        //    {
        //        User = user,
        //        TemporalSends = temporalSends,
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        //{
        //    User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    model.User = user;
        //    model.TemporalSends = await _context.TemporalSends
        //        .Include(ts => ts.Package)
        //        .ThenInclude(p => p.PackageType)
        //        .Where(ts => ts.User.Id == user.Id)
        //        .ToListAsync();

        //    Response response = await _historyHelper.ProcessOrderAsync(model);
        //    if (response.IsSuccess)
        //    {
        //        return RedirectToAction(nameof(OrderSuccess));
        //    }

        //    ModelState.AddModelError(string.Empty, response.Message);
        //    return View(model);
        //}


        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    TemporalSend temporalSend = await _context.TemporalSends.FindAsync(id);
        //    if (temporalSend == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TemporalSends.Remove(temporalSend);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ShowCart));
        //}

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    TemporalSend temporalSend = await _context.TemporalSends.FindAsync(id);
        //    if (temporalSend == null)
        //    {
        //        return NotFound();
        //    }

        //    EditTemporalSendViewModel model = new()
        //    {
        //        Id = temporalSend.Id,
        //        Remarks = temporalSend.Remarks,
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, EditTemporalSendViewModel model)
        //{
        //    if (id != model.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            TemporalSend temporalSend = await _context.TemporalSends.FindAsync(id);
        //            temporalSend.Remarks = model.Remarks;
        //            _context.Update(temporalSend);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (Exception exception)
        //        {
        //            ModelState.AddModelError(string.Empty, exception.Message);
        //            return View(model);
        //        }

        //        return RedirectToAction(nameof(ShowCart));
        //    }

        //    return View(model);
        //}

        //[Authorize]
        //public IActionResult OrderSuccess()
        //{
        //    return View();
        //}


        //public async Task<IActionResult> Add(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //if (!User.Identity.IsAuthenticated)
        //    //{
        //    //    return RedirectToAction("Login", "Account");
        //    //}

        //    Package package = await _context.Packages.FindAsync(id);
        //    if (package == null)
        //    {
        //        return NotFound();
        //    }

        //    User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    TemporalSend temporalSend = new()
        //    {
        //        Package = package,
        //        User = user
        //    };

        //    _context.TemporalSends.Add(temporalSend);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

       
    }
}
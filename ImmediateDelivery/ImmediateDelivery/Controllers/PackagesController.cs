using ImmediateDelivery.Common;
using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Helpers;
using ImmediateDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static ImmediateDelivery.Helpers.ModalHelper;

namespace ImmediateDelivery.Controllers
{
    public class PackagesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;
        private readonly IUserHelper _userHelper;
        private readonly IHistoryHelper _historyHelper;

        public PackagesController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper,
            IFlashMessage flashMessage, IUserHelper userHelper, IHistoryHelper historyHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
            _userHelper = userHelper;
            _historyHelper = historyHelper;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Packages
                .Include(c => c.PackageType)
                .Include(c => c.User)
                .ThenInclude(u => u.Sends)
                .ThenInclude(s => s.SendDetails)
                //.Where(s => s.User.UserName == User.Identity.Name)
                .ToListAsync());
        }


        [NoDirectAccess]
        public async Task<IActionResult> Create()
        {
            CreatePackageViewModel model = new()
            {
                PackageTypes = await _combosHelper.GetComboPackageTypesAsync(),
            };

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePackageViewModel model)
        {
            if (ModelState.IsValid)
            {
       
                Package package = new()
                {
                    WrapperType = model.WrapperType,
                    Height = model.Height,
                    Long = model.Long,
                    Width = model.Width,
                    Delicate = model.Delicate,
                    Contain = model.Contain,
                    FullNameRecipient = model.FullNameRecipient,
                    DocRecipient = model.DocRecipient,
                    PhoneNumber = model.PhoneNumber,
                    AddressRecipient = model.AddressRecipient,
                    PackageType = await _context.PackageTypes.FindAsync(model.PackageTypeId),
                    //User = await _context.Users.FindAsync(model.UserId)
                };

                

                try
                {
                    _context.Add(package);
                    await _context.SaveChangesAsync();
                    //var NumberGuie = package.Id + 1999999;
                    //_flashMessage.Confirmation("Su número de guía es: " , { "NumberGuie"});
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllPackage", _context.Packages
                       .Include(p => p.PackageType)
                       .ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un paquete con la misma información.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

               
            }

            model.PackageTypes = await _combosHelper.GetComboPackageTypesAsync();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            Package package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            EditPackageViewModel model = new()
            {
                Id = package.Id,
                WrapperType = package.WrapperType,
                Height = package.Height,
                Long = package.Long,
                Width = package.Width,
                Delicate = package.Delicate,
                Contain = package.Contain,
                FullNameRecipient = package.FullNameRecipient,
                DocRecipient = package.DocRecipient,
                PhoneNumber = package.PhoneNumber,
                AddressRecipient = package.AddressRecipient,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreatePackageViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Package package = await _context.Packages.FindAsync(model.Id);
                ///package.WrapperType = model.WrapperType;
                package.Height = model.Height;
                package.Long = model.Long;
                package.Width = model.Width;
              ///  package.Delicate = model.Delicate;
                package.Contain = model.Contain;
                package.FullNameRecipient = model.FullNameRecipient;
                package.DocRecipient = model.DocRecipient;
                package.PhoneNumber = model.PhoneNumber;
                package.AddressRecipient = model.AddressRecipient;
                _context.Update(package);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllPackage", _context.Packages
                    .Include(p => p.PackageType)

                    .ToList())
                });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un paquete con el misma información.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Package package = await _context.Packages
                .Include(p => p.PackageType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyPackages()
        {

            return View(await _context.Sends
                .Include(s => s.User)
                .Include(s => s.SendDetails)
                .ThenInclude(sd => sd.Package)
                .Where(s => s.User.UserName == User.Identity.Name)
                .ToListAsync());
        }

        public async Task<IActionResult> MyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Send send = await _context.Sends
                .Include(s => s.User)
                .Include(s => s.SendDetails)
                .ThenInclude(s => s.Package)
                .ThenInclude(s => s.PackageType)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (send == null)
            {
                return NotFound();
            }

            return View(send);
        }


        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Package package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalSend temporalSend = new()
            {
                Package = package,
                User = user
            };

            _context.TemporalSends.Add(temporalSend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult OrderSuccess()
        {
            return View();
        }


        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<TemporalSend>? temporalSends = await _context.TemporalSends
                .Include(ts => ts.Package)
                .ThenInclude(p => p.PackageType)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            ShowCartViewModel model = new()
            {
                User = user,
                TemporalSends = temporalSends,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.TemporalSends = await _context.TemporalSends
                .Include(ts => ts.Package)
                .ThenInclude(p => p.PackageType)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            Response response = await _historyHelper.ProcessOrderAsync(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(OrderSuccess));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }

        public async Task<IActionResult> DeletePackage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSend temporalSend = await _context.TemporalSends.FindAsync(id);
            if (temporalSend == null)
            {
                return NotFound();
            }

            _context.TemporalSends.Remove(temporalSend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> EditPackage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSend temporalSend = await _context.TemporalSends.FindAsync(id);
            if (temporalSend == null)
            {
                return NotFound();
            }

            EditTemporalSendViewModel model = new()
            {
                Id = temporalSend.Id,
                Remarks = temporalSend.Remarks,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPackage(int id, EditTemporalSendViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TemporalSend temporalSend = await _context.TemporalSends.FindAsync(id);
                    temporalSend.Remarks = model.Remarks;
                    _context.Update(temporalSend);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }

                return RedirectToAction(nameof(ShowCart));
            }

            return View(model);
        }
        public async Task<IActionResult> DetailPackages(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Package package = await _context.Packages
                .Include(p => p.PackageType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            AddPackageToCartViewModel model = new()
            {
                Id = package.Id,
                PackageType = package.PackageType,
                WrapperType = package.WrapperType,
                Long = package.Long,
                Height = package.Height,
                Delicate = package.Delicate,
                Contain = package.Contain,
                FullNameRecipient = package.FullNameRecipient,
                DocRecipient = package.DocRecipient,
                PhoneNumber = package.PhoneNumber,
                AddressRecipient = package.AddressRecipient,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailPackages(AddPackageToCartViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Package package = await _context.Packages.FindAsync(model.Id);
            if (package == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalSend temporalSend = new()
            {
                Package = package,
                Remarks = model.Remarks,
                User = user
            };

            _context.TemporalSends.Add(temporalSend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
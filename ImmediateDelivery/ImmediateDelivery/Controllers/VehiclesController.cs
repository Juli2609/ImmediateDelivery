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
    
    public class VehiclesController : Controller
    {

        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public VehiclesController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper,
            IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicles
                .Include(c => c.VehicleType)
                .ToListAsync());
        }

        [Authorize(Roles = "Messenger")]
        [NoDirectAccess]
        public async Task<IActionResult> Create()
        {
            CreateVehicleViewModel model = new()
            {
                VehicleTypes = await _combosHelper.GetComboVehicleTypesAsync(),
              
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
            
                Vehicle vehicle = new()
                {
                    Plaque = model.Plaque,
                    BrandVehicle = model.BrandVehicle,
                    Color = model.Color,
                    VehicleType = await _context.VehicleTypes.FindAsync(model.VehicleTypeId),
                };


                try
                {
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllVehicles", _context.Vehicles
                       .Include(v => v.VehicleType)
                       .ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un vehículo con la misma placa.");
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

            model.VehicleTypes = await _combosHelper.GetComboVehicleTypesAsync();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }

        [Authorize(Roles = "Messenger")]
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            Vehicle vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            EditVehicleViewModel model = new()
            {
                Id = vehicle.Id,
                Plaque = vehicle.Plaque,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateVehicleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Vehicle vehicle = await _context.Vehicles.FindAsync(model.Id);
                vehicle.Plaque = model.Plaque;
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllVehicles", _context.Vehicles
                    .Include(p => p.VehicleType)

                    .ToList())
                });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un vehículo con la misma placa.");
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

        [Authorize(Roles = "Messenger")]
        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Vehicle vehicle = await _context.Vehicles
                .Include(p => p.VehicleType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vehicle vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }
    }
}
#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Vereyon.Web;
using ImmediateDelivery.Helpers;
using static ImmediateDelivery.Helpers.ModalHelper;

namespace ImmediateDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public StatesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.States
                .Include(s => s.Cities)
                .ThenInclude(c => c.Neighborhoods)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            State state = await _context.States
                .Include(s => s.Cities)
                .ThenInclude(c => c.Neighborhoods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        public async Task<IActionResult> DetailsCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(c => c.State)
                .Include(s => s.Neighborhoods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddNeighborhood(int id)
        {
            City city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            NeighborhoodViewModel model = new()
            {
                CityId = city.Id,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNeighborhood(NeighborhoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Neighborhood neighborhood = new()
                    {
                        City = await _context.Cities.FindAsync(model.CityId),
                        Name = model.Name,
                    };
                    _context.Add(neighborhood);
                    await _context.SaveChangesAsync();
                    City city = await _context.Cities
                            .Include(c => c.Neighborhoods)
                            .FirstOrDefaultAsync(c => c.Id == model.CityId);
                    _flashMessage.Confirmation("Registro Adicionado");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllNeighborhoods", city) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Barrio/Vereda con" +
                            " el mismo nombre en esta Ciudad.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddNeighborhood", model) });

        }

        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> AddCity(int id)
        {
            State state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            CityViewModel model = new()
            {
                StateId = state.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        Neighborhoods = new List<Neighborhood>(),
                        State = await _context.States.FindAsync(model.StateId),
                        Name = model.Name,

                    };
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    State state = await _context.States
                        .Include(s => s.Cities)
                        .ThenInclude(c => c.Neighborhoods)
                        .FirstOrDefaultAsync(c => c.Id == model.StateId);
                    _flashMessage.Info("Registro creado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCities", state) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Ciudad con" +
                            " el mismo nombre en este Departamento.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddCity", model) });

        }

        [NoDirectAccess]
        public async Task<IActionResult> EditCity(int id)
        {
            City city = await _context.Cities
                .Include(c => c.State)
                /// .ThenInclude(a => a.City)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            CityViewModel model = new()
            {
                StateId = city.State.Id,
                Id = city.Id,
                Name = city.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, CityViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(city);
                    State state = await _context.States
                         .Include(s => s.Cities)
                         .ThenInclude(c => c.Neighborhoods)
                         .FirstOrDefaultAsync(c => c.Id == model.StateId);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro Actualizado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCities", state) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Ciudad con" +
                            " el mismo nombre en este Departamento.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditCity", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditNeighborhood(int id)
        {
            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (neighborhood == null)
            {
                return NotFound();
            }

            NeighborhoodViewModel model = new()
            {
                CityId = neighborhood.City.Id,
                Id = neighborhood.Id,
                Name = neighborhood.Name,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNeighborhood(int id, NeighborhoodViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Neighborhood neighborhood = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(neighborhood);
                    await _context.SaveChangesAsync();
                    City city = await _context.Cities
                        .Include(c => c.Neighborhoods)
                        .FirstOrDefaultAsync(c => c.Id == model.CityId);
                    _flashMessage.Confirmation("Registro actualizado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllNeighborhoods", city) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un barrio/vereda con el mismo" +
                            " nombre en esta Ciudad.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditNeighborhood", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteNeighborhood(int id)
        {
            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            try
            {
                _context.Neighborhoods.Remove(neighborhood);
                await _context.SaveChangesAsync();
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el barrio/vereda porque tiene registros relacionados.");
            }

            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(DetailsCity), new { id = neighborhood.City.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteCity(int id)
        {
            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            try
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la ciudad porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Details), new { id = city.State.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {

            State state = await _context.States.FirstOrDefaultAsync(c => c.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            try
            {
                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el departamento porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new State());
            }
            else
            {
                State state = await _context.States.FindAsync(id);
                if (state == null)
                {
                    return NotFound();
                }

                return View(state);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, State state)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _context.Add(state);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _context.Update(state);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(
                            this,
                            "_ViewAll",
                            _context.States
                                .Include(c => c.Cities)
                                .ThenInclude(s => s.Neighborhoods)
                                .ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", state) });
        }
    }
}



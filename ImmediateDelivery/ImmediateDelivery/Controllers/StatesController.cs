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

namespace ImmediateDelivery.Controllers
{
   [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.States
                .Include(s => s.Cities)
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
        public async Task<IActionResult> DetailsNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
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
        
        public IActionResult Create()
        {
            State state = new() { Cities = new List<City>() };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(State state)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un departamento con el mismo nombre.");
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
            return View(state);

        }

        public async Task<IActionResult> AddNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsCity), new { Id = model.CityId });
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
            return View(model);

        }
         [HttpGet]
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsNeighborhood), new { Id = model.StateId });
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

            return View(model);
        }

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.StateId });
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
            return View(model);
        }

        public async Task<IActionResult> EditNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsCity), new { Id = model.CityId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un barrio con el mismo" +
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
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            State state = await _context.States
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Departamento con el mismo nombre.");
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
            return View(state);
        }

        public async Task<IActionResult> DeleteNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }


        [HttpPost, ActionName("DeleteNeighborhood")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNeighborhoodConfirmed(int id)
        {
            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .ThenInclude(c => c.State)
                .FirstOrDefaultAsync(n => n.Id == id);
            _context.Neighborhoods.Remove(neighborhood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsCity), new {Id = neighborhood.City.Id});
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            State state = await _context.States
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            State state = await _context.States.FindAsync(id);
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }


        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCityConfirmed(int id)
        {
            City city = await _context.Cities
                .Include(a => a.State)
                .FirstOrDefaultAsync(a => a.Id == id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {Id = city.State.Id});
        }
    }

    
    }





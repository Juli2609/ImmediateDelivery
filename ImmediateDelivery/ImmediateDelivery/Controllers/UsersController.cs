using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Enums;
using ImmediateDelivery.Helpers;
using ImmediateDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImmediateDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;

        public UsersController(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper, ICombosHelper combosHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(u => u.Neighborhood)
                .ThenInclude(n => n.City)
                .ThenInclude(c => c.State)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddUserViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                Cities = await _combosHelper.GetComboCitiesAsync(0),
                Neighborhoods = await _combosHelper.GetComboNeighborhoodsAsync(0),
                States = await _combosHelper.GetComboStatesAsync(),
                UserType = UserType.Admin,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                model.ImageId = imageId;
                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                    model.Neighborhoods = await _combosHelper.GetComboNeighborhoodsAsync(model.CityId);
                    model.States = await _combosHelper.GetComboStatesAsync();
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }

            model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
            model.Neighborhoods = await _combosHelper.GetComboNeighborhoodsAsync(model.CityId);
            model.States = await _combosHelper.GetComboStatesAsync();
            return View(model);
        }
        public JsonResult GetCities(int stateId)
        {
            State state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id == stateId);
            if (state == null)
            {
                return null;
            }

            return Json(state.Cities.OrderBy(c => c.Name));
        }

        public JsonResult GetNeighborhoods(int cityId)
        {
            City city = _context.Cities
                .Include(c => c.Neighborhoods)
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return null;
            }

            return Json(city.Neighborhoods.OrderBy(d => d.Name));
        }
    }
}


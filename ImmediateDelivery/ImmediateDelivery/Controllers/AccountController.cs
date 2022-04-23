using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Enums;
using ImmediateDelivery.Helpers;
using ImmediateDelivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImmediateDelivery.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;

        public AccountController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
        public async Task<IActionResult> Register()
        {
            AddUserViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                Cities = await _combosHelper.GetComboCitiesAsync(0),
                Neighborhoods = await _combosHelper.GetComboNeighborhoodsAsync(0),
                States = await _combosHelper.GetComboStatesAsync(),
                UserType = UserType.User,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
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

                LoginViewModel loginViewModel = new()
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };

                var result2 = await _userHelper.LoginAsync(loginViewModel);

                if (result2.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
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

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new()
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Neighborhoods = await _combosHelper.GetComboNeighborhoodsAsync(user.Neighborhood.City.Id),
                NeighborhoodId = user.Neighborhood.Id,
                States = await _combosHelper.GetComboStatesAsync(),
                StateId = user.Neighborhood.City.State.Id,
                CityId = user.Neighborhood.City.Id,
                Cities = await _combosHelper.GetComboCitiesAsync(user.Neighborhood.City.State.Id),
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.Neighborhood = await _context.Neighborhoods.FindAsync(model.NeighborhoodId);
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
            model.Neighborhoods = await _combosHelper.GetComboNeighborhoodsAsync(model.CityId);
            model.States = await _combosHelper.GetComboStatesAsync();
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña diferente.");
                    return View(model);
                }

                User? user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult? result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,"La contraseña actual es incorrecta");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado");
                }
            }

            return View(model);
        }


    }

}


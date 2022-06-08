using ImmediateDelivery.Data;
using ImmediateDelivery.Data.Entities;
using ImmediateDelivery.Enums;
using ImmediateDelivery.Helpers;
using ImmediateDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ImmediateDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext, IUserHelper userHelper)
        {
            _logger = logger;
            _context = dataContext;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Messengers(AddVehicleViewModel addVehicle)
        {

            List<User> messengers = await _context.Users
                .Include(p => p.Vehicles)
                .ThenInclude(v => v.VehicleType)
                .Where(u => u.UserType == UserType.Messenger)
                .OrderBy(p => p.UserName)
                .ToListAsync();

            HomeViewModel model = new()
            {
                Messengers = messengers,

            };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }


    }
}
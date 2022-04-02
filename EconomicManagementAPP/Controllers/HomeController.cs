using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IRepositorieUsers repositorieUsers;

        public HomeController(ILogger<HomeController> logger,IRepositorieUsers repositorieUsers, IRepositorieAccounts repositorieAccounts, IRepositorieAccountTypes repositorieAccountTypes)
        {
            _logger = logger;
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.repositorieUsers = repositorieUsers;
        }

        public async Task<IActionResult> Index()
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            int userId = UsersController.valorSesion.Id;
            AccountAndAccountTypes accountAndAccountTypes = new()
            {
                UserName = UsersController.valorSesion.Email,
                AccountTypes = await repositorieAccountTypes.GetAccountsTypes(userId),
                Accounts = await repositorieAccounts.GetAccounts(userId)
            };
            //var  auth = repositorieUsers.Auth(HttpContext.Session);
            

            return !accountAndAccountTypes.AccountTypes.Any() ? RedirectToAction("Create", "AccountTypes") : View(accountAndAccountTypes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Interfaz para error de no encontrar el id
        public IActionResult  Overrride  ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
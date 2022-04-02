using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IRepositorieUsers repositorieUsers;

        public AccountsController(IRepositorieAccounts repositorieAccounts, IRepositorieAccountTypes repositorieAccountTypes, IRepositorieUsers repositorieUsers)
        {
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.repositorieUsers = repositorieUsers;
        }

        // Creamos index para ejecutar la interfaz
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> AddFounds(int atId, int id)
        //{
        //    var accountType = await repositorieAccountTypes.GetAccountById(atId);
        //    if (accountType is null)
        //    {
        //        return RedirectToAction("NotFound", "Home");
        //    }
        //    var account = await repositorieAccounts.GetAccountById(id);
        //    if (account is null)
        //    {
        //        return RedirectToAction("NotFound", "Home");
        //    }
        //    return View(account);
        //}


        public async Task<IActionResult> Create(int id)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            int userId = UsersController.valorSesion.Id;
            var accountType = await repositorieAccountTypes.GetAccountById(id, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            Accounts account = new()
            {
                AccountTypeId = accountType.Id
            };

            return View(account);
        }
       

        [HttpPost]
        public async Task<IActionResult> Create(Accounts accounts)
        {
            if (!ModelState.IsValid)
            {
                return View(accounts);
            }


            // Validamos si ya existe antes de registrar
            var accountExist =
               await repositorieAccounts.ExistingAccount(accounts.Name, accounts.AccountTypeId);

            if (accountExist)
            {
                // AddModelError ya viene predefinido en .net
                // nameOf es el tipo del campo
                ModelState.AddModelError(nameof(accounts.Name),
                    $"The account {accounts.Name} already exist.");

                return View(accounts);
            }

            accounts.Money = (accounts.Money is null) ? "0" : accounts.Money.ToString().Replace(".", ",");

            if (!Decimal.TryParse(accounts.Money.ToString(), out Decimal numBalance))
            {
                ModelState.AddModelError(nameof(accounts.Money),
                    $"The value {accounts.Money} is not valid in controller.");
            }
            accounts.DbStatus = true;
            accounts.Balance = numBalance;

            await repositorieAccounts.Create(accounts);
            // Redireccionamos a la lista
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        //public async Task<ActionResult> AddFounds()
        //{
        //    return RedirectToAction("Index", "Home");
        //}

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerificaryAccounts(string Name, int AccountTypeId)
        {
            var accountExist = await repositorieAccounts.ExistingAccount(Name, AccountTypeId);

            if (accountExist)
            {
                // permite acciones directas entre front y back
                return Json($"The account {Name} already exist");
            }

            return Json(true);
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id, int atId)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var account = await repositorieAccounts.GetAccountById(id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            account.AccountTypeId = atId;
            Console.WriteLine("lleva al post"+account.AccountTypeId);
            return View(account);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(Accounts accounts)
        {
            if (!ModelState.IsValid)
            {
                return View(accounts);
            }
            Console.WriteLine("este es el id de cuenta: " + accounts.Id + "este es el id tipos" + accounts.AccountTypeId);
            var account = await repositorieAccounts.GetAccountById(accounts.Id);
            Console.WriteLine("este es el id de cuenta: "+account.Id+ "este es el id tipo" + account.AccountTypeId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccounts.Modify(accounts);// el que llega
            return RedirectToAction("Index", "Home");

        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int atId)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var account = await repositorieAccounts.GetAccountById(id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            account.NumberTransaction = await repositorieAccounts.GetNumberTransaction(id);
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            
            var account = await repositorieAccounts.GetAccountById(id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var accountTransaction = await repositorieAccounts.ExistingAccountTransaction(account.Id);
            if(accountTransaction == false)
            {
                await repositorieAccounts.Delete(id);
                return RedirectToAction("Index", "Home");
            }
            await repositorieAccounts.DeleteModify(id);
            return RedirectToAction("Index", "Home");
        }



    }
}

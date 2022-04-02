using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class TransactionsController: Controller
    {
        public readonly IRepositorieTransactions repositorieTransactions;
        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        public readonly IRepositorieAccounts repositorieAccounts;
        public readonly IRepositorieCategories repositorieCategories;
        public readonly IRepositorieUsers repositorieUsers;
        public TransactionsController(IRepositorieTransactions repositorieTransactions,IRepositorieOperationTypes repositorieOperationTypes,  IRepositorieAccounts repositorieAccounts, IRepositorieCategories repositorieCategories, IRepositorieUsers repositorieUsers)
        {
            this.repositorieTransactions = repositorieTransactions;
            this.repositorieOperationTypes = repositorieOperationTypes;
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieCategories = repositorieCategories;
            this.repositorieUsers = repositorieUsers;

        }

        // Creamos index para ejecutar la interfaz
        public async Task<IActionResult> Index(int id)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var transactions = await repositorieTransactions.GetTransactions(id);
            return View(transactions);
        }

        public async Task<IActionResult> Create(int id)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var accounts = await repositorieAccounts.GetAccountById(id);
            if(accounts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            //ViewResult result = (ViewResult)TempData["User"];
            //result.TempData = TempData["User"];
            //Users user = (Users)TempData["User"];
            //string valor = TempData["Id"].ToString();
            //int valor2 = Int16.Parse(valor);

            //object user = (object)TempData["IdAutentication"];

            //Console.WriteLine("hola sol el id "+user);


            //Users users = UsersController.valorSesion;
        

          
            int userId = UsersController.valorSesion.Id;

            Transactions transactions = new()
            {
                AccountId = accounts.Id,
                AccountName = accounts.Name,
                CategoryList = await repositorieCategories.GetCategories(userId)
            };
            //transactions.AccountId = id;
            //Console.WriteLine("hola " + transactions.AccountId);
            return View(transactions);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return View(transactions);
            }
            Console.WriteLine("hola "+transactions.CategoryId);
            //Console.WriteLine("hola soy lista de categorias "+ transactions.CategoryList.ToList());
            var accounts = await repositorieAccounts.GetAccountById(transactions.AccountId);

            if (accounts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

           

            transactions.UserId = UsersController.valorSesion.Id;
            transactions.TransactionDate = DateTime.Now;
                       
            await repositorieTransactions.Create(transactions);
            var operationType = await repositorieOperationTypes.GetOperationTypeByCategoryId(transactions.CategoryId, transactions.UserId);
            if (transactions.Id > 0)
            {
                if(operationType.Equals("Income"))
                {
                    accounts.Balance += transactions.Total;
                }else if (operationType.Equals("Expenditure"))
                {
                    accounts.Balance -= transactions.Total;
                }
                await repositorieAccounts.Modify(accounts);
            }
            else
            {
                ModelState.AddModelError(nameof(transactions.Total),
                    $"No se pudo realizar la transaccion");

                return View(transactions);
            }
            //Console.WriteLine(Id);
            // Redireccionamos a la lista
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "Transactions", new { id = transactions.AccountId});
        }

    }
}

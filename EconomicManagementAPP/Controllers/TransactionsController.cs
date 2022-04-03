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
            var transactions = await repositorieTransactions.GetTransactions(id,UsersController.valorSesion.Id);
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
        

          
            int userId = UsersController.valorSesion.Id;

            Transactions transactions = new()
            {
                AccountId = accounts.Id,
                AccountName = accounts.Name,
                CategoryList = await repositorieCategories.GetCategories(userId)
            };

            return View(transactions);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                transactions.CategoryList = await repositorieCategories.GetCategories(UsersController.valorSesion.Id);
                return View(transactions);
            }

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
            return RedirectToAction("Index", "Transactions", new { id = transactions.AccountId});
        }

    }
}

using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositorieCategories repositorieCategories;

        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        private readonly IRepositorieUsers repositorieUsers;

        public CategoriesController(IRepositorieCategories repositorieCategories, IRepositorieOperationTypes repositorieOperationTypes, IRepositorieUsers repositorieUsers)
        {
            this.repositorieCategories = repositorieCategories;
            this.repositorieOperationTypes = repositorieOperationTypes;
            this.repositorieUsers = repositorieUsers;
        }


        public async Task<IActionResult> Index()
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }

            var userId = UsersController.valorSesion.Id;
            var categories = await repositorieCategories.GetCategories(userId);
            if (categories is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            //Categories category = new();
            //category.OperationTypeId = operation.Id;
            ////Console.WriteLine(category.OperationTypeId);
            //var categories = await repositorieCategories.GetCategories(userId);
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            Categories category = new()
            {
                OperationTypesList = await repositorieOperationTypes.GetOperation()
            };

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Categories categories)
        {
            if (!ModelState.IsValid)
            {
                categories.OperationTypesList = await repositorieOperationTypes.GetOperation();
                return View(categories);
            }
            categories.UserId = UsersController.valorSesion.Id;
            var categorieExist =
               await repositorieCategories.ExistingCategories(categories.Name, categories.OperationTypeId, categories.UserId);

            if (categorieExist)
            {
                ModelState.AddModelError(nameof(categories.Name),
                    $"The category {categories.Name} already exist.");
                categories.OperationTypesList = await repositorieOperationTypes.GetOperation();
                return View(categories);
            }
            await repositorieCategories.Create(categories);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Modify( int id)
        {
            var userId = UsersController.valorSesion.Id;
            var category = await repositorieCategories.GetCategorieByIds(id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(Categories categories)
        {
            if (!ModelState.IsValid)
            {
                return View(categories);
            }
            var categorie = await repositorieCategories.GetCategorieByIds(categories.Id, UsersController.valorSesion.Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Modify(categories);// el que llega
            return RedirectToAction("Index", "Categories");

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = UsersController.valorSesion.Id;
            var categorie = await repositorieCategories.GetCategorieByIds(id, userId);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            
            return View(categorie);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = UsersController.valorSesion.Id;
            var categorie = await repositorieCategories.GetCategorieByIds(id, userId);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
           
            await repositorieCategories.Delete(id);
            return RedirectToAction("Index", "Categories");
        }

    }
}

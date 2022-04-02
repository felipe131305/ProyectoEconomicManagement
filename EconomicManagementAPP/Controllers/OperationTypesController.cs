using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Controllers
{
    public class OperationTypesController : Controller
    {
        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        public OperationTypesController(IRepositorieOperationTypes repositorieOperationTypes)
        {
            this.repositorieOperationTypes = repositorieOperationTypes;

        }
        public async Task<IActionResult> Index()
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var operationTypes = await repositorieOperationTypes.GetOperation();
            return View(operationTypes);
        }

        public IActionResult Create()
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> VerifyOperationTypes(string Description)
        {
            var operationExist = await repositorieOperationTypes.Exist(Description);

            if (operationExist)
            {
                return Json($"The Operation Types with Description {operationExist} is already registered");
            }

            return Json(true);

        }
        [HttpPost]
       
        public async Task<IActionResult> Create(OperationTypes operationTypes)
        {
            if (!ModelState.IsValid)
            {
                return View(operationTypes);
            } // Validamos si ya existe antes de registrar
            var operationTypesExist =
            await repositorieOperationTypes.Exist(operationTypes.Description); if (operationTypesExist)
            {
                // AddModelError ya viene predefinido en .net
                // nameOf es el tipo del campo
                ModelState.AddModelError(nameof(operationTypes.Description),
                $"The description {operationTypes.Description} already exist."); return View(operationTypes);
            }
            await repositorieOperationTypes.Create(operationTypes);
            // Redireccionamos a la lista
            return RedirectToAction("Index");
            // return RedirectToAction("Create", "Accounts");
        }



        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var operationType = await repositorieOperationTypes.GetOperationById(id);

            if (operationType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(operationType);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(OperationTypes operationTypes)
        {

            if (!ModelState.IsValid)
            {
                return View(operationTypes);
            }

            var operationType = await repositorieOperationTypes.GetOperationById(operationTypes.Id);

            if (operationType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }


            await repositorieOperationTypes.Modify(operationTypes);// el que llega
            return RedirectToAction("Index");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }

            var operationType = await repositorieOperationTypes.GetOperationById(id);

            if (operationType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(operationType);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOperationTypes(int id)
        {

            var operationTypes = await repositorieOperationTypes.GetOperationById(id);

            if (operationTypes is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieOperationTypes.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

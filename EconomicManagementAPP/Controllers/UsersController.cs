using EconomicManagementAPP.Interface;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Session;
//using Microsoft.AspNetCore.Session;

namespace EconomicManagementAPP.Controllers
{
    public class UsersController: Controller
    {

        private readonly IRepositorieUsers repositorieUsers;

        internal static Users valorSesion;
        public UsersController(IRepositorieUsers repositorieUsers)
        {
            this.repositorieUsers = repositorieUsers;
        }

        

        public IActionResult Create()
        {
            return View();
        }
        
        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            Console.WriteLine("entro");
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var result = await repositorieUsers.Login(loginViewModel.Email, loginViewModel.Password);

            if (result is null)
            {
                ModelState.AddModelError(string.Empty, "Wrong Email or Password");
                return View(loginViewModel);
            }
            else
            {
            
                valorSesion = result;
                return RedirectToAction("Index", "Home"); 
            }

        }

        [HttpGet]
        public async Task<IActionResult> VerifyUser(string email)
        {
            var userExist = await repositorieUsers.ExistingUser(email);

            if (userExist)
            {
                return Json($"The user with email {email} is already registered");
            }

            return Json(true);

        }
        [HttpPost]
        public async Task<IActionResult> Create(Users users)
        {
            if(!ModelState.IsValid)
            {
                return View(users);
            }

            var usersExist = await repositorieUsers.ExistingUser(users.Email);
            if (usersExist)
            {
                ModelState.AddModelError(nameof(users.Email),
                    $"The email {users.Email} already exist.");

                return View(users);
            }
            users.DbStatus = true;
            //Console.WriteLine("soy el id de users " + users.Id);
            await repositorieUsers.Create(users);
            //Console.WriteLine("soy el id de users "+users.Id);
            if(users.Id.ToString() is null)
            {
                ModelState.AddModelError(nameof(users.Email),
                    $"Failed to create the user {users.Email}.");

                return View(users);
            }
            //TempData["IdAutentication"] = users.Id;
            //session["idUser"] = users.Id;
            valorSesion = users;
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyEmail)))
            //{
            //    HttpContext.Session.SetInt32(SessionKeyId, users.Id);
            //    HttpContext.Session.SetString(SessionKeyEmail, users.Email);
            //}

            //await signInManager.SignInAsync(users, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify()
        {
            if (UsersController.valorSesion is null)
            {
                return RedirectToAction("Login", "Users");
            }
            var user = await repositorieUsers.GetUserById(UsersController.valorSesion.Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(Users users)
        {

            if (!ModelState.IsValid)
            {
                return View(users);
            }

            var user = await repositorieUsers.GetUserById(users.Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            if(users.Password is null)
            {
           
                users.Password = user.Password;
            }

            await repositorieUsers.Modify(users);// el que llega
            return RedirectToAction("Index", "Home");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
         
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
           
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUsers.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult LogOut()
        {
            
            valorSesion = null;
            



            return RedirectToAction("Login");
        }
    }
}

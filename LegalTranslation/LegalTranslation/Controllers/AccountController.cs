using LegalTranslation.Data;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using LegalTranslation.Repository;
using LegalTranslation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LegalTranslation.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountRepository _accountRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAccountRepository accountRepository)
        {
            _signInManager = signInManager;
            _accountRepository = accountRepository;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);

                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        TempData["AlertMessageRed"] = "Нещо се обърка!";
                        return View(loginVM);
                    }
                }
                else
                {
                    TempData["AlertMessageRed"] = "Невалидна парола!";
                    return View(loginVM);
                }
            }
            else
            {
                TempData["AlertMessageRed"] = "Този e-mail не беше намерен!";
                return View(loginVM);
            }

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult ForgottenPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgottenPassword(string email)
        {
            
            if (!ModelState.IsValid)
            {
                TempData["AlertMessageRed"] = "Нещо се обърка!";
                return View();
            }

            if (_accountRepository.CheckEmailCategory(email))
            {
                TempData["AlertMessageGreen"] = "Администраторът беше уведомен!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["AlertMessageRed"] = "Нещо се обърка!";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}

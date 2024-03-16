using LegalTranslation.Data;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using LegalTranslation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using LegalTranslation.Helpers;

namespace LegalTranslation.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IAdminRepository adminRepository, ILanguageRepository languageRepository,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _adminRepository = adminRepository;
            _languageRepository = languageRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Request> requests = await _adminRepository.GetAll();
            IEnumerable<Request> orderedRequests = requests.OrderByDescending(r => r.Id).ToList();

            return View(orderedRequests);
        }

        public async Task<IActionResult> PendingRequests()
        {
            IEnumerable<Request> requests = await _adminRepository.GetAllPending();
            IEnumerable<Request> orderedRequests = requests.OrderByDescending(r => r.Id).ToList();

            return View(orderedRequests);
        }

        public async Task<IActionResult> CompletedRequests()
        {
            IEnumerable<Request> requests = await _adminRepository.GetAllCompleted();
            IEnumerable<Request> orderedRequests = requests.OrderByDescending(r => r.Id).ToList();

            return View(orderedRequests);
        }

        public async Task<IActionResult> Details(int id)
        {
            DetailsRequestViewModel request = await _adminRepository.GetVByIdAsync(id);
            return View(request);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> LanguagesEdit()
        {
            IEnumerable<Language> languages = await _languageRepository.GetAll();
            return View(languages);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> LanguagesEdit(List<int> languagesIds)
        {
            _languageRepository.UpdateAll(languagesIds);

            TempData["AlertMessageGreen"] = $"Успешно запазени промени по езиците!";
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddLanguage()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddLanguage(Language language)
        {
            language.IsActive = true;

            if (ModelState.IsValid)
            {
                _languageRepository.Add(language);

                TempData["AlertMessageGreen"] = $"Успешно добавен нов език - {language.Name}, който е активен!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessageRed"] = $"Не добавихте нов език!";

                return RedirectToAction("Index");
            }


        }

        public async Task<IActionResult> UpdateRequestPending(int id)
        {
            if (ModelState.IsValid)
            {
                _adminRepository.UpdateByIdPending(id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateRequestReady(int id)
        {
            if (ModelState.IsValid)
            {
                _adminRepository.UpdateByIdReady(id);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddWorker()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddWorker(LoginViewModel register)
        {
            if (PassowrdChecker.IsPasswordValid(register.Password))
            {
                var newUser = await _userManager.FindByEmailAsync(register.Email);
                if (newUser == null)
                {
                    var newUserToAdd = new AppUser()
                    {
                        UserName = register.Email,
                        Email = register.Email,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(newUserToAdd, register.Password);

                    await _userManager.AddToRoleAsync(newUserToAdd, UserRoles.Worker);


                    TempData["AlertMessageGreen"] = $"Успешно добавен нов worker - {register.Email}!";
                    return RedirectToAction("Index");
                }

                TempData["AlertMessageRed"] = "Worker с този e-mail вече съществува!";
                return View(register);
            }
            else
            {
                TempData["AlertMessageRed"] = "Минимум 6 знака - поне 1 цифра, поне 1 главна буква, поне 1 малка буква, поне един специален символ!";
                return View(register);
            }


        }







        public async Task<IActionResult> ChangePassword()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                Id = userId
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordVM)
        {
            if (ModelState.IsValid)
            {
                if (_adminRepository.CheckPasswords(changePasswordVM).Check)
                {
                    var pass = await _adminRepository.ChangePassword(changePasswordVM);

                    if (pass.Succeeded)
                    {
                        TempData["AlertMessageGreen"] = "Паролата беше подновена!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["AlertMessageRed"] = "Паролата не беше променена!";
                    }
                }
                else
                {
                    TempData["AlertMessageRed"] = _adminRepository.CheckPasswords(changePasswordVM).Message +
                        " Минимум 6 знака - поне 1 цифра, поне 1 главна буква, поне 1 малка буква, поне един специален символ!";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                TempData["AlertMessageRed"] = "Нещо се обърка";
            }

            return RedirectToAction("Index");
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeEmail()
        {
            var response = _adminRepository.GetAdminData();

            ChangeEmailViewModel model = new ChangeEmailViewModel()
            {
                Id = response.Id
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel changeEmailVM)
        {
            if (ModelState.IsValid)
            {
                var user = _adminRepository.GetAdminData();
                var passwordCheck = await _userManager.CheckPasswordAsync(user, changeEmailVM.Password);

                if (passwordCheck)
                {
                    var pass = await _adminRepository.ChangeAdminEmail(changeEmailVM);

                    if (pass.Succeeded)
                    {
                        TempData["AlertMessageGreen"] = "E-mail беше подновен!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["AlertMessageRed"] = "E-mail не беше променен!";
                    }
                }
                else
                {
                    TempData["AlertMessageRed"] = "Паролата е грешна!";
                    return View(changeEmailVM);
                }
            }
            else
            {
                TempData["AlertMessageRed"] = "Нещо се обърка";
            }

            return RedirectToAction("Index");
        }










        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllWorkers()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var item in _userManager.Users.ToList())
            {
                UserViewModel user = new UserViewModel();
                user.Id = item.Id;
                user.Email = item.Email;
                if (await _userManager.IsInRoleAsync(item, "admin"))
                {
                    user.Role = "admin";
                }
                else
                {
                    user.Role = "worker";
                }
                users.Add(user);
            }

            return View(users);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(id);
            string name = user.Email;

            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            // Delete the user
            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                // Remove the user from all roles
                var resul1t = await _userManager.RemoveFromRolesAsync(user, userRoles);


                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // User deletion successful
                    TempData["AlertMessageGreen"] = $"Успешно премахнат {name}!";
                    return RedirectToAction("Index"); // Redirect to a success page or another action
                }
            }

            return RedirectToAction("Index");
        }



        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeCompanyEmail()
        {
            ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM = new ChangeAdminOrCompanyEmailViewModel();
            changeAOCEmailVM.Name = _adminRepository.GetCompanyEmail();
            return View(changeAOCEmailVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeCompanyEmail(ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertMessageRed"] = "Нещо се обърка";
                return View(changeAOCEmailVM);
            }

            var user = _adminRepository.GetAdminData();
            var passwordCheck = await _userManager.CheckPasswordAsync(user, changeAOCEmailVM.ConfirmationPassword);

            if (passwordCheck)
            {
                var result = _adminRepository.UpdateCompanyEmail(changeAOCEmailVM);

                if (result.Check)
                {
                    TempData["AlertMessageGreen"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessageRed"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["AlertMessageRed"] = "Паролата е грешна!";
                return View(changeAOCEmailVM);
            }

        }







        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeAdminEmail()
        {
            ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM = new ChangeAdminOrCompanyEmailViewModel();
            changeAOCEmailVM.Name = _adminRepository.GetAdminEmail();
            return View(changeAOCEmailVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeAdminEmail(ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertMessageRed"] = "Нещо се обърка";
                return View(changeAOCEmailVM);
            }

            var user = _adminRepository.GetAdminData();
            var passwordCheck = await _userManager.CheckPasswordAsync(user, changeAOCEmailVM.ConfirmationPassword);

            if (passwordCheck)
            {
                var result = _adminRepository.UpdateAdminEmail(changeAOCEmailVM);

                if (result.Check)
                {
                    TempData["AlertMessageGreen"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessageRed"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["AlertMessageRed"] = "Паролата е грешна!";
                return View(changeAOCEmailVM);
            }
        }



    }
}
using LegalTranslation.Data;
using LegalTranslation.Helpers;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using LegalTranslation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LegalTranslation.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRepository(AppDbContext context,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public bool Add(Request request)
        {
            _context.Add(request);
            return Save();
        }



        public CustomErrorMessage UpdateAdminEmail(ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM)
        {
            CustomErrorMessage result = new CustomErrorMessage(true, "Успешно променен admin E-mail!");
            var profile = _context.Emails.FirstOrDefault(admin => admin.IsAdmin == true);


            if (profile != null)
            {
                profile.Name = changeAOCEmailVM.Name;
                profile.Password = changeAOCEmailVM.Password;

                _context.SaveChanges();

                return result;
            }
            result.Check = false;
            result.Message = "Нещо се обърка";
            return result;
        }

        public CustomErrorMessage UpdateCompanyEmail(ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM)
        {
            CustomErrorMessage result = new CustomErrorMessage(true, "Успешно променен фирмен E-mail!");
            var profile = _context.Emails.FirstOrDefault(admin => admin.IsAdmin == false);


            if (profile != null)
            {
                profile.Name = changeAOCEmailVM.Name;

                _context.SaveChanges();

                return result;
            }
            result.Check = false;
            result.Message = "Нещо се обърка";
            return result;
        }

        public string GetCompanyEmail()
        {
            return _context.Emails.FirstOrDefault(admin => admin.IsAdmin == false).Name;
        }

        public string GetAdminEmail()
        {
            return _context.Emails.FirstOrDefault(admin => admin.IsAdmin == true).Name;
        }







        public async Task<IdentityResult> ChangeAdminEmail(ChangeEmailViewModel changeEmailVM)
        {
            var user = await _userManager.FindByIdAsync(changeEmailVM.Id);

            if (GetAdminData().Email == changeEmailVM.OldEmail)
            {
                if (user != null)
                {
                    user.Email = changeEmailVM.NewEmail;
                    user.UserName = changeEmailVM.NewEmail;

                    var result = await _userManager.UpdateAsync(user);

                    return result;
                }
            }

            return IdentityResult.Failed(new IdentityError { Description = "Your failure message here" });
        }








        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel changePasswordVM)
        {
            var user = await _userManager.FindByIdAsync(changePasswordVM.Id);

            if (user != null)
            {
                return await _userManager.ChangePasswordAsync(user, changePasswordVM.OldPassword, changePasswordVM.NewPassword);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Нещо се обърка!" });
        }







        public async Task<IdentityResult> ChangeAdminForgottenPassword(string id, string password)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<AppUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, password);

                var result = await _userManager.UpdateAsync(user);

                return result;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Your failure message here" });
        }






        public CustomErrorMessage CheckPasswords(ChangePasswordViewModel changePasswordVM)
        {
            CustomErrorMessage result = new CustomErrorMessage(true, null);


            if (!PassowrdChecker.IsPasswordValid(changePasswordVM.OldPassword))
            {
                result.Check = false;
                result.Message = "Невалидна стара парола!";
            }
            if (!PassowrdChecker.IsPasswordValid(changePasswordVM.NewPassword))
            {
                result.Check = false;
                result.Message = "Невалидна нова парола!";
            }
            if (changePasswordVM.OldPassword == changePasswordVM.NewPassword)
            {
                result.Check = false;
                result.Message = "Паролите са еднакви!";
            }
            if (changePasswordVM.NewPassword != changePasswordVM.NewPasswordConfirm)
            {
                result.Check = false;
                result.Message = "Потвърдената парола не е като новата парола!";
            }

            return result;
        }


        public AppUser GetAdminData()
        {
            var a = _userManager.GetUsersInRoleAsync("admin").Result;

            return a.FirstOrDefault();
        }

        public bool Delete(Request request)
        {
            _context.Remove(request);
            return Save();
        }

        public async Task<IEnumerable<Request>> GetAll()
        {
            IEnumerable<Request> result = await _context.Requests.ToListAsync();

            foreach (Request request in result)
            {
                request.FromLanguage = await _context.Languages.FirstOrDefaultAsync(n => n.Id == request.FromLanguageId);
                request.ToLanguage = await _context.Languages.FirstOrDefaultAsync(n => n.Id == request.ToLanguageId);
            }

            return result;
        }


        public async Task<IEnumerable<Request>> GetAllCompleted()
        {
            return await _context.Requests.Where(x => x.IsPending == false).ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetAllPending()
        {
            return await _context.Requests.Where(x => x.IsPending == true).ToListAsync();
        }

        public async Task<Request> GetByIdAsync(int id)
        {
            return await _context.Requests.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<DetailsRequestViewModel> GetVByIdAsync(int id)
        {
            var result = new DetailsRequestViewModel();
            Request request = await _context.Requests.FirstOrDefaultAsync(i => i.Id == id);



            result.Id = request.Id;
            result.FirstName = request.FirstName;
            result.FamilyName = request.FamilyName;
            result.Phone = request.Phone;
            result.Email = request.Email;
            result.FromLanguage = _context.Languages.FirstOrDefault(l => l.Id == request.FromLanguageId);
            result.ToLanguage = _context.Languages.FirstOrDefault(l => l.Id == request.ToLanguageId);
            result.AdditionalInfo = request.AdditionalInfo;
            result.DateCreated = request.DateCreated;


            List<RequestFiles> requestFiles = _context.RequestFiles.Where(i => i.UserId == id).ToList();
            List<string> files = new List<string>();

            foreach (var item in requestFiles)
            {
                files.Add(item.Route);
            }

            result.Files = files;

            return result;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Request request)
        {
            _context.Update(request);
            return Save();
        }

        public void UpdateByIdPending(int id)
        {
            Request result = _context.Requests.FirstOrDefault(i => i.Id == id);
            result.IsPending = true;
            Update(result);
        }

        public void UpdateByIdReady(int id)
        {
            Request result = _context.Requests.FirstOrDefault(i => i.Id == id);
            result.IsPending = false;
            Update(result);
        }
    }
}

using LegalTranslation.Data;
using LegalTranslation.Models;
using LegalTranslation.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LegalTranslation.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Request>> GetAll();
        Task<IEnumerable<Request>> GetAllPending();
        Task<IEnumerable<Request>> GetAllCompleted();
        Task<Request> GetByIdAsync(int id);
        Task<DetailsRequestViewModel> GetVByIdAsync(int id);
        void UpdateByIdPending(int id);
        void UpdateByIdReady(int id);
        bool Add(Request request);
        bool Update(Request request);
        bool Delete(Request request);
        bool Save();
        Task<IdentityResult> ChangeAdminEmail(ChangeEmailViewModel changeEmailVM);
        Task<IdentityResult> ChangePassword(ChangePasswordViewModel changePasswordVM);
        Task<IdentityResult> ChangeAdminForgottenPassword(string id, string password);
        public AppUser GetAdminData();
        public CustomErrorMessage CheckPasswords(ChangePasswordViewModel changePasswordVM);
        public CustomErrorMessage UpdateAdminEmail(ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM);
        public CustomErrorMessage UpdateCompanyEmail(ChangeAdminOrCompanyEmailViewModel changeAOCEmailVM);
        public string GetCompanyEmail();
        public string GetAdminEmail();
    }
}

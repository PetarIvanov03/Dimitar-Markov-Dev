using LegalTranslation.Data;
using LegalTranslation.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LegalTranslation.Interfaces
{
    public interface IAccountRepository
    {
        bool CheckEmailCategory(string email);
        bool SendAdmin(string email);
        bool SendWorker(string email);
        bool SendUnidentified(string email);
    }
}

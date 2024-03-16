using LegalTranslation.Models;
using LegalTranslation.ViewModels;

namespace LegalTranslation.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAll();
        Task<Request> GetByIdAsync(int id);
        List<int> GetIdByEmail(string email);
        List<int> GetIdByPhone(string phone);
        int GetLatestId(string phone, string email);
        bool Add(Request request);
        bool Update(Request request);
        bool Delete(Request request);
        bool Save();
        bool SendEmail(CreateRequestViewModel createRequestVM, List<string> files);
    }
}

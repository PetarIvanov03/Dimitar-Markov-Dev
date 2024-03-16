using LegalTranslation.Models;

namespace LegalTranslation.Interfaces
{
    public interface IRequestFilesRepository
    {
        Task<IEnumerable<RequestFiles>> GetAll();
        Task<RequestFiles> GetByIdAsync(int id);
        Task<ICollection<RequestFiles>> GetAllByIdAsync(int id);
        bool Add(RequestFiles requestFiles);
        bool Update(RequestFiles requestFiles);
        bool Delete(RequestFiles requestFiles);
        bool Save();
    }
}

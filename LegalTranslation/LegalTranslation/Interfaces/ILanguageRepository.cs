using LegalTranslation.Models;

namespace LegalTranslation.Interfaces
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Language>> GetAll();
        Task<IEnumerable<Language>> GetAllActive();
        Task<Language> GetById(int id);
        bool Add(Language language);
        bool Update(Language language);
        bool UpdateAll(IEnumerable<int> languagesIds);
        bool Delete(Language language);
        bool Save();
    }
}

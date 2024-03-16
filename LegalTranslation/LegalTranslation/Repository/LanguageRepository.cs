using LegalTranslation.Data;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using Microsoft.EntityFrameworkCore;

namespace LegalTranslation.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly AppDbContext _context;

        public LanguageRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(Language language)
        {
            _context.Add(language);
            return Save();
        }

        public bool Delete(Language language)
        {
            _context.Remove(language);
            return Save();
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<IEnumerable<Language>> GetAllActive()
        {
            return await _context.Languages.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<Language> GetById(int id)
        {
            Language language = null;
            language = await _context.Languages.FirstOrDefaultAsync(x => x.Id == id);
            return language;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Language language)
        {
            _context.Update(language);
            return Save();
        }

        public bool UpdateAll(IEnumerable<int> languagesIds)
        {
            IEnumerable<Language> languages = _context.Languages.ToList();
            foreach (var item in languages)
            {
                if (languagesIds.Contains(item.Id))
                {
                    item.IsActive = true;
                }
                else
                {
                    item.IsActive = false;
                }
                Update(item);
            }
            return Save();
        }
    }
}

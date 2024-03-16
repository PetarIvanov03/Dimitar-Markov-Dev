using LegalTranslation.Data;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using Microsoft.EntityFrameworkCore;

namespace LegalTranslation.Repository
{
    public class RequestFilesRepository : IRequestFilesRepository
    {
        private readonly AppDbContext _context;

        public RequestFilesRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(RequestFiles requestFiles)
        {
            _context.Add(requestFiles);
            return Save();
        }

        public bool Delete(RequestFiles requestFiles)
        {
            return Save();
        }

        public async Task<IEnumerable<RequestFiles>> GetAll()
        {
            return await _context.RequestFiles.ToListAsync();
        }

        public async Task<ICollection<RequestFiles>> GetAllByIdAsync(int id)
        {
            return await _context.RequestFiles.Where(i => i.Id == id).ToListAsync();
        }

        public async Task<RequestFiles> GetByIdAsync(int id)
        {
            return await _context.RequestFiles.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(RequestFiles requestFiles)
        {
            return Save();
        }
    }
}

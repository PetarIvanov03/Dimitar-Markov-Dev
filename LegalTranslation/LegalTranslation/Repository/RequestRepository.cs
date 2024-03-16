using LegalTranslation.Data;
using LegalTranslation.Helpers;
using LegalTranslation.Interfaces;
using LegalTranslation.Models;
using LegalTranslation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LegalTranslation.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AppDbContext _context;

        public RequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Request request)
        {
            _context.Add(request);
            return Save();
        }

        public bool Delete(Request request)
        {
            _context.Remove(request);
            return Save();
        }

        public async Task<IEnumerable<Request>> GetAll()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<Request> GetByIdAsync(int id)
        {
            return await _context.Requests.FirstOrDefaultAsync(i => i.Id == id);
        }

        public List<int> GetIdByEmail(string email)
        {
            List<int> result = new List<int>();
            foreach (var item in _context.Requests.Where(i => i.Email == email))
            {
                result.Add(item.Id);
            }

            return result;
        }

        public List<int> GetIdByPhone(string phone)
        {
            List<int> result = new List<int>();
            foreach (var item in _context.Requests.Where(i => i.Phone == phone))
            {
                result.Add(item.Id);
            }

            return result;
        }

        public int GetLatestId(string phone, string email)
        {
            IEnumerable<int> commonElements = GetIdByPhone(phone).Intersect(GetIdByEmail(email));

            // If there are common elements, return the maximum value
            if (commonElements.Any())
            {
                return commonElements.Max();
            }
            else
            {
                // No common elements found
                return int.MinValue; // You can choose another value to indicate no common elements
            }
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SendEmail(CreateRequestViewModel createRequestVM, List<string> files)
        {
            var emailSender = new EmailSender(_context);
            emailSender.SendFiles($"Клиент: {createRequestVM.FirstName + " " + createRequestVM.FamilyName} - телефон: {createRequestVM.Phone}, e-mail: {createRequestVM.Email}" +
                $"\n Допълнителна информация: {createRequestVM.AdditionalInfo}", "Нова заявка!", files);
            return true;
        }

        public bool Update(Request request)
        {
            _context.Update(request);
            return Save();
        }
    }
}

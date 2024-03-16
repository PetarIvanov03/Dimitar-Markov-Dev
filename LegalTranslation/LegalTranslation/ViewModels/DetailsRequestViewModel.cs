using LegalTranslation.Data;
using LegalTranslation.Models;

namespace LegalTranslation.ViewModels
{
    public class DetailsRequestViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Language FromLanguage { get; set; }
        public Language ToLanguage { get; set; }
        public ICollection<string> Files { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsPending { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

using LegalTranslation.Data;
using LegalTranslation.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegalTranslation.ViewModels
{
    public class CreateRequestViewModel
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FromLanguage { get; set; }
        public string ToLanguage { get; set; }
        public IEnumerable<SelectListItem>? AvailableLanguages { get; set; }
        public ICollection<IFormFile> Files { get; set; }
        public string AdditionalInfo { get; set; }
    }
}

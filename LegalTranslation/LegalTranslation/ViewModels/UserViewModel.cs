using System.ComponentModel.DataAnnotations;

namespace LegalTranslation.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}

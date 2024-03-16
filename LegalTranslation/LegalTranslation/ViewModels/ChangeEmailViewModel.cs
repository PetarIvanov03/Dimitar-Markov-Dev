using System.ComponentModel.DataAnnotations;

namespace LegalTranslation.ViewModels
{
    public class ChangeEmailViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string OldEmail { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string NewEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

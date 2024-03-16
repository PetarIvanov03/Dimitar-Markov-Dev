using System.ComponentModel.DataAnnotations;

namespace LegalTranslation.ViewModels
{
    public class ChangeAdminOrCompanyEmailViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string ConfirmationPassword { get; set; }
    }
}

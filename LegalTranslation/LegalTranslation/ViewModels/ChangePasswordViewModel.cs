using System.ComponentModel.DataAnnotations;

namespace LegalTranslation.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}

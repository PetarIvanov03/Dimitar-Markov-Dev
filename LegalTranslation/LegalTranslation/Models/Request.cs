using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalTranslation.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int FromLanguageId { get; set; }
        [ForeignKey("FromLanguageId")]
        public Language FromLanguage { get; set; }
        public int ToLanguageId { get; set; }
        [ForeignKey("ToLanguageId")]
        public Language ToLanguage { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsPending { get; set; }
        public DateTime DateCreated{ get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LegalTranslation.Models
{
    public class Emails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}

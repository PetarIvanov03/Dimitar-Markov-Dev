using System.ComponentModel.DataAnnotations;

namespace LegalTranslation.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}

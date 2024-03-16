using System.ComponentModel.DataAnnotations.Schema;

namespace LegalTranslation.Models
{
    public class RequestFiles
    {
        public int Id { get; set; }
        [ForeignKey("Requests")]
        public int UserId { get; set; }
        public string Route { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class EntityBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; } = DateTimeOffset.Now;
        public string? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}

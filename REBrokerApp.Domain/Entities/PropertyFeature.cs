using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class PropertyFeature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        [MaxLength(500)]
        public string? Internal { get; set; }

        [MaxLength(500)]
        public string External { get; set; }

        [MaxLength(500)]
        public string General { get; set; }

        [MaxLength(500)]
        public string Security { get; set; }

        [MaxLength(500)]
        public string Location { get; set; }

        [MaxLength(500)]
        public string LifeStyle { get; set; }    
    }
}

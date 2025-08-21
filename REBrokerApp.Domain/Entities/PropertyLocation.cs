using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class PropertyLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        [MaxLength(100)]
        public string? Address1 { get; set; }

        [MaxLength(100)]
        public string? Address2 { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(100)]
        public string? Suburb { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(20)]
        public string? Latitude { get; set; }

        [MaxLength(20)]
        public string? Longitude { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class PropertyImage : EntityBase
    {
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

        public required byte[] ImageData { get; set; }

        [MaxLength(20)]
        public required string ImageMimeType { get; set; }

        [MaxLength(50)]
        public required string FileName { get; set; }
    }
}

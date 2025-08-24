using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REBrokerApp.Domain.Entities
{
    public class BrokerCommisionSetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MinPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MaxPrice { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal Percentage { get; set; } 
    }
}

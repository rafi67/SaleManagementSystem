using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INvoicePracitic.Models
{
    public class SaleDetails
    {
        [Key]
        public long SaleDetailId { get; set; }
        public long SaleId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        
        [ForeignKey("SaleId")]
        public virtual SaleMaster SaleMasters { get; set; }
    }
}

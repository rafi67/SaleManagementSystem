using System.ComponentModel.DataAnnotations;

namespace INvoicePracitic.Models
{
    public class SaleMaster
    {
        [Key]
        public long SaleId { get; set; }

        public DateTime? CreateDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string? Photo { get; set; }
        public ICollection<SaleDetails> SaleDetails { get; set; }
    }
}

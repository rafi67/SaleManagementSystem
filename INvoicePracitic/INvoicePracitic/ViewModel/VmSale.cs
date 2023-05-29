namespace INvoicePracitic.ViewModel
{
    public class VmSale
    {
        public long SaleId { get; set; }

        public DateTime? CreateDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string? Photo { get; set; }
        public IFormFile? File { get; set; }
        public string[] ProductName { get; set; }
        public int[]? Qty { get; set; }
        public decimal[]? Price { get; set; }
        public List<VmSaleDetail> SaleDetails { get; set; } = new List<VmSaleDetail>();
        public class VmSaleDetail
        {
            public long SaleDetailId { get; set; }
            public long SaleId { get; set; }
            public string ProductName { get; set; }
            public int Qty { get; set; }
            public decimal Price { get; set; }
        }
    }
}

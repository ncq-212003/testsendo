namespace Erp.Social.Sendo.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public int DId { get; set; }
        public int ThirdpartyId { get; set; }
        public string ThirdpartySKU { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string Unit { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string HasdCode { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime LastModifyAt { get; set; }
        public int LastModifyBy { get; set; }
        public char Flag { get; set; }
    }
}

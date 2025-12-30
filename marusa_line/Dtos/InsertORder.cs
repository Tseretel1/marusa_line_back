namespace marusa_line.Dtos
{
    public class InsertOrder
    {
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public string DeliveryType { get; set; }
        public string Comment { get; set; }
        public int FinalPrice { get; set; }
        public string? lng { get; set; }
        public string? lat { get; set; }
        public string? address{ get; set; }
    }
}

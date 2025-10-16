namespace marusa_line.Dtos
{
    public class InsertOrder
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public string DeliveryType { get; set; }
        public string Comment { get; set; }
        public int FinalPrice { get; set; }
    }
}

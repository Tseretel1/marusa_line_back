namespace marusa_line.Dtos.ControlPanelDtos.ShopDtos
{
    public class ShopDto
    {
         public int Id { get; set; }
         public string? Name { get; set; }
         public string? Logo { get; set; }
         public string? Location { get; set; }
         public string? Gmail { get; set; }
         public string? Subscription { get; set; }
         public string? Instagram { get; set; }
         public string? Facebook { get; set; }
         public string? Tiktok { get; set; }
         public string? Bog { get; set; }
         public string? Tbc{ get; set; }
         public string? Receiver { get; set; }
    }
    public class ShopDtoMatch 
    { 
        public ShopDto shop { get; set; }
       public bool isFollowed { get; set; }
    }

}

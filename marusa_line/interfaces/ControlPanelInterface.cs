namespace marusa_line.interfaces
{
    public interface ControlPanelInterface
    {
        Task<int> ToggleOrderIsPaidAsync(int orderId,bool isPaid);
        Task<int> GetOrdersTotalCountAsync(bool? isPaid);
    }
}

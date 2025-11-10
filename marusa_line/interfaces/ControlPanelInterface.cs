namespace marusa_line.interfaces
{
    public interface ControlPanelInterface
    {
        Task<int> ToggleOrderIsPaidAsync(int orderId,bool isPaid);
        Task<int> ChangeOrderStatus(int orderId, int isPaid);
        Task<int> DeleteOrder(int orderId);
        Task<int> GetOrdersTotalCountAsync(bool? isPaid);
    }
}

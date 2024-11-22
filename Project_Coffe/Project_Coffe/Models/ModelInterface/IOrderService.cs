namespace Project_Coffe.Models.ModelInterface
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int userId, List<int> productIds);
    }
}

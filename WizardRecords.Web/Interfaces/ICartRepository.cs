using WizardRecords.Api.Data.Entities;
using WizardRecords.Api.Domain.Entities;

namespace WizardRecords.Api.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllItemAsync();

        Task<IEnumerable<Cart>> GetCartByIdAsync(Guid cartId);
        Task<Cart> GetCartByUserIdAsync(Guid userId);

        //CRUD
        Task<Cart?> DeleteItemByIdAsync(Guid cartId, Guid AlbumId);
        Task<Cart?> UpdateItemByIdAsync(Guid cartId, Guid AlbumId, int qty);
        Task<Cart?> AddItemByIdAsync(Guid cartId, Guid AlbumId);


        Task<Cart> CreateCartAsync(Guid userId);
        Task<Cart?> GetUserCartAsync(Guid userId);

        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> CreateUserGuest();
        Task<User> DeleteUserGuest(Guid userId);

        List<Order> GetUserOrders(Guid userId);
        List<Order> GetAllOrders();
        Order GetOrderById(Guid orderId);
        void UpdateOrder(Order order);
        void CancelOrder(Order order);
        Order CreateOrder(Cart cart);
        bool FindByEmail(string email);
        void UpdatePriceCart(Cart cart);
        void addPayment(Payment payment);
        Task<IEnumerable<Payment>> GetAllPaymentsByUserId(Guid userId);
    }
}

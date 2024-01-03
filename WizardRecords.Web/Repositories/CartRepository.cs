using WizardRecords.Api.Data.Entities;
using WizardRecords.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using WizardRecords.Api.Domain.Entities;



namespace WizardRecords.Api.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly WizRecDbContext _dbContext;

        public CartRepository(WizRecDbContext context)
        {
            _dbContext = context;
        }


      
        public async Task<Cart?> AddItemByIdAsync(Guid cartId, Guid albumId)
        {
            try
            {
                var cart = await _dbContext.Carts

                    .Include(c => c.CartItems).ThenInclude(ci => ci.Album)
                    .Where(c => c.CartId == cartId)
                    .FirstOrDefaultAsync();

                var album = await _dbContext.Albums.Where(a => a.AlbumId == albumId).FirstOrDefaultAsync();

                if (cart != null && album != null)
                {
                    var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Album.AlbumId == albumId);

                    if (cartItem == null)
                    {
                        if (album.Quantity == 0)
                        {
                            return null;
                        }
                        else {

                            cartItem = new CartItem();
                            cartItem.CartId = cart.CartId;
                            cartItem.Album = album;
                            cartItem.Quantity = 1;
                            cart.CartItems.Add(cartItem);
                            album.Quantity--;
                        }
                   
                    
                    }
                    else
                    {
                        if (album.Quantity == 0)
                        {
                            return null;
                        }
                        else
                        {
                            album.Quantity--;
                        cartItem.Quantity++;
                        }
                   
                    }

                    await _dbContext.SaveChangesAsync();
                    return cart;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Une erreur s'est produite dans AddItemByIdAsync : {ex.Message}");
                throw;
            }
        }

        public async Task<Cart> CreateCartAsync(Guid userId)
        {
            try
            {
                var user = await _dbContext.Client.Where(u => u.Id == userId).FirstOrDefaultAsync();
                Cart cart;

                if (user == null)
                {
                    await _dbContext.Carts.AddAsync(new Cart());
                    await _dbContext.SaveChangesAsync();
                    cart = await _dbContext.Carts.Where(c => c.UserId == userId).FirstOrDefaultAsync();
                }
                else
                {
                    cart = await _dbContext.Carts.Where(c => c.UserId == user.Id).FirstOrDefaultAsync();

                    if (cart == null)
                    {
                        await _dbContext.Carts.AddAsync(new Cart { UserId = user.Id });
                        await _dbContext.SaveChangesAsync();
                        cart = await _dbContext.Carts.Where(c => c.UserId == user.Id).FirstOrDefaultAsync();


                    }
                    
                }

                return cart;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> CreateUserGuest()
        {
            try
            {
                User guest = new(userName: "Guest")
                {
                    Id = Guid.NewGuid(),
                    UserName = "Guest",
                    Email = "",
                    City = "",
                    PostalCode = "",
                    StreetName = "",

                    PhoneNumber = "",
                    FirstName = "",
                    LastName = "",
                    AddressNum = 0
                };

                await _dbContext.Client.AddAsync(guest);
                await _dbContext.SaveChangesAsync();


                return guest;
            }
            catch (Exception)
            {
                throw;
            }
        }

    



        public async Task<Cart?> DeleteItemByIdAsync(Guid cartId, Guid albumId)
        {
            try
            {
                var cart = await _dbContext.Carts
                  .Include(c => c.CartItems).ThenInclude(ci => ci.Album)
                  .Where(c => c.CartId == cartId)
                  .FirstOrDefaultAsync();

                var album = await _dbContext.Albums.Where(a => a.AlbumId == albumId).FirstOrDefaultAsync();

                var cartItem = cart.CartItems.FirstOrDefault(c => c.Album.AlbumId == album.AlbumId);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                       
                        cartItem.Quantity--;
                    }
                    else
                    {
                
                        cart.CartItems.Remove(cartItem);
                    }
                    album.Quantity++;
                    await _dbContext.SaveChangesAsync();


                    return cart;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> DeleteUserGuest(Guid userId)
        {
            var use = await _dbContext.Client.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (use != null)
            {
                _dbContext.Client.Remove(use);
                await _dbContext.SaveChangesAsync();
                return use;
            }
            else
            {
                return null;
            }
        }

        public Task<string> GenerateJwtTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cart>> GetAllItemAsync()
        {
            return await _dbContext.Carts.ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartByIdAsync(Guid cartId)
        {
            return await _dbContext.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Album).Where(a => a.CartId == cartId).ToListAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _dbContext.Carts.Include(x => x.CartItems).ThenInclude(c => c.Album).FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _dbContext.Client.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<Cart?> GetUserCartAsync(Guid userId)
        {
            try
            {
                var cart = await _dbContext.Carts
     .Include(c => c.CartItems) // Inclure les cart items
         .ThenInclude(ci => ci.Album) // Inclure les albums pour chaque cart item
     .Where(c => c.UserId == userId)
     .FirstOrDefaultAsync();

                if (cart == null)
                {
                    return null;
                }
                else
                {

                    return cart;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<Cart?> UpdateItemByIdAsync(Guid cartId, Guid albumId, int qty)
        {
            try
            {
                var cart = await _dbContext.Carts
                    .Include(c => c.CartItems)
                    .Where(c => c.CartId == cartId)
                    .FirstOrDefaultAsync();

                if (cart != null)
                {
                    var cartItem = cart.CartItems.FirstOrDefault(ci => ci.AlbumId == albumId);

                    if (cartItem != null)
                    {
                        cartItem.Quantity = qty;

                        await _dbContext.SaveChangesAsync();
                    }

                    return cart;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Order> GetUserOrders(Guid userId) {
            try
            {
                var orders = _dbContext.Orders
                    .Include(c => c.CartItems)
                    .ThenInclude(c => c.Album)
                    .Where(c => c.UserId == userId)
                    .ToList();

                if (orders != null)
                {
                    return orders;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Order> GetAllOrders() {
            try
            {
                var orders = _dbContext.Orders
                    .Include(c => c.CartItems)
                    .ThenInclude(c => c.Album)
                    .ToList();

                if (orders != null)
                {
                    return orders;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Order GetOrderById(Guid orderId)
        {
            return _dbContext.Orders.Include(x => x.CartItems)
                                          .FirstOrDefault(x => x.OrderId == orderId);
        }

        public void UpdateOrder(Order order)
        {
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        public void CancelOrder(Order order)
        {
            Cart cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == order.UserId);
            order.State = OrderState.Annulée;

            if(cart != null)
                cart.CartItems.AddRange(order.CartItems);
            else
            {
                cart = new Cart();
                cart.CartItems.AddRange(order.CartItems);
                cart.UserId = order.UserId;
                _dbContext.Carts.Add(cart);
            }
            
            _dbContext.SaveChanges();
        }

        public Order CreateOrder(Cart cart) 
        {
            Order order = new Order(cart);
            
            _dbContext.Carts.Remove(cart);
            _dbContext.Orders.Add(order);

            _dbContext.SaveChanges();

            return order;
        }

        public bool FindByEmail(string email)
        {
            var user = _dbContext.Client.Where(u => u.Email == email).FirstOrDefault();
            return user != null;
        }

        public void UpdatePriceCart(Cart cart)
        {
            _dbContext.Carts.Update(cart);
            _dbContext.SaveChanges();
        }

        public void addPayment(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            _dbContext.SaveChanges();
        }

        public Task<IEnumerable<Payment>> GetAllPaymentsByUserId(Guid userId)
        {
            return Task.FromResult(_dbContext.Payments.Where(x => x.UserId == userId).ToList() as IEnumerable<Payment>);
        }

    }
}

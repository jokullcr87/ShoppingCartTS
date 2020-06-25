using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartBlazor.Data
{
    public class CartService
    {
        private readonly ApplicationDbContext mContext;

        public CartService(ApplicationDbContext context)
        {
          mContext = context;
        }

        public Task<Cart> Get(string name)
        {
            return Task<Cart>.Run(async () => {
              Console.WriteLine(name);
              IdentityUser user = mContext.Users.SingleOrDefault(u=>u.UserName.Equals(name));

              var cart = mContext.Cart
                              .Include(c => c.Owner)
                              .Include(c => c.Products)
                              .AsNoTracking()
                              .SingleOrDefault(p => p.Owner == user);
              if (cart == null) {
                cart = new Cart();
                cart.Owner = user;
                cart.Products = new List<CartItem>();
                await Insert(cart);
              }

              return cart;
            });
        }

        private Task Insert(Cart cart) {
            return Task.Run(() => {
              mContext.Cart.Add(cart);
              mContext.SaveChanges();
            });
        }

        public Task Update(Cart cart) {
            return Task.Run(() => {
              var oldCart = mContext.Cart.AsNoTracking().Single(c => c.Id == cart.Id);
              oldCart.Products = cart.Products;

              mContext.SaveChanges();
            });
        }
    }
}

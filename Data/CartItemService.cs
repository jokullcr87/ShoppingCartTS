using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartBlazor.Data
{
    public class CartItemService
    {
        private readonly ApplicationDbContext mContext;
        private readonly ProductService mProductService;

        public CartItemService(ApplicationDbContext context, ProductService pService)
        {
          mContext = context;
          mProductService = pService;
        }

        public Task<CartItem> Update(CartItem cartItem) {
            return Task.Run(async () => {
              var oldCartItem = mContext.CartItems.AsNoTracking().Single(ci => ci.Id == cartItem.Id);
              var delta = cartItem.Quantity - oldCartItem.Quantity;

              Console.WriteLine("delta: " + delta);

              if (delta < 0 || (delta > 0 && await mProductService.HasAvailableItems(cartItem.Item, Convert.ToInt64(delta))))
                  oldCartItem.Quantity = cartItem.Quantity;
              else //Rollback change
                  cartItem.Quantity = oldCartItem.Quantity;

              mContext.SaveChanges();

              return cartItem;
            });
        }

        public Task Delete(CartItem item) {
            return Task.Run(() => {
              mContext.CartItems.Remove(mContext.CartItems.AsNoTracking().Single(ci => ci.Id == item.Id));
              mContext.SaveChanges();
            });
        }
    }
}

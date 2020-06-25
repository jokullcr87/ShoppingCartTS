using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.EntityFrameworkCore;

namespace ShoppingCartBlazor.Data
{
    public class ProductService
    {
        private readonly ApplicationDbContext mContext;

        public ProductService(ApplicationDbContext context)
        {
            mContext = context;
        }

        public Task<List<Product>> Get(bool isListed = true)
        {
            return Task.Run(async() => {
                var cartItems = await mContext.CartItems.Include(ci => ci.Item).AsNoTracking().ToListAsync();
                var products = await mContext.Products.AsNoTracking().ToListAsync();
                return products.GroupJoin(
                        cartItems,
                        p => p.ID,
                        ci => ci.Item.ID,
                        (p, ciList) => new { p, Quantity = Convert.ToInt64(ciList.Sum(ci => ci.Quantity)) })
                    .Select(x => new Product() {
                          ID = x.p.ID,
                          Name = x.p.Name,
                          IsListed = x.p.IsListed,
                          Price = x.p.Price,
                          Stock = (x.p.Stock - x.Quantity)
                      }).Where(p => p.IsListed == isListed && p.Stock > 0).ToList();
            });
        }

        public Task Insert(Product product)
        {
            return Task.Run(() => {
              mContext.Products.Add(product);
              mContext.SaveChanges();
            });
        }

        public Task Delete(Product product)
        {
            return Task.Run(() => {
              mContext.Products.Remove(product);
              mContext.SaveChanges();
            });
        }

        public Task Update(Product product)
        {
            return Task.Run(() => {
              var savedProduct = mContext.Products.AsNoTracking().First(p => p.ID == product.ID);
              savedProduct.Name = product.Name;
              savedProduct.IsListed = product.IsListed;
              savedProduct.Price = product.Price;
              savedProduct.Stock = product.Stock;
              mContext.SaveChanges();
            });
        }

        private bool HasAvailableItemsSync(Product product, long amount = 1) {
            long stock = mContext.Products.AsNoTracking().Single(p => p.ID == product.ID).Stock;
            long quantityInCarts = Convert.ToInt64(mContext.CartItems.AsNoTracking().Where(ci => ci.Item.ID == product.ID).Sum(ci => ci.Quantity));

            return stock - quantityInCarts >= amount;
        }

        public Task<bool> HasAvailableItems(Product product, long amount = 1) {
            return Task<bool>.Run(() => {
                  return HasAvailableItemsSync(product, amount);
            });
        }
    }
}

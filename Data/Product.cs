using System;

namespace ShoppingCartBlazor.Data {

  public class Product {
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsListed { get; set; }
    public decimal Price { get; set; }
    public long Stock { get; set; }
  }

}

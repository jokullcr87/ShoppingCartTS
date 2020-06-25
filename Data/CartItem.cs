using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCartBlazor.Data {

  public class CartItem {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; protected set; }
    public Product Item { get; set; }
    public int Quantity { get; set; } = 0;
    //public Cart OwnerCart {get;set;}
  }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCartBlazor.Data {

  public class Cart {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; protected set; }

    public IdentityUser Owner { get; set; }
    public List<CartItem> Products { get; set; } = new List<CartItem>();
  }

}

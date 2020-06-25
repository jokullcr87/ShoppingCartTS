using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ShoppingCartBlazor.Data;

namespace ShoppingCartBlazor.API {

  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : Controller {

    private ProductService mService;

    public ProductsController(ProductService service) {
      mService = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get(
        [FromQuery] bool isListed = true)
    {
        List<Product> result = await mService.Get(isListed);
        return result;
    }
  }

}

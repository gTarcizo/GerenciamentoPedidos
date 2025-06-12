using ProductsAPI.Domain;
using ProductsAPI.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace ProductsAPI.Endpoints.Pedidos;

public class PedidosGETAll
{
  public static string Pattern => "/pedidos";
  public static string[] Methods => [HttpMethod.Get.ToString()];
  public static Delegate Handler => Action;

   [Authorize]
   public static IResult Action(AppDbContext context)
  {
    try
    {
      List<Pedido> categoryList = new List<Pedido>();
      categoryList = context.Pedidos.ToList();
      return Results.Ok(categoryList);
    }
    catch(Exception e)
    {
      return Results.BadRequest(e.ToString());
    }
  }
}

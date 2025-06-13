using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Interface;
using Shared.EnumsSistema;
using Shared.Infra.Data;

namespace ProductAPI.Endpoints.Pedidos;

public class PedidoPUT
{
   public static string Pattern => "/pedido/{id:int}";
   public static string[] Methods => [HttpMethod.Put.ToString()];
   public static Delegate Handler => Action;

   [Authorize]
   public static async Task<IResult> Action([FromRoute] int id, StatusPedidoEnum status, HttpContext http, IPedidoRepository repository)
   {

      try
      {
         await repository.UpdateStatusAsync(id, status);
         return Results.Created($"/pedido/{id}", id);
      }
      catch (Exception e)
      {
         return Results.BadRequest(e.ToString());
      }
   }
}

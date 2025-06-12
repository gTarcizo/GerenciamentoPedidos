using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Domain;
using ProductsAPI.Endpoints.Pedidos;
using ProductsAPI.EnumsSistema;
using ProductsAPI.Infra.Data;
using System.Security.Claims;

namespace ProductsAPI.Endpoints.Pedidos;

public class PedidoPUT
{
   public static string Pattern => "/pedido/{id:int}";
   public static string[] Methods => [HttpMethod.Put.ToString()];
   public static Delegate Handler => Action;

   [Authorize]
   public static IResult Action([FromRoute] int id, StatusPedidoEnum status, HttpContext http, AppDbContext context)

   {
      try
      {
         var pedido = context.Pedidos.Where(x => x.Id == id).FirstOrDefault();
         if (pedido == null) return Results.NotFound("Pedido não Encontrado");

         pedido.Status = status;
         context.Pedidos.Update(pedido);
         context.SaveChanges();

         return Results.Created($"/pedido/{pedido.Id}", pedido.Id);
      }
      catch (Exception e)
      {
         return Results.BadRequest(e.ToString());
      }
   }
}

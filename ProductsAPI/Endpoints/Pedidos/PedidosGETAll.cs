using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain;
using ProductsAPI.Infra.Data;

namespace ProductsAPI.Endpoints.Pedidos;

public class PedidosGETAll
{
   public static string Pattern => "/pedidos";
   public static string[] Methods => [HttpMethod.Get.ToString()];
   public static Delegate Handler => Action;

   [Authorize]
   public static async Task<IResult> Action(AppDbContext context)
   {
      try
      {
         List<Pedido> pedidos = new List<Pedido>();
         List<PedidoDto> pedidoDto = new List<PedidoDto>();
         pedidos = await context.Pedidos.Include(p => p.Itens).ToListAsync();
         foreach (var pedido in pedidos)
         {
            pedidoDto.Add(new PedidoDto(pedido.Id, pedido.Cliente, pedido.Itens, pedido.Total, pedido.Status));
         }

         return Results.Ok(pedidoDto);
      }
      catch (Exception e)
      {
         return Results.BadRequest(e.ToString());
      }
   }
}

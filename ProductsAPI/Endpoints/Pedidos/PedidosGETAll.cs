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
         List<PedidoDto> pedidosDTO = new List<PedidoDto>();

         pedidos = await context.Pedidos.Include(p=> p.Itens).ToListAsync();
         
         foreach (var pedido in pedidos)
         {
            List<ItemPedidoDTO> itensDTO = new List<ItemPedidoDTO>();
            
            foreach (var item in pedido.Itens)
            {
               itensDTO.Add(new ItemPedidoDTO(item.Nome, item.Quantidade, item.PrecoUnitario));
            }
            pedidosDTO.Add(new PedidoDto(pedido.Id, pedido.Cliente, itensDTO, pedido.Total, pedido.Status));
         }
         return Results.Ok(pedidosDTO);
      }
      catch (Exception e)
      {
         return Results.BadRequest(e.ToString());
      }
   }
}

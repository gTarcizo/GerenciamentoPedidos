using Microsoft.AspNetCore.Authorization;
using Shared.Domain;
using Shared.Domain.Interface;

namespace ProductAPI.Endpoints.Pedidos;

public class PedidosGETAll
{
   public static string Pattern => "/pedidos";
   public static string[] Methods => [HttpMethod.Get.ToString()];
   public static Delegate Handler => Action;

   [Authorize]
   public static async Task<IResult> Action(IPedidoRepository repository)
   {
      try
      {
         List<Pedido> pedidos = new List<Pedido>();
         List<PedidoDto> pedidoDto = new List<PedidoDto>();
         pedidos = await repository.GetAllPedidos();
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

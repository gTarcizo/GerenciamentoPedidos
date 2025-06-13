using Shared.Domain;
using Microsoft.AspNetCore.Authorization;
using Shared.Domain.Interface;

namespace ProductAPI.Endpoints.Pedidos;

public class PedidoPOST
{
  public static string Pattern => "/pedido";
  public static string[] Methods => [HttpMethod.Post.ToString()];
  public static Delegate Handler => Action;

  [Authorize]
  public static async Task<IResult> Action(PedidoDto pedidoDto,HttpContext http , IPedidoPublisher publisher)
  {
    try
    {
         var pedido = new Pedido()
         {
            Cliente = pedidoDto.cliente,
            Itens = pedidoDto.itens.Select(i => new ItemPedido
            {
               Nome = i.Nome,
               Quantidade = i.Quantidade,
               PrecoUnitario = i.PrecoUnitario
            }).ToList(),
            Total = pedidoDto.totalPedido,
            Status = pedidoDto.status
         };

         await publisher.PublicarPedidoNoRabbitMQ(pedido);

         return Results.Created("/pedido", $"Pedido realizado! total de: {pedido.Total}");
      }
      catch(Exception e)
    {
      return Results.BadRequest(e.ToString());
    }
  }
}

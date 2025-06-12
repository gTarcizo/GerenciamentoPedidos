using ProductsAPI.Domain;
using ProductsAPI.Endpoints.Pedidos;
using ProductsAPI.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace ProductsAPI.Endpoints.Pedidos;

public class PedidoPOST
{
  public static string Pattern => "/pedido";
  public static string[] Methods => [HttpMethod.Post.ToString()];
  public static Delegate Handler => Action;

  [Authorize]
  public static async Task<IResult> Action(PedidoDto pedidoDto,HttpContext http ,AppDbContext context)
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

         var factory = new ConnectionFactory { HostName = "localhost" };
         var connection = await factory.CreateConnectionAsync();
         var channel = await connection.CreateChannelAsync();
         await channel.QueueDeclareAsync(queue: "criar-pedido", durable: false, exclusive: false, autoDelete: false, arguments: null);

         var message = JsonSerializer.Serialize(pedido);
         var body = Encoding.UTF8.GetBytes(message);

         await channel.BasicPublishAsync(exchange: "", routingKey: "criar-pedido", body: body);
         
         return Results.Created("Pedido realizado!", $"Total de: {pedido.Total}");
      }
      catch(Exception e)
    {
      return Results.BadRequest(e.ToString());
    }
  }
}

using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using Shared.Domain;
using Shared.Domain.Interface;
using System.Text;
using System.Text.Json;

namespace Shared.Infra.Messaging;
public class PedidoPublisher : IPedidoPublisher
{
   private readonly string _hostname = "localhost";

   public async Task PublicarPedidoAsync(Pedido pedido)
   {
      var factory = new ConnectionFactory { HostName = _hostname };
      var connection = await factory.CreateConnectionAsync();
      var channel = await connection.CreateChannelAsync();

      await channel.QueueDeclareAsync(
          queue: "criar-pedido",
          durable: false,
          exclusive: false,
          autoDelete: false,
          arguments: null
      );

      var message = JsonSerializer.Serialize(pedido);
      var body = Encoding.UTF8.GetBytes(message);

      await channel.BasicPublishAsync(
          exchange: "",
          routingKey: "criar-pedido",
          body: body
      );
   }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Domain;
using Shared.Infra.Data;
using System.Text;
using System.Text.Json;
namespace PedidoWorker
{
   class Program
   {
      public static async Task Main(string[] args)
      {
         var host = Host.CreateDefaultBuilder(args)
             .ConfigureServices((context, services) =>
             {
                services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer("Server=localhost;Database=PedidosDb;Trusted_Connection=True;TrustServerCertificate=True"));
             })
             .Build();

         var factory = new ConnectionFactory { HostName = "localhost" };
         using var connection = await factory.CreateConnectionAsync();
         using var channel = await connection.CreateChannelAsync();

         await channel.QueueDeclareAsync(
             queue: "criar-pedido",
             durable: false,
             exclusive: false,
             autoDelete: false,
             arguments: null
         );

         Console.WriteLine("Aguardando mensagens.");

         var consumer = new AsyncEventingBasicConsumer(channel);
         consumer.ReceivedAsync += async (model, ea) =>
         {
            try
            {
               var body = ea.Body.ToArray();
               var message = Encoding.UTF8.GetString(body);
               var pedidoDto = JsonSerializer.Deserialize<PedidoDto>(message);

               if (pedidoDto is null)
               {
                  Console.WriteLine("Pedido inválido");
                  return;
               }

               using var scope = host.Services.CreateScope();
               var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

               var pedido = new Pedido
               {
                  Cliente = pedidoDto.cliente,
                  Total = pedidoDto.totalPedido,
                  Status = pedidoDto.status,
                  Itens = pedidoDto.itens.Select(i => new ItemPedido
                  {
                     Nome = i.Nome,
                     Quantidade = i.Quantidade,
                     PrecoUnitario = i.PrecoUnitario
                  }).ToList()
               };

               db.Pedidos.Add(pedido);
               await db.SaveChangesAsync();

               Console.WriteLine($"Pedido ID {pedido.Id} salvo no banco.");
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }

            await Task.CompletedTask;
         };

         await channel.BasicConsumeAsync(queue: "criar-pedido", autoAck: true, consumer: consumer);

         await Task.Delay(Timeout.Infinite);
      }
   }
}
using Shared.Domain;
using Shared.Domain.Interface;

namespace TestesAPI.FakeRepository
{
   public class FakePedidoPublisher : IPedidoPublisher
   {
      public List<Pedido> MensagensPublicadas { get; } = new();

      public Task PublicarPedidoNoRabbitMQ(Pedido pedido)
      {
         MensagensPublicadas.Add(pedido);
         return Task.CompletedTask;
      }
   }
}

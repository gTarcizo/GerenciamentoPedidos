using Shared.Domain;
using Shared.Domain.Interface;
using Shared.EnumsSistema;

namespace TestesAPI.FakeRepository
{
   public class FakePedidoRepository : IPedidoRepository
   {
      private readonly List<Pedido> _pedidos = new();
      public Pedido? GetPedidoById(int id) => _pedidos.FirstOrDefault(p => p.Id == id);

      public Task<List<Pedido>> GetAllPedidos()
      {
         return Task.FromResult(_pedidos);
      }

      public Task UpdateStatusAsync(int id, StatusPedidoEnum novoStatus)
      {
         var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
         if (pedido != null)
         {
            pedido.Status = novoStatus;
         }

         return Task.CompletedTask;
      }

      public void AddPedidoManual(Pedido pedido)
      {
         pedido.Id = _pedidos.Count + 1;
         _pedidos.Add(pedido);
      }

   }
}

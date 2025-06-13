using Shared.Domain;
using Shared.Infra.Data;

namespace ConsumerConsoleExample.Services
{
   public class PedidoConsumerService
   {
      private readonly AppDbContext _context;

      public PedidoConsumerService(AppDbContext context)
      {
         _context = context;
      }

      public async Task ProcessarPedidoAsync(Pedido pedido)
      {
         _context.Pedidos.Add(pedido);
         await _context.SaveChangesAsync();
      }
   }
}

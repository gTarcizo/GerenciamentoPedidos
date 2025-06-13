using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Domain.Interface;
using Shared.EnumsSistema;
using Shared.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infra.Repository
{
   public class PedidoRepository : IPedidoRepository
   {
      private readonly AppDbContext _context;
      public PedidoRepository(AppDbContext context)
      {
         _context = context;
      }
      public async Task<List<Pedido>> GetAllPedidos()
      {
         return await _context.Pedidos.Include(p => p.Itens).ToListAsync();
      }

      public async Task UpdateStatusAsync(int id, StatusPedidoEnum novoStatus)
      {
         var pedido = await _context.Pedidos.FindAsync(id);
         if (pedido == null) throw new Exception("Pedido não encontrado.");

         pedido.Status = novoStatus;
         _context.Pedidos.Update(pedido);
         await _context.SaveChangesAsync();
      }
      public async Task AddAsync(Pedido pedido)
      {
         await _context.Pedidos.AddAsync(pedido);
         await _context.SaveChangesAsync();
      }
   }
}

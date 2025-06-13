using Microsoft.AspNetCore.Http;
using Shared.EnumsSistema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
   public interface IPedidoRepository
   {
      Task<List<Pedido>> GetAllPedidos();

      Task UpdateStatusAsync(int id, StatusPedidoEnum novoStatus);

   }
}

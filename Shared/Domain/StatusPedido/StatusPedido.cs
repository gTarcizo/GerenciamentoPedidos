using Shared.EnumsSistema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Domain;

public class StatusPedido 
{
   [Key]
   public StatusPedidoEnum Id { get; set; }
   public string Status { get; set; } = string.Empty;
   public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}

using ProductsAPI.EnumsSistema;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Domain;

public class Pedido : Entity
{
   public string Cliente { get; set; } = string.Empty;
   public double Total { get; set; }
   public StatusPedidoEnum Status { get; set; }
   public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
   public StatusPedido StatusPedido { get; set; }
}

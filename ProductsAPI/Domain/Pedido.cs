using ProductsAPI.EnumsSistema;

namespace ProductsAPI.Domain;

public class Pedido : Entity
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Itens { get; set; } = string.Empty;
    public double TotalPedido { get; set; }

   public StatusPedidoEnum Status { get; set; }
   public StatusPedido StatusPedido { get; set; }
}

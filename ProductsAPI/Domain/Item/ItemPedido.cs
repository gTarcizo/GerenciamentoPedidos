namespace ProductsAPI.Domain;
public class ItemPedido : Entity
{

   public string Nome { get; set; } = string.Empty;
   public int Quantidade { get; set; }
   public double PrecoUnitario { get; set; } = 0.0;
   public int PedidoId { get; set; }
   public Pedido Pedido { get; set; }
}

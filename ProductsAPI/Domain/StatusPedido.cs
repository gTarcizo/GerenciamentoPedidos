namespace ProductsAPI.Domain;

public class StatusPedido 
{
   public int Id { get; set; }
   public string Status { get; set; } = string.Empty;
   public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}

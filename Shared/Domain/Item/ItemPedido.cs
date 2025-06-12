using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Domain;
public class ItemPedido 
{
   [Key]
   public int Id { get; set; }
   public string Nome { get; set; } = string.Empty;
   public int Quantidade { get; set; }
   public double PrecoUnitario { get; set; } = 0.0;
   public int PedidoId { get; set; }
   [JsonIgnore]
   public Pedido Pedido { get; set; }
}

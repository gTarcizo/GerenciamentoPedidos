using ProductsAPI.EnumsSistema;

namespace ProductsAPI.Domain;
public record PedidoDto(int id, string cliente, List<ItemPedidoDTO> itens, double totalPedido, StatusPedidoEnum status);

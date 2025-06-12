using Shared.EnumsSistema;

namespace Shared.Domain;
public record PedidoDto(int id, string cliente, List<ItemPedido> itens, double totalPedido, StatusPedidoEnum status);

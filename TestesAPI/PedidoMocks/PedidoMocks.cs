using Shared.Domain;
using Shared.EnumsSistema;

namespace TestesAPI.Mocks
{
   public static class PedidoMocks
   {
      public static Pedido PedidoPendente()
      {
         return new Pedido
         {
            Cliente = "João",
            Status = StatusPedidoEnum.Pendente,
            Total = 100,
            Itens = new List<ItemPedido>
                {
                    new ItemPedido { Nome = "Martelo", Quantidade = 1, PrecoUnitario = 100 }
                }
         };
      }

      public static Pedido PedidoProcessado()
      {
         return new Pedido
         {
            Cliente = "Gabriel",
            Status = StatusPedidoEnum.Processado,
            Total = 200,
            Itens = new List<ItemPedido>
                {
                    new ItemPedido { Nome = "Pedra", Quantidade = 2, PrecoUnitario = 100 }
                }
         };
      }
   }
}

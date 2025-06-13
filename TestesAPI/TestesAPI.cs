using Shared.Domain;
using Shared.EnumsSistema;
using TestesAPI.FakeRepository;

namespace TestesAPI
{
   [TestClass]
   public class TestesAPI
   {
      [TestMethod]
      public void GetAllPedidosTest()
      {
      }

      [TestMethod]
      public async Task UpdateStatusPedidoTest()
      {
         var repo = new FakePedidoRepository();
         var pedido = new Pedido
         {
            Cliente = "João",
            Status = StatusPedidoEnum.Pendente
         };

         repo.AddPedidoManual(pedido);


         await repo.UpdateStatusAsync(1, StatusPedidoEnum.Pendente);
         var atualizado = repo.GetPedidoById(1);
         Assert.AreEqual(StatusPedidoEnum.Pendente, atualizado?.Status);
      }

      [TestMethod]
      public void InsertPedidoTest()
      {
      }
   }
}

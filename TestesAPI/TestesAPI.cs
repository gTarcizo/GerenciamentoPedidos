using Shared.Domain;
using Shared.EnumsSistema;
using TestesAPI.FakeRepository;
using TestesAPI.Mocks;

namespace TestesAPI
{
   [TestClass]
   public class TestesAPI
   {
      [TestMethod]
      public void GetAllPedidosTest()
      {
         var repo = new FakePedidoRepository();

         repo.AddPedidoManual(PedidoMocks.PedidoProcessado());
         repo.AddPedidoManual(PedidoMocks.PedidoPendente());

         var pedidos = repo.GetAllPedidos().Result;

         Assert.AreEqual(2, pedidos.Count);
         Assert.IsTrue(pedidos.Count > 0);
         Assert.IsTrue(pedidos.Any(p => p.Itens.Count > 0));

      }

      [TestMethod]
      public async Task UpdateStatusPedidoTest()
      {
         var repo = new FakePedidoRepository();
         repo.AddPedidoManual(PedidoMocks.PedidoPendente());

         await repo.UpdateStatusAsync(1, StatusPedidoEnum.Pendente);
         var atualizado = repo.GetPedidoById(1);
         Assert.AreEqual(StatusPedidoEnum.Pendente, atualizado?.Status);
      }

      [TestMethod]
      public async Task InsertPedidoNoRabbitTest()
      {
         var publisher = new FakePedidoPublisher();

         await publisher.PublicarPedidoNoRabbitMQ(PedidoMocks.PedidoPendente());
         await publisher.PublicarPedidoNoRabbitMQ(PedidoMocks.PedidoProcessado());
         Assert.AreEqual(2, publisher.MensagensPublicadas.Count);
      }

   }
}

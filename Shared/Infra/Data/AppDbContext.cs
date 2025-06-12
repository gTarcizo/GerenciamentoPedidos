using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.EnumsSistema;

namespace Shared.Infra.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{

   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

   public DbSet<Pedido> Pedidos => Set<Pedido>();
   public DbSet<StatusPedido> StatusPedidos => Set<StatusPedido>();
   public DbSet<ItemPedido> Itens => Set<ItemPedido>();
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      #region Pedido
      builder.Entity<Pedido>(pedido =>
      {
         pedido.ToTable("Pedidos");
         pedido.HasKey(e => e.Id);
         pedido.Property(e => e.Cliente).IsRequired();
         pedido.Property(e => e.Total).IsRequired();
         pedido.Property(e => e.DataCriacao).IsRequired();
         pedido.Property(e => e.Status).HasConversion<int>().IsRequired();
         pedido.HasOne(e => e.StatusPedido).WithMany(s => s.Pedidos).HasForeignKey(e => (int)e.Status).HasConstraintName("FK_Pedidos_StatusPedido");
         pedido.HasMany(p => p.Itens)
              .WithOne(i => i.Pedido)
              .HasForeignKey(i => i.PedidoId);
      });
      #endregion

      #region Itens
      builder.Entity<ItemPedido>(item =>
      {
         item.HasKey(i => i.Id);
         item.Property(i => i.Nome).IsRequired().HasMaxLength(200);
         item.Property(i => i.Quantidade).IsRequired();
         item.Property(i => i.PrecoUnitario).IsRequired();
         item.HasOne(e => e.Pedido).WithMany(s => s.Itens).HasForeignKey(e => e.PedidoId).HasConstraintName("FK_Itens_PedidoId");

      });
      #endregion

      #region StatusPedido
      builder.Entity<StatusPedido>().HasData(
      new StatusPedido { Id = StatusPedidoEnum.Pendente, Status = "Pendente" },
      new StatusPedido { Id = StatusPedidoEnum.Processado, Status = "Processado" });
      #endregion
   }
}

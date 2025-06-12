using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain;

namespace ProductsAPI.Infra.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{

   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

   public DbSet<Pedido> Pedidos => Set<Pedido>();
   public DbSet<StatusPedido> StatusPedidos => Set<StatusPedido>();
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      #region Ignore
      builder.Ignore<Notification>();
      #endregion

      #region Pedido
      builder.Entity<Pedido>(entity =>
      {
         entity.ToTable("Pedidos");
         entity.HasKey(e => e.Id);
         entity.Property(e => e.Cliente).IsRequired();
         entity.Property(e => e.Itens).IsRequired();
         entity.Property(e => e.TotalPedido).IsRequired();
         entity.Property(e => e.DataCriacao).IsRequired();
         entity.Property(e => e.Status).HasConversion<int>().IsRequired();
         entity.HasOne(e => e.StatusPedido).WithMany(s => s.Pedidos).HasForeignKey(e => (int)e.Status).HasConstraintName("FK_Pedidos_StatusPedido");
      });
      #endregion

      #region StatusPedido
      builder.Entity<StatusPedido>().HasData(
      new StatusPedido { Id = EnumsSistema.StatusPedidoEnum.Pendente, Status = "Pendente" },
      new StatusPedido { Id = EnumsSistema.StatusPedidoEnum.Processado, Status = "Processado" });
      #endregion

   }
}

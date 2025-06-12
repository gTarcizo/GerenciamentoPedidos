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
   public DbSet<ItemPedido> Itens => Set<ItemPedido>();
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      #region Pedido
      builder.Entity<Pedido>(entity =>
      {
         entity.ToTable("Pedidos");
         entity.HasKey(e => e.Id);
         entity.Property(e => e.Cliente).IsRequired();
         entity.Property(e => e.Total).IsRequired();
         entity.Property(e => e.DataCriacao).IsRequired();
         entity.Property(e => e.Status).HasConversion<int>().IsRequired();
         entity.HasOne(e => e.StatusPedido).WithMany(s => s.Pedidos).HasForeignKey(e => (int)e.Status).HasConstraintName("FK_Pedidos_StatusPedido");
         entity.HasMany(p => p.Itens)
              .WithOne(i => i.Pedido)
              .HasForeignKey(i => i.PedidoId);
      });
      #endregion

      #region Itens
      builder.Entity<ItemPedido>(entity =>
      {
         entity.HasKey(i => i.Id);
         entity.Property(i => i.Nome).IsRequired().HasMaxLength(200);
         entity.Property(i => i.Quantidade).IsRequired();
         entity.Property(i => i.PrecoUnitario).IsRequired();
      });
      #endregion

      #region StatusPedido
      builder.Entity<StatusPedido>().HasData(
      new StatusPedido { Id = EnumsSistema.StatusPedidoEnum.Pendente, Status = "Pendente" },
      new StatusPedido { Id = EnumsSistema.StatusPedidoEnum.Processado, Status = "Processado" });
      #endregion
   }
}

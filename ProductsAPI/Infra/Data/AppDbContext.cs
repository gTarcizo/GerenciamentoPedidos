using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain;
using System.Reflection.Emit;

namespace ProductsAPI.Infra.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{

   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

   public DbSet<Pedido> Pedidos => Set<Pedido>();
   public DbSet<StatusPedido> StatusPedidos => Set<StatusPedido>();
   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);

   }
}

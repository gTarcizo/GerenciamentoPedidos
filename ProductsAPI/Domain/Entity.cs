using Flunt.Notifications;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Domain;

public abstract class Entity : Notifiable<Notification>
{
  public Entity()
  {
    DataCriacao = DateTime.Now;
  }
  [Key]
  public int Id { get; set; }
  public DateTime DataCriacao { get; set; }
}

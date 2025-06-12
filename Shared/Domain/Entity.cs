using System.ComponentModel.DataAnnotations;

namespace Shared.Domain;

public abstract class Entity
{
  public Entity()
  {
    DataCriacao = DateTime.Now;
  }
  [Key]
  public int Id { get; set; }
  public DateTime DataCriacao { get; set; }
}

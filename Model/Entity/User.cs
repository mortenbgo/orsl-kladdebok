using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
  public class User
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
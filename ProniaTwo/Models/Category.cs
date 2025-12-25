using ProniaTwo.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ProniaTwo.Models;

public class Category : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}
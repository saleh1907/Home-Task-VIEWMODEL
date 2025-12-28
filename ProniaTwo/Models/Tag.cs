using ProniaTwo.Models.Common;

namespace ProniaTwo.Models;

public class Tag:BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<ProductTag> ProductTags { get; set; } = [];

}

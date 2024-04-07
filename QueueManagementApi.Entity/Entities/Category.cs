namespace QueueManagementApi.Core.Entities;

public class Category : BaseEntity, IAuditable
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int DisplayOrder { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime? UpdatedOn { get; set; }

    // navigation properties
    public virtual ICollection<Exhibit> Exhibits { get; set; }
}
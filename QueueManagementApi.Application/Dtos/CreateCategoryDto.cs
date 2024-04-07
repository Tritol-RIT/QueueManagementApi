namespace QueueManagementApi.Controllers;

public class CreateCategoryDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
}
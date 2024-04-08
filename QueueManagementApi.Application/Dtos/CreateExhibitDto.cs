namespace QueueManagementApi.Application.Dtos;

public class CreateExhibitDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxCapacity { get; set; }
    public int InitialDuration { get; set; }
    public bool InsuranceFormRequired { get; set; }
    public bool AgeRequired { get; set; }
    public int? AgeMinimum { get; set; }
    public int CategoryId { get; set; }
    public List<ExhibitImageDto> ExhibitImages { get; set; }
}
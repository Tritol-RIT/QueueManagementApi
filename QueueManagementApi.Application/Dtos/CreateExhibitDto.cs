using System.ComponentModel.DataAnnotations;

namespace QueueManagementApi.Application.Dtos;

public class CreateExhibitDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime InitialDuration { get; set; }
    public bool InsuranceFormRequired { get; set; }
    public bool AgeRequired { get; set; }
    public string? InsuranceFormFileUrl { get; set; }
}

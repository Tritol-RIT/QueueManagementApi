﻿namespace QueueManagementApi.Application.Dtos;

public class UpdateExhibitDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? MaxCapacity { get; set; }
    public int? InitialDuration { get; set; }
    public bool? InsuranceFormRequired { get; set; }
    public bool? AgeRequired { get; set; }
    public string? InsuranceFormFileUrl { get; set; }
    public List<ExhibitImageDto> ExhibitImages { get; set; }
}

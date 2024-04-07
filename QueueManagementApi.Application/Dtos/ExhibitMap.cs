using CsvHelper.Configuration;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Dtos;

public sealed class ExhibitMap : ClassMap<Exhibit>
{
    public ExhibitMap()
    {
        Map(m => m.Title).Name("Title");
        Map(m => m.Description).Name("Description");
        Map(m => m.MaxCapacity).Name("MaxCapacity", "Maximum # of people at once");
        Map(m => m.InitialDuration).Name("InitialDuration", "Expected duration");
        Map(m => m.InsuranceFormRequired).Name("InsuranceFormRequired", "Is insurance form release required").TypeConverter<BooleanConverter>();
        Map(m => m.AgeRequired).Name("AgeRequired", "Is age required").TypeConverter<BooleanConverter>();
        Map(m => m.AgeMinimum).Name("AgeMinimum", "Age Minimum", "Minimum Age");
    }    
}
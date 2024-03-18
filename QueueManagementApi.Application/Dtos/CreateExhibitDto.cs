using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using QueueManagementApi.Core.Entities;

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
}

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

public class BooleanConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text.ToLower() == "yes")
            return true;
        else        
            return false;
    }
}

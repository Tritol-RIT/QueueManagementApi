using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace QueueManagementApi.Application.Dtos;

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
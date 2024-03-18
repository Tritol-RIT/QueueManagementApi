using Newtonsoft.Json;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.JsonConverters;

public class PagedListConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        // Check if the type is a PagedList<> by looking for the generic type definition
        bool isPagedList = objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(PagedList<>));
        return isPagedList;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("Deserialization not implemented.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
            return;

        Type valueType = value.GetType();
        Type itemType = valueType.GetGenericArguments()[0]; // Get the type of T in PagedList<T>
        Type dtoType = typeof(PagedListDto<>).MakeGenericType(itemType); // Create the generic type of PagedListDto<T>

        // Use reflection to instantiate a PagedListDto<T> with the value
        object? dto = Activator.CreateInstance(dtoType, value);

        // Serialize the PagedListDto<T> instance
        serializer.Serialize(writer, dto);
    }
}


public class PagedListDto<T>
{
    [JsonProperty(Order = 1)]
    public int TotalCount { get; set; }

    [JsonProperty(Order = 2)]
    public int CurrentPage { get; set; }

    [JsonProperty(Order = 3)]
    public bool HasNext { get; }

    [JsonProperty(Order = 4)]
    public int TotalPages { get; private set; }

    [JsonProperty(Order = 5)]
    public int PageSize { get; private set; }

    [JsonProperty(Order = 6)]
    public bool HasPrevious { get; }

    [JsonProperty(Order = 7)]
    public List<T> Items { get; set; }

    public PagedListDto(PagedList<T> pagedList)
    {
        TotalCount = pagedList.TotalCount;
        CurrentPage = pagedList.CurrentPage;
        Items = pagedList.Items;
        TotalPages = pagedList.TotalPages;
        PageSize = pagedList.PageSize;
        HasPrevious = pagedList.HasPrevious;
        HasNext = pagedList.HasNext;
    }
}

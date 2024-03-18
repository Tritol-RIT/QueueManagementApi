namespace QueueManagementApi.Core;


public class PagedList<T> : List<T>
{
    public int CurrentPage { get; }
    public int TotalPages { get;  }
    public int PageSize { get; private set; }
    public int TotalCount { get; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        TotalCount = source.Count();
        PageSize = pageSize > 0 ? pageSize : 10;
        CurrentPage = pageNumber > 1 ? pageNumber : 1;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        AddRange(items);
    }

    public PagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var items = source as IList<T> ?? source.ToList();
        TotalCount = items.Count;
        PageSize = pageSize > 0 ? pageSize : 10;
        CurrentPage = pageNumber > 1 ? pageNumber : 1;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        var enumerableItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        AddRange(enumerableItems);
    }
}

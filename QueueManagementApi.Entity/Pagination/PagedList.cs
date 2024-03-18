using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QueueManagementApi.Core.Pagination;

public class PagedList<T> : IEnumerable<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public List<T> Items { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        TotalCount = source.Count();
        PageSize = pageSize > 0 ? pageSize : 10;
        CurrentPage = pageNumber > 1 ? pageNumber : 1;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        Items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public PagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var items = source as IList<T> ?? source.ToList();
        TotalCount = items.Count;
        PageSize = pageSize > 0 ? pageSize : 10;
        CurrentPage = pageNumber > 1 ? pageNumber : 1;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        Items = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

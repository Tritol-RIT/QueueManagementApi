namespace QueueManagementApi.Core.Pagination;


public static class PaginationExtensions
{
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        // Ensure the pageNumber and pageSize are valid
        if (pageNumber < 1)
        {
            throw new QueueApiException("Page number should be greater than or equal to 1.");
        }

        if (pageSize < 1)
        {
            throw new QueueApiException("Page size should be greater than or equal to 1.");
        }

        return new PagedList<T>(source, pageNumber, pageSize);
    }

    public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        // Ensure the pageNumber and pageSize are valid
        if (pageNumber < 1)
        {
            throw new QueueApiException("Page number should be greater than or equal to 1.");
        }

        if (pageSize < 1)
        {
            throw new QueueApiException("Page size should be greater than or equal to 1.");
        }

        return new PagedList<T>(source, pageNumber, pageSize);
    }

}
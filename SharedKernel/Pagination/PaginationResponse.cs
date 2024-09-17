﻿namespace SharedKernel.Pagination;

public class PaginationResponse<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> items)
    where TEntity : class
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long Count { get; } = count;
    public IEnumerable<TEntity> Items { get; } = items;
}
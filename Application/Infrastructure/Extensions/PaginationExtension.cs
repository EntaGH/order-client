using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Infrastructure.Extensions;

public static class PaginationExtension
{
    public static PaginationResponse<T> ToPagedList<T>(this IEnumerable<T> entities, int current, int pageSize)
    {
        IEnumerable<T> items = entities.Skip((current - 1) * pageSize).Take(pageSize);
        return new PaginationResponse<T>(items, entities.Count(), pageSize, current);
    }

    public static PaginationResponse<TEntity, TMoreInfo> ToPagedList<TEntity, TMoreInfo>(this IEnumerable<TEntity> entities, int current, int pageSize, TMoreInfo moreInfo)
    {
        IEnumerable<TEntity> items = entities.Skip((current - 1) * pageSize).Take(pageSize);
        return new PaginationResponse<TEntity, TMoreInfo>(items, entities.Count(), pageSize, current, moreInfo);
    }

    public static async Task<PaginationResponse<T>> ToPagedListAsync<T>(this IQueryable<T> entities, int current, int pageSize, CancellationToken cancellationToken = default)
    {
        IEnumerable<T> items = await entities.Skip((current - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginationResponse<T>(items, entities.Count(), pageSize, current);
    }

    public static async Task<PaginationResponse<TEntity, TMoreInfo>> ToPagedListAsync<TEntity, TMoreInfo>(this IQueryable<TEntity> entities, int current, int pageSize, TMoreInfo moreInfo, CancellationToken cancellationToken = default)
    {
        IEnumerable<TEntity> items = await entities.Skip((current - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginationResponse<TEntity, TMoreInfo>(items, entities.Count(), pageSize, current, moreInfo);
    }
}

public class PaginationResponse<T>
{
    public PaginationResponse(IEnumerable<T> items, int totalCount, int pageSize, int current)
    {
        PagedData = items;
        PageInfo = new(totalCount, pageSize, current);
    }

    public IEnumerable<T> PagedData { get; set; }

    public PageInfo PageInfo { get; set; }
}

public sealed class PaginationResponse<T, TMoreInfo> : PaginationResponse<T>
{
    public PaginationResponse(IEnumerable<T> items, int totalCount, int pageSize, int current, TMoreInfo moreInfo)
        : base(items, totalCount, pageSize, current)
    {
        MoreInfo = moreInfo;
    }

    public TMoreInfo MoreInfo { get; set; }
}

public class PageInfo
{
    public PageInfo(int totalCount, int pageSize, int current)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        Current = current;
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public int Current { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNext => Current < TotalPages;

    public bool HasPrevious => Current > 1 && Current <= TotalPages;
}

public class QueryContainer : IValidatableObject
{
    public int PageSize { get; set; } = int.MaxValue / 2;

    public int Current { get; set; } = 1;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PageSize <= 0 || PageSize > int.MaxValue / 2)
        {
            yield return new ValidationResult(
                $"{nameof(PageSize)}Invalid",
                new[] { nameof(PageSize) }
            );
        }

        if (Current <= 0 || Current > int.MaxValue / 2)
        {
            yield return new ValidationResult(
                $"{nameof(PageSize)}Invalid",
                new[] { nameof(PageSize) }
            );
        }
    }
}
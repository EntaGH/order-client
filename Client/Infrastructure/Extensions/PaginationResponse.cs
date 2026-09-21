using System.ComponentModel.DataAnnotations;

namespace Client.Infrastructure.Extensions;

public class PaginationResponse<T>
{
    public IEnumerable<T>? PagedData { get; set; }

    public PageInfo? PageInfo { get; set; }
}

public class PageInfo
{
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
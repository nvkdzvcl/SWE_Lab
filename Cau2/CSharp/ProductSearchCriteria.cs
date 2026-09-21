using System.Text.RegularExpressions;

namespace Cau2;

public sealed class ProductSearchCriteria
{
    public string? Keyword { get; }
    public string? Category { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    public int Page { get; }
    public int PageSize { get; }

    private ProductSearchCriteria(Builder builder)
    {
        Keyword = Normalize(builder.KeywordValue);
        Category = Normalize(builder.CategoryValue);
        MinPrice = builder.MinPriceValue;
        MaxPrice = builder.MaxPriceValue;
        Page = builder.PageValue;
        PageSize = builder.PageSizeValue;
    }

    private static string? Normalize(string? value)
    {
        if (value is null) return null;
        var normalized = Regex.Replace(value, @"\s+", " ").Trim().ToLowerInvariant();
        return normalized.Length == 0 ? null : normalized;
    }

    public sealed class Builder
    {
        internal string? KeywordValue { get; private set; }
        internal string? CategoryValue { get; private set; }
        internal decimal? MinPriceValue { get; private set; }
        internal decimal? MaxPriceValue { get; private set; }
        internal int PageValue { get; private set; } = 1;
        internal int PageSizeValue { get; private set; } = 5;

        public Builder Keyword(string? value) { KeywordValue = value; return this; }
        public Builder Category(string? value) { CategoryValue = value; return this; }
        public Builder MinPrice(decimal? value) { MinPriceValue = value; return this; }
        public Builder MaxPrice(decimal? value) { MaxPriceValue = value; return this; }
        public Builder Page(int value) { PageValue = value; return this; }
        public Builder PageSize(int value) { PageSizeValue = value; return this; }

        public ProductSearchCriteria Build()
        {
            if (PageValue < 1)
                throw new ArgumentOutOfRangeException(nameof(Page), "Page must be at least 1.");
            if (PageSizeValue is < 1 or > 100)
                throw new ArgumentOutOfRangeException(nameof(PageSize), "PageSize must be between 1 and 100.");
            if (MinPriceValue < 0 || MaxPriceValue < 0)
                throw new ArgumentException("Prices must be non-negative.");
            if (MinPriceValue > MaxPriceValue)
                throw new ArgumentException("MinPrice must not exceed MaxPrice.");
            return new ProductSearchCriteria(this);
        }
    }
}

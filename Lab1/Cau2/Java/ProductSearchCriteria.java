import java.math.BigDecimal;
import java.util.Locale;

public final class ProductSearchCriteria {
    private final String keyword;
    private final String category;
    private final BigDecimal minPrice;
    private final BigDecimal maxPrice;
    private final int page;
    private final int pageSize;

    private ProductSearchCriteria(Builder builder) {
        keyword = normalize(builder.keyword);
        category = normalize(builder.category);
        minPrice = builder.minPrice;
        maxPrice = builder.maxPrice;
        page = builder.page;
        pageSize = builder.pageSize;
    }

    private static String normalize(String value) {
        if (value == null) return null;
        String normalized = value.replaceAll("(?U)\\s+", " ").strip().toLowerCase(Locale.ROOT);
        return normalized.isEmpty() ? null : normalized;
    }

    public String getKeyword() { return keyword; }
    public String getCategory() { return category; }
    public BigDecimal getMinPrice() { return minPrice; }
    public BigDecimal getMaxPrice() { return maxPrice; }
    public int getPage() { return page; }
    public int getPageSize() { return pageSize; }

    public static final class Builder {
        private String keyword;
        private String category;
        private BigDecimal minPrice;
        private BigDecimal maxPrice;
        private int page = 1;
        private int pageSize = 5;

        public Builder keyword(String value) { keyword = value; return this; }
        public Builder category(String value) { category = value; return this; }
        public Builder minPrice(BigDecimal value) { minPrice = value; return this; }
        public Builder maxPrice(BigDecimal value) { maxPrice = value; return this; }
        public Builder page(int value) { page = value; return this; }
        public Builder pageSize(int value) { pageSize = value; return this; }

        public ProductSearchCriteria build() {
            if (page < 1)
                throw new IllegalArgumentException("Page must be at least 1.");
            if (pageSize < 1 || pageSize > 100)
                throw new IllegalArgumentException("PageSize must be between 1 and 100.");
            if ((minPrice != null && minPrice.signum() < 0) ||
                    (maxPrice != null && maxPrice.signum() < 0))
                throw new IllegalArgumentException("Prices must be non-negative.");
            if (minPrice != null && maxPrice != null && minPrice.compareTo(maxPrice) > 0)
                throw new IllegalArgumentException("MinPrice must not exceed MaxPrice.");
            return new ProductSearchCriteria(this);
        }
    }
}

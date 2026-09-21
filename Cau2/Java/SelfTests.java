import java.math.BigDecimal;
import java.util.Locale;

public final class SelfTests {
    public static void main(String[] args) {
        Locale.setDefault(Locale.forLanguageTag("tr-TR"));
        ProductSearchCriteria defaults = new ProductSearchCriteria.Builder().build();
        check(defaults.getPage() == 1 && defaults.getPageSize() == 5, "Default pagination.");
        check(defaults.getKeyword() == null && defaults.getCategory() == null &&
            defaults.getMinPrice() == null && defaults.getMaxPrice() == null, "Optional filters.");

        ProductSearchCriteria.Builder builder = new ProductSearchCriteria.Builder()
            .keyword("  ChAiR  ").category(" FURNITURE ")
            .minPrice(new BigDecimal("500000")).maxPrice(new BigDecimal("5000000"))
            .page(2).pageSize(5);
        ProductSearchCriteria criteria = builder.build();
        check("chair".equals(criteria.getKeyword()) && "furniture".equals(criteria.getCategory()) &&
            criteria.getMinPrice().compareTo(new BigDecimal("500000")) == 0 &&
            criteria.getMaxPrice().compareTo(new BigDecimal("5000000")) == 0 &&
            criteria.getPage() == 2 && criteria.getPageSize() == 5, "Assignment example, locale independent.");
        builder.keyword("Desk").category(null).minPrice(null).maxPrice(null).page(3).pageSize(10);
        ProductSearchCriteria changed = builder.build();
        check("chair".equals(criteria.getKeyword()) && "furniture".equals(criteria.getCategory()) &&
            criteria.getMinPrice().compareTo(new BigDecimal("500000")) == 0 &&
            criteria.getMaxPrice().compareTo(new BigDecimal("5000000")) == 0 &&
            criteria.getPage() == 2 && criteria.getPageSize() == 5, "Built object is independent of builder.");
        check("desk".equals(changed.getKeyword()) && changed.getMinPrice() == null && changed.getPage() == 3,
            "Builder can create another object.");

        ProductSearchCriteria spaces = new ProductSearchCriteria.Builder()
            .keyword("\u00a0 OFFICE\t\n ChAiR \u00a0").category(" \t ").build();
        check("office chair".equals(spaces.getKeyword()) && spaces.getCategory() == null,
            "Trim, collapse whitespace, lowercase and remove empty filters.");
        check(new ProductSearchCriteria.Builder().minPrice(BigDecimal.ZERO)
            .maxPrice(new BigDecimal("0.00")).pageSize(1).build().getMinPrice().signum() == 0,
            "Zero and numerically equal price bounds are valid.");
        check(new ProductSearchCriteria.Builder().pageSize(100).build().getPageSize() == 100,
            "Maximum page size is valid.");
        check(new ProductSearchCriteria.Builder().minPrice(BigDecimal.ONE).build().getMaxPrice() == null,
            "Only minimum price.");
        check(new ProductSearchCriteria.Builder().maxPrice(BigDecimal.ONE).build().getMinPrice() == null,
            "Only maximum price.");

        reject(new ProductSearchCriteria.Builder().page(0));
        reject(new ProductSearchCriteria.Builder().page(-1));
        reject(new ProductSearchCriteria.Builder().pageSize(0));
        reject(new ProductSearchCriteria.Builder().pageSize(101));
        reject(new ProductSearchCriteria.Builder().minPrice(new BigDecimal("-1")));
        reject(new ProductSearchCriteria.Builder().maxPrice(new BigDecimal("-1")));
        reject(new ProductSearchCriteria.Builder().minPrice(BigDecimal.TEN).maxPrice(BigDecimal.ONE));
        System.out.println("PASS: defaults, sample, normalization, immutability and validation.");
    }

    private static void check(boolean condition, String message) {
        if (!condition) throw new AssertionError("FAIL: " + message);
    }

    private static void reject(ProductSearchCriteria.Builder builder) {
        try { builder.build(); }
        catch (IllegalArgumentException expected) { return; }
        throw new AssertionError("FAIL: build must reject invalid criteria.");
    }
}

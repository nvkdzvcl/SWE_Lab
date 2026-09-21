import java.math.BigDecimal;
import java.text.NumberFormat;
import java.util.Locale;

public final class Main {
    public static void main(String[] args) {
        ProductSearchCriteria criteria = new ProductSearchCriteria.Builder()
            .keyword("  ChAiR  ")
            .category("  Furniture  ")
            .minPrice(new BigDecimal("500000"))
            .maxPrice(new BigDecimal("5000000"))
            .page(2)
            .pageSize(5)
            .build();

        NumberFormat money = NumberFormat.getIntegerInstance(Locale.forLanguageTag("vi-VN"));
        System.out.println("=== Tieu chi tim kiem san pham ===");
        System.out.println("Keyword: " + criteria.getKeyword());
        System.out.println("Category: " + criteria.getCategory());
        System.out.println("MinPrice: " + money.format(criteria.getMinPrice()) + " VND");
        System.out.println("MaxPrice: " + money.format(criteria.getMaxPrice()) + " VND");
        System.out.println("Page: " + criteria.getPage());
        System.out.println("PageSize: " + criteria.getPageSize());
    }
}

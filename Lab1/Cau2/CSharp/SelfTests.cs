namespace Cau2;

internal static class SelfTests
{
    public static void Run()
    {
        var defaults = new ProductSearchCriteria.Builder().Build();
        Check(defaults.Page == 1 && defaults.PageSize == 5, "Default pagination.");
        Check(defaults.Keyword is null && defaults.Category is null &&
            defaults.MinPrice is null && defaults.MaxPrice is null, "Optional filters.");

        var builder = new ProductSearchCriteria.Builder()
            .Keyword("  ChAiR  ").Category(" Furniture ")
            .MinPrice(500_000m).MaxPrice(5_000_000m).Page(2).PageSize(5);
        var criteria = builder.Build();
        Check(criteria.Keyword == "chair" && criteria.Category == "furniture" &&
            criteria.MinPrice == 500_000m && criteria.MaxPrice == 5_000_000m &&
            criteria.Page == 2 && criteria.PageSize == 5, "Assignment example.");
        builder.Keyword("Desk").Category(null).MinPrice(null).MaxPrice(null).Page(3).PageSize(10);
        var changed = builder.Build();
        Check(criteria.Keyword == "chair" && criteria.Category == "furniture" &&
            criteria.MinPrice == 500_000m && criteria.MaxPrice == 5_000_000m &&
            criteria.Page == 2 && criteria.PageSize == 5, "Built object is independent of builder.");
        Check(changed.Keyword == "desk" && changed.MinPrice is null && changed.Page == 3,
            "Builder can create another object.");

        var spaces = new ProductSearchCriteria.Builder()
            .Keyword("\u00a0 OFFICE\t\n ChAiR \u00a0").Category(" \t ").Build();
        Check(spaces.Keyword == "office chair" && spaces.Category is null,
            "Trim, collapse whitespace, lowercase and remove empty filters.");
        Check(new ProductSearchCriteria.Builder().MinPrice(0).MaxPrice(0).PageSize(1).Build().MinPrice == 0,
            "Zero and equal price bounds are valid.");
        Check(new ProductSearchCriteria.Builder().PageSize(100).Build().PageSize == 100,
            "Maximum page size is valid.");
        Check(new ProductSearchCriteria.Builder().MinPrice(500_000m).Build().MaxPrice is null,
            "Only minimum price.");
        Check(new ProductSearchCriteria.Builder().MaxPrice(500_000m).Build().MinPrice is null,
            "Only maximum price.");

        Reject(new ProductSearchCriteria.Builder().Page(0));
        Reject(new ProductSearchCriteria.Builder().Page(-1));
        Reject(new ProductSearchCriteria.Builder().PageSize(0));
        Reject(new ProductSearchCriteria.Builder().PageSize(101));
        Reject(new ProductSearchCriteria.Builder().MinPrice(-1));
        Reject(new ProductSearchCriteria.Builder().MaxPrice(-1));
        Reject(new ProductSearchCriteria.Builder().MinPrice(10).MaxPrice(9));
        Console.WriteLine("PASS: defaults, sample, normalization, immutability and validation.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception($"FAIL: {message}");
    }

    private static void Reject(ProductSearchCriteria.Builder builder)
    {
        try { builder.Build(); }
        catch (ArgumentException) { return; }
        throw new Exception("FAIL: Build must reject invalid criteria.");
    }
}

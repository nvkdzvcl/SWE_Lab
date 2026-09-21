using System.Globalization;
using Cau2;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");

if (args is ["--test"])
{
    SelfTests.Run();
    return;
}

var criteria = new ProductSearchCriteria.Builder()
    .Keyword("  ChAiR  ")
    .Category("  Furniture  ")
    .MinPrice(500_000m)
    .MaxPrice(5_000_000m)
    .Page(2)
    .PageSize(5)
    .Build();

Console.WriteLine("=== Tieu chi tim kiem san pham ===");
Console.WriteLine($"Keyword: {criteria.Keyword}");
Console.WriteLine($"Category: {criteria.Category}");
Console.WriteLine($"MinPrice: {criteria.MinPrice:N0} VND");
Console.WriteLine($"MaxPrice: {criteria.MaxPrice:N0} VND");
Console.WriteLine($"Page: {criteria.Page}");
Console.WriteLine($"PageSize: {criteria.PageSize}");

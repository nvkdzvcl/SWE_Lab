using System.Globalization;
using Cau1;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");

if (args is ["--test"])
{
    SelfTests.Run();
    return;
}

var repository = new InMemoryOrderRepository();
var notification = new ConsoleNotificationService();
var service = new OrderService(repository, new ReferralRewardPolicy(), notification);
var customer = new Customer("Khach hang");
var referrer = new Customer("Nguoi gioi thieu");
var order = new Order("DH001", 1_500_000m, customer, referrer);

Console.WriteLine("=== Hoan tat don da thanh toan ===");
order.Pay();
service.CompleteOrder(order);
Console.WriteLine($"Trang thai da luu: {repository.FindById(order.Id)!.Status}");
Console.WriteLine($"Tien thuong nguoi gioi thieu: {referrer.RewardBalance:N0} VND");

Console.WriteLine("\n=== Tu choi don chua thanh toan ===");
var unpaidOrder = new Order("DH002", 1_500_000m, customer, referrer);
try
{
    service.CompleteOrder(unpaidOrder);
}
catch (InvalidOperationException exception)
{
    Console.WriteLine($"Tu choi: {exception.Message}");
}
Console.WriteLine($"Trang thai: {unpaidOrder.Status}");
Console.WriteLine($"Tien thuong van la: {referrer.RewardBalance:N0} VND");

Console.WriteLine("\n=== Don khong co nguoi gioi thieu ===");
var noReferrerOrder = new Order("DH003", 1_500_000m, customer);
noReferrerOrder.Pay();
service.CompleteOrder(noReferrerOrder);
Console.WriteLine($"Trang thai: {noReferrerOrder.Status}; thuong gioi thieu: 0 VND");

Console.WriteLine("\n=== Doi chinh sach: khach VIP nhan diem ===");
var vip = new Customer("Khach VIP", isVip: true);
var vipOrder = new Order("DH004", 1_500_000m, vip);
var vipService = new OrderService(repository, new VipPointsRewardPolicy(), notification);
vipOrder.Pay();
vipService.CompleteOrder(vipOrder);
Console.WriteLine($"Trang thai: {vipOrder.Status}; diem VIP: {vip.Points}");

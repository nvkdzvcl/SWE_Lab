namespace Cau1;

internal static class SelfTests
{
    public static void Run()
    {
        var buyer = new Customer("Buyer");
        var referrer = new Customer("Referrer");
        var repository = new InMemoryOrderRepository();
        var notification = new RecordingNotification();
        var service = new OrderService(repository, new ReferralRewardPolicy(), notification);
        var order = new Order("paid", 1_500_000m, buyer, referrer);
        order.Pay();
        service.CompleteOrder(order);
        Check(order.Status == OrderStatus.COMPLETED, "Paid order completes.");
        Check(repository.FindById(order.Id)?.Status == OrderStatus.COMPLETED, "Completed order is saved.");
        Check(referrer.RewardBalance == 30_000m, "Referrer receives exactly 30,000 VND.");
        Check(notification.Sent == 1, "Completion sends one notification.");

        ExpectThrows<InvalidOperationException>(() => service.CompleteOrder(order));
        Check(referrer.RewardBalance == 30_000m && notification.Sent == 1,
            "Repeated completion cannot award or notify twice.");

        var unpaid = new Order("unpaid", 1_500_000m, buyer, referrer);
        ExpectThrows<InvalidOperationException>(() => unpaid.Complete());
        ExpectThrows<InvalidOperationException>(() => service.CompleteOrder(unpaid));
        Check(unpaid.Status == OrderStatus.PENDING && repository.FindById(unpaid.Id) is null,
            "Unpaid order remains pending and is not saved.");
        Check(referrer.RewardBalance == 30_000m && notification.Sent == 1,
            "Unpaid order cannot award or notify.");

        var noReferrer = new Order("no-referrer", 1_500_000m, buyer);
        noReferrer.Pay();
        service.CompleteOrder(noReferrer);
        Check(noReferrer.Status == OrderStatus.COMPLETED && notification.Sent == 2,
            "Order without referrer still completes and notifies.");
        Check(referrer.RewardBalance == 30_000m, "No-referrer order does not credit another customer.");

        var vip = new Customer("VIP", isVip: true);
        var vipService = new OrderService(repository, new VipPointsRewardPolicy(), notification);
        var vipOrder = new Order("vip", 1_509_999m, vip);
        vipOrder.Pay();
        vipService.CompleteOrder(vipOrder);
        Check(vip.Points == 150 && vip.RewardBalance == 0m,
            "VIP policy grants whole points, not money, through the same service.");
        var regularOrder = new Order("regular", 1_500_000m, buyer);
        regularOrder.Pay();
        vipService.CompleteOrder(regularOrder);
        Check(buyer.Points == 0, "VIP policy grants no points to a regular customer.");

        ExpectThrows<ArgumentOutOfRangeException>(() => new Order("invalid", -1m, buyer));
        ExpectThrows<ArgumentOutOfRangeException>(() => new Order("zero", 0m, buyer));
        ExpectThrows<ArgumentException>(() => new Order(" ", 1m, buyer));
        ExpectThrows<ArgumentNullException>(() => new Order("null", 1m, null!));
        ExpectThrows<ArgumentOutOfRangeException>(() => referrer.CreditReward(-1m));
        ExpectThrows<ArgumentOutOfRangeException>(() => vip.AddPoints(-1));
        Console.WriteLine("PASS: completion, persistence, rewards, invalid states, VIP and input validation.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"FAIL: {message}");
    }

    private static void ExpectThrows<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new Exception($"FAIL: Expected {typeof(T).Name}.");
    }

    private sealed class RecordingNotification : NotificationService
    {
        public int Sent { get; private set; }
        public void NotifyCompleted(Order order) => Sent++;
    }
}

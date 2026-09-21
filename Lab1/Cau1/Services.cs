namespace Cau1;

public interface OrderRepository
{
    void Save(Order order);
}

public interface RewardPolicy
{
    void Apply(Order order);
}

public interface NotificationService
{
    void NotifyCompleted(Order order);
}

public sealed class OrderService
{
    private readonly OrderRepository repository;
    private readonly RewardPolicy rewardPolicy;
    private readonly NotificationService notification;

    public OrderService(OrderRepository repository, RewardPolicy rewardPolicy,
        NotificationService notification)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.rewardPolicy = rewardPolicy ?? throw new ArgumentNullException(nameof(rewardPolicy));
        this.notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    public void CompleteOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.Complete();
        repository.Save(order);
        rewardPolicy.Apply(order);
        notification.NotifyCompleted(order);
    }
}

public sealed class InMemoryOrderRepository : OrderRepository
{
    // ponytail: single-process demo; use persistent storage and transactions for production.
    private readonly Dictionary<string, Order> orders = new();

    public void Save(Order order) => orders[order.Id] = order;
    public Order? FindById(string id) => orders.GetValueOrDefault(id);
}

public sealed class ReferralRewardPolicy : RewardPolicy
{
    public void Apply(Order order)
    {
        if (order.Referrer is not null)
            order.Referrer.CreditReward(order.Total * 0.02m);
    }
}

public sealed class VipPointsRewardPolicy : RewardPolicy
{
    public void Apply(Order order)
    {
        // Demo rule: one point per full 10,000 VND; the assignment does not specify a rate.
        if (order.Customer.IsVip)
            order.Customer.AddPoints(decimal.ToInt32(decimal.Floor(order.Total / 10_000m)));
    }
}

public sealed class ConsoleNotificationService : NotificationService
{
    public void NotifyCompleted(Order order) =>
        Console.WriteLine($"Thong bao cho {order.Customer.Name}: don {order.Id} da hoan tat.");
}

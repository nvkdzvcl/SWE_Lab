namespace Cau1;

public enum OrderStatus { PENDING, PAID, COMPLETED }

public sealed class Customer
{
    public string Name { get; }
    public bool IsVip { get; }
    public decimal RewardBalance { get; private set; }
    public int Points { get; private set; }

    public Customer(string name, bool isVip = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        IsVip = isVip;
    }

    public void CreditReward(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        RewardBalance += amount;
    }

    public void AddPoints(int points)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(points);
        Points = checked(Points + points);
    }
}

public sealed class Order
{
    public string Id { get; }
    public decimal Total { get; }
    public Customer Customer { get; }
    public Customer? Referrer { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.PENDING;

    public Order(string id, decimal total, Customer customer, Customer? referrer = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(total);
        ArgumentNullException.ThrowIfNull(customer);
        Id = id;
        Total = total;
        Customer = customer;
        Referrer = referrer;
    }

    public void Pay()
    {
        if (Status != OrderStatus.PENDING)
            throw new InvalidOperationException("Chi thanh toan don dang PENDING.");
        Status = OrderStatus.PAID;
    }

    public void Complete()
    {
        if (Status != OrderStatus.PAID)
            throw new InvalidOperationException("Chi hoan tat don da thanh toan (PAID).");
        Status = OrderStatus.COMPLETED;
    }
}

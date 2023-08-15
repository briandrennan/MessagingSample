namespace MessagingSample.Shared;

public record struct UserIdValue(Guid UserId)
{
    public override readonly string ToString() => $"UserId: {UserId}";
}

public record Customer(
    UserIdValue UserId,
    string Name,
    DateOnly JoinedDate);

public record struct CatalogItemIdValue(Guid ItemId)
{
    public override readonly string ToString() => $"Catalog Item: {ItemId}";
}

public record CatalogItem(
    CatalogItemIdValue ItemId,
    string Title,
    string? Description,
    decimal SuggestedRetailPrice)
{
    public CatalogItem(CatalogItemIdValue itemId, string title, decimal suggestedRetailPrice)
        : this(itemId, title, null, suggestedRetailPrice) { }
}

public record DiscountModel(
    string DiscountCode);

public record struct OrderIdValue(Guid OrderId)
{
    public override readonly string ToString() => $"Order: {OrderId}";
}

public record OrderModel(
    OrderIdValue OrderId,
    UserIdValue UserId,
    DateTimeOffset OrderDate,
    IReadOnlyList<CatalogItem> Items,
    DiscountModel? Discount);

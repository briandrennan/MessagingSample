namespace MessagingSample.Database.EF;

public class StoreContent
{
    public int StoreContentId { get; set; }

    public int StoreId { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Modified { get; set; }

    public virtual StoreHeader Store { get; set; } = null!;
}
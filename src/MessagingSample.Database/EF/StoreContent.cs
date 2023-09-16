namespace MessagingSample.Database.EF;

public class StoreContent
{
    public int StoreContentId { get; set; }

    public int StoreId { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Modified { get; set; }
}
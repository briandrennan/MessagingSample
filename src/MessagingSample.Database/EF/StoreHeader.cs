namespace MessagingSample.Database.EF;

public class StoreHeader
{
    public int StoreId { get; set; }

    public string StoreName { get; set; }

    public DateTimeOffset Created { get; set; }

    public List<StoreContent> Contents { get; set; }
}
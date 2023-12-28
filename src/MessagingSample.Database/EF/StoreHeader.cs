namespace MessagingSample.Database.EF;

public class StoreHeader
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

#pragma warning disable CA2227
    public ICollection<StoreContent> Contents { get; set; } = new HashSet<StoreContent>();
#pragma warning restore CA2227
}
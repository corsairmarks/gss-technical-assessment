using Gss.Domain.Item;

namespace Gss.Domain.Bin;

public class BinModel
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public IReadOnlyDictionary<int, int> Items { get; } = new Dictionary<int, int>();
}
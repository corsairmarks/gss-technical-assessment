namespace Gss.Web.Models;

/// <summary>
/// A bin that contains items. Defined in a 3rd-party system named Smart rack.
/// </summary>
public class BinDetailsModel
{
    /// <summary>
    /// The bin's ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// A description of the bin.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// A map of all item keys to the quantity of that item currently in the bin.
    /// </summary>
    public IReadOnlyDictionary<int, int> Items { get; } = new Dictionary<int, int>();
}
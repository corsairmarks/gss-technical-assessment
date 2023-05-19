namespace Gss.Web.Models;

/// <summary>
/// Basic metadata about an item that can be stored in a bin.
/// </summary>
public class ItemDetailsModel
{
    /// <summary>
    /// The item's ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// A description of the item.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    // It does not store quantity but will need to be able to get the total quantity from the bin aggregate when necessary.
}
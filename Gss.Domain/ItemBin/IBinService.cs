namespace Gss.Domain.Bin;

/// <summary>
/// A service for interacting with bins.
/// </summary>
public interface IBinService
{
    /// <summary>
    /// Gets the specified <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin to retrieve.</param>
    /// <returns>The bin if it exists; otherwise, <see langword="null"/>.</returns>
    Task<BinModel> GetBin(int binId);

    /// <summary>
    /// Adds the specified <paramref name="itemId"/> to the <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin to which to add <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item to add to the <paramref name="binId"/>.</param>
    /// <returns>The bin if it exists; otherwise, <see langword="null"/>.</returns>
    Task<BinModel> AddItem(int binId, int itemId);

    /// <summary>
    /// Adjusts the specified <paramref name="itemId"/> to the <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin to which to add <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item to which to add to the <paramref name="binId"/>.</param>
    /// <param name="quantity">The amount of <paramref name="itemId"/> to add (positive) or remove (negative) to the <paramref name="binId"/>.</param>
    /// <returns>The bin if it exists; otherwise, <see langword="null"/>.</returns>
    Task<BinModel> UpdateItemQuantity(int binId, int itemId, int quantity);

    /// <summary>
    /// Removes the specified <paramref name="itemId"/> from the <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin from which to remove <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item to remove from the <paramref name="binId"/>.</param>
    /// <returns>The bin if it exists; otherwise, <see langword="null"/>.</returns>
    Task<BinModel> RemoveItem(int binId, int itemId);

    /// <summary>
    /// Creates a new bin with the specified <paramref name="description"/>.
    /// </summary>
    /// <param name="description">An arbitrary description of the bin.</param>
    /// <returns>A tuple of the ID of the new bin and the bin that was created.</returns>
    // note: if the bin ever has more than 1 static property, then it would be better to use a form of BinCreationModel, and then the BinModel could inherit from it but add item info
    Task<(int BinId, BinModel Bin)> Create(string description);

    // TODO: does Smart Rack ever update or remove bins - and does this application need those capabilities?
}
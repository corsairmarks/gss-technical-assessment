namespace Gss.Domain.Item;

/// <summary>
/// A service for interacting with items.
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Gets the specified <paramref name="itemId"/>.
    /// </summary>
    /// <param name="itemId">The item to retrieve.</param>
    /// <returns>The item if it exists; otherwise, <see langword="null"/>.</returns>
    Task<ItemModel> Get(int itemId);

    /// <summary>
    /// Updates the specified <paramref name="itemId"/> with data from <paramref name="item"/>.
    /// </summary>
    /// <param name="itemId">The item to update with new data from <paramref name="item"/>.</param>
    /// <param name="item">The new data for <paramref name="itemId"/>.</param>
    /// <returns>The updated item if it exists; otherwise, <see langword="null"/>.</returns>
    Task<ItemModel> Update(int itemId, ItemModel item);

    /// <summary>
    /// Creates a new item with data from <paramref name="item"/>.
    /// </summary>
    /// <param name="item">Data for the new item to create.</param>
    /// <returns>A tuple of the new item's ID and the item that was created.</returns>
    Task<(int ItemId, ItemModel Item)> Create(ItemModel item);

    /// <summary>
    /// Deletes the specified <paramref name="itemId"/> from the system.
    /// </summary>
    /// <param name="itemId">The item to delete.</param>
    /// <returns>The item if it existed; otherwise, <see langword="null"/>.</returns>
    // note: what is the expected behavior if an item is attempted to be removed but it still is in one or more bins?
    Task<ItemModel> Delete(int itemId);
}
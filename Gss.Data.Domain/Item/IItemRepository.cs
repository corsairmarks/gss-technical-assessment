namespace Gss.Data.Domain.Item;

/// <summary>
/// A repository for persisting items.
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Gets the specified <paramref name="itemId"/>.
    /// </summary>
    /// <param name="itemId">The item to retrieve.</param>
    /// <returns>The item if it exists; otherwise, <see langword="null"/>.</returns>
    Task<ItemDataModel?> Get(int itemId);

    /// <summary>
    /// Updates the specified <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item to update.</param>
    /// <returns>The updated item if it existed; otherwise, <see langword="null"/>.</returns>
    Task<ItemDataModel?> Update(ItemDataModel item);

    /// <summary>
    /// Creates a new <paramref name="item"/>.
    /// </summary>
    /// <param name="item">Data for the new item to create.</param>
    /// <returns>The item that was created.</returns>
    Task<ItemDataModel> Create(ItemDataModel item);

    /// <summary>
    /// Deletes the specified <paramref name="itemId"/> from the system.
    /// </summary>
    /// <param name="itemId">The item to delete.</param>
    /// <returns>The deleted item if it existed; otherwise, <see langword="null"/>.</returns>
    Task<ItemDataModel?> Delete(int itemId);
}
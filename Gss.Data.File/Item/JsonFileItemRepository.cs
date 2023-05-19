using System.Text.Json;
using Gss.Data.Domain.Item;
using static System.IO.File;

namespace Gss.Data.File.Item;

public class JsonFileItemRepository : IItemRepository, IDisposable
{
    private const string itemFileName = "item.json";

    private static readonly ReaderWriterLockSlim _padlock = new ReaderWriterLockSlim();

    private bool _disposed;

    public async Task<ItemDataModel> Create(ItemDataModel item)
    {
        var items = await ReadItems();
        items.Add(item);
        await WriteItems(items);
        return item;
    }

    public async Task<ItemDataModel?> Delete(int itemId)
    {
        var items = await ReadItems();
        var existingItem = items.SingleOrDefault(i => i.Id == itemId);
        if (existingItem != null)
        {
            items.Remove(existingItem);
            await WriteItems(items);
        }
        return existingItem;
    }

    public async Task<ItemDataModel?> Get(int itemId)
    {
        var items = await ReadItems();
        return items.SingleOrDefault(i => i.Id == itemId);
    }

    public async Task<ItemDataModel?> Update(ItemDataModel item)
    {
        if (itemFileName == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var items = await ReadItems();
        var existingItem = items.SingleOrDefault(i => i.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Description = item.Description;
            await WriteItems(items);
        }
        return item;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _padlock.Dispose();
        }

        _disposed = true;
    }

    public async Task<List<ItemDataModel>> ReadItems()
    {
        List<ItemDataModel>? items = null;
        _padlock.EnterReadLock();
        if (Exists(itemFileName))
        {
            try
            {
                using var fileStream = OpenRead(itemFileName);

                items = await JsonSerializer.DeserializeAsync<List<ItemDataModel>>(fileStream, JsonSerializerOptions.Default);
            }
            finally
            {
                _padlock.ExitReadLock();
            }
        }

        return items ?? new List<ItemDataModel>();
    }

    public async Task WriteItems(List<ItemDataModel> items)
    {
        _padlock.EnterWriteLock();
        try
        {
            using var fileStream = System.IO.File.Open(itemFileName, FileMode.OpenOrCreate, FileAccess.Write);

            await JsonSerializer.SerializeAsync(fileStream, items);
        }
        finally
        {
            _padlock.ExitWriteLock();
        }
    }
}
using System.Net.Mime;
using AutoMapper;
using Gss.Domain.Item;
using Gss.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Gss.Web.App.Controllers;

/// <summary>
/// Endpoints for work with catalog items.
/// </summary>
[ApiController]
// [Authorize]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> _logger;

    private readonly IMapper _mapper;

    private readonly IItemService _itemService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemController" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="mapper">The type mapper.</param>
    /// <param name="itemService">The item service.</param>
    public ItemController(
        ILogger<ItemController> logger,
        IMapper mapper,
        IItemService itemService)
    {
        _logger = logger;
        _mapper = mapper;
        _itemService = itemService;
    }

    // [HttpGet]
    // public IEnumerable<ItemDetailsModel> Get()
    // {
    //     return null;
    // }

    /// <summary>
    /// Gets the <paramref name="itemId"/> from the catalog.
    /// </summary>
    /// <param name="itemId">The item to retrieve.</param>
    /// <returns>Details about the item.</returns>
    /// <response code="200">The item was found.</response>
    /// <response code="404">The item does not exist.</response>
    [HttpGet("{itemId}")]
    [ProducesResponseType(typeof(ItemDetailsModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IStatusCodeActionResult> Get(int itemId)
    {
        var item = await _itemService.Get(itemId);
        return item != null
            ? Ok(_mapper.Map<ItemDetailsModel>(item))
            : NotFound();
    }

    /// <summary>
    /// Create an item in the catalog using the data from <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The data for the new item.</param>
    /// <returns>Details about the item that was created.</returns>
    /// <response code="201">The item was created.</response>
    /// <response code="400">The new item data was invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ItemDetailsModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IStatusCodeActionResult> Create(ItemDetailsModel item)
    {
        // TODO: other validation on the item, possibly leverage model validation
        if (ModelState.IsValid)
        {
            var (itemId, createdItem) = await _itemService.Create(_mapper.Map<ItemModel>(item));
            return CreatedAtAction(nameof(Get), new { itemId = itemId, }, createdItem!);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Update the item <paramref name="itemId"/> in the catalog using the data from <paramref name="item"/>.
    /// </summary>
    /// <param name="itemId">The item in the catalog to update.</param>
    /// <param name="item">The new data for the <paramref name="itemId"/>.</param>
    /// <returns>Details about the item that was updated.</returns>
    /// <response code="200">The item was updated.</response>
    /// <response code="400">The new item data was invalid.</response>
    /// <response code="404">The item does not exist.</response>
    [HttpPut("{itemId}")]
    [ProducesResponseType(typeof(ItemDetailsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IStatusCodeActionResult> Update(int itemId, ItemDetailsModel item)
    {
        // TODO: other validation on the item, possibly leverage model validation
        if (ModelState.IsValid)
        {
            var updatedItem = await _itemService.Update(itemId, _mapper.Map<ItemModel>(item));
            return item != null
                ? Ok(_mapper.Map<ItemDetailsModel>(updatedItem))
                : NotFound();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Delete the item <paramref name="itemId"/> from the catalog.
    /// </summary>
    /// <param name="itemId">The item to delete from the catalog.</param>
    /// <returns>Details about the item that was deleted.</returns>
    /// <response code="200">The item was deleted.</response>
    /// <response code="404">The item does not exist.</response>
    [HttpDelete("{itemId}")]
    [ProducesResponseType(typeof(ItemDetailsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IStatusCodeActionResult> Delete(int itemId)
    {
        var item = await _itemService.Delete(itemId);
        return item != null
            ? Ok(_mapper.Map<ItemDetailsModel>(item))
            : NotFound();
    }
}
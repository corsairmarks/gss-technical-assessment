using System.Net.Mime;
using AutoMapper;
using Gss.Domain.Bin;
using Gss.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Gss.Web.App.Controllers;

/// <summary>
/// Endpoints for work with catalog bins.
/// </summary>
[ApiController]
// [Authorize]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class BinController : ControllerBase
{
    private readonly ILogger<BinController> _logger;

    private readonly IMapper _mapper;

    private readonly IBinService _binService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BinController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="mapper">The type mapper.</param>
    /// <param name="binService">The bin service.</param>
    public BinController(
        ILogger<BinController> logger,
        IMapper mapper,
        IBinService binService)
    {
        _logger = logger;
        _mapper = mapper;
        _binService = binService;
    }

    // [HttpGet]
    // [ProducesResponseType(typeof(BinDetailsModel[]), StatusCodes.Status200OK)]
    // public IStatusCodeActionResult Get()
    // {
    //     // TODO: does the system need a way to fetch all current bins?
    //     return null;
    // }

    /// <summary>
    /// Get the details about <paramref name="binId"/>.
    /// </summary>
    /// <returns>Details about the bin.</returns>
    /// <response code="200">The bin exists.</response>
    /// <response code="404">The bin does not exist.</response>
    [HttpGet("{binId}")]
    [ProducesResponseType(typeof(BinDetailsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IStatusCodeActionResult> GetBin(int binId)
    {
        var bin = await _binService.GetBin(binId);
        return bin != null
            ? Ok(_mapper.Map<BinDetailsModel>(bin))
            : NotFound();
    }

    /// <summary>
    /// Get the current quantity of <paramref name="itemId"/> in <paramref name="binId"/>.
    /// </summary>
    /// <returns>The current quantity of <paramref name="itemId"/> in <paramref name="binId"/></returns>
    /// <param name="binId">The bin to which to add the <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item to add to the <paramref name="binId"/>.</param>
    /// <response code="200">The item is in the bin.</response>
    /// <response code="404">The bin does not exist.</response>
    /// <response code="409">The item is not in the bin.</response>
    [HttpGet("{binId}/item/{itemId}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BinDetailsModel), StatusCodes.Status409Conflict)]
    public async Task<IStatusCodeActionResult> GetQuantityInBin(int binId, int itemId)
    {
        var bin = await _binService.GetBin(binId);
        return bin != null
            ? bin.Items.TryGetValue(itemId, out var quantity)
                ? Ok(quantity)
                : Conflict(_mapper.Map<BinDetailsModel>(bin))
            : NotFound();
    }

    /// <summary>
    /// Add the item <paramref name="itemId"/> to bin <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin to which to add the <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item to add to the <paramref name="binId"/>.</param>
    /// <returns>Details about the bin to which the item was added.</returns>
    /// <response code="200">The item was added to in the bin, or was already in the bin.</response>
    /// <response code="404">The bin does not exist.</response>
    [HttpPost("{binId}/item/{itemId}")]
    [ProducesResponseType(typeof(BinDetailsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IStatusCodeActionResult> AddToBin(int binId, int itemId)
    {
        // TODO: does this need to validate the item ID is for an item that exists?
        // should this method include a quantity? if not, is a default of 0 or 1 more appropriate?
        // TODO: is it important to the consumer to differentiate whether the item was already in the bin? (e.g. 200 vs 201)
        // or is the expected behavior that this operation will fail if the item is already in the bin? (409 conflict)
        return CreatedAtAction(
            nameof(GetQuantityInBin),
            new { binId = binId, itemId = itemId, },
            _mapper.Map<BinDetailsModel>(await _binService.AddItem(binId, itemId)));
    }

    /// <summary>
    /// Adjust the <paramref name="quantity"/> of item <paramref name="itemId"/> in bin <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin for which to update the <paramref name="quantity"/> of <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item for which to update the <paramref name="quantity"/> in <paramref name="binId"/>.</param>
    /// <param name="quantity">The quantity to add (positive) or remove (negative) of <paramref name="itemId"/> in <paramref name="binId"/>.</param>
    /// <returns>Details about the bin in which the item's <paramref name="quantity"/> was adjusted.</returns>
    /// <response code="200">The item quantity was updated.</response>
    /// <response code="400">The resulting quantity would be negative.</response>
    /// <response code="404">The bin does not exist.</response>
    /// <response code="409">The item is not in the bin.</response>
    [HttpPut("{binId}/item/{itemId}/quantity/{quantity}")]
    [ProducesResponseType(typeof(BinDetailsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BinDetailsModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BinDetailsModel), StatusCodes.Status409Conflict)]
    public async Task<IStatusCodeActionResult> AdjustQuantityInBin(int binId, int itemId, int quantity)
    {
        var bin = await _binService.GetBin(binId);
        if (bin != null)
        {
            return bin.Items.TryGetValue(itemId, out var currentQuantity)
                ? currentQuantity + quantity >= 0
                    ? Ok(_mapper.Map<BinDetailsModel>(_binService.UpdateItemQuantity(binId, itemId, quantity)))
                    : BadRequest(_mapper.Map<BinDetailsModel>(bin))
                : Conflict(_mapper.Map<BinDetailsModel>(bin));
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Remove the item <paramref name="itemId"/> from bin <paramref name="binId"/>.
    /// </summary>
    /// <param name="binId">The bin from which to remove the <paramref name="itemId"/>.</param>
    /// <param name="itemId">The item to remove from the <paramref name="binId"/>.</param>
    /// <returns>Details about the bin from which the item was removed.</returns>
    /// <response code="200">The item was deleted.</response>
    /// <response code="404">The bin does not exist, or the item is not in the bin.</response>
    [HttpDelete("{binId}/item/{itemId}")]
    [ProducesResponseType(typeof(BinDetailsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IStatusCodeActionResult> RemoveFromBin(int binId, int itemId)
    {
        var bin = await _binService.RemoveItem(binId, itemId);
        return bin != null
            ? Ok(_mapper.Map<BinDetailsModel>(bin))
            : NotFound();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Clients;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _itemRepository;
        private readonly IRepository<CatalogItem> catalogItemRepository;

        public ItemsController(IRepository<InventoryItem> itemRepository, IRepository<CatalogItem> catalogItemRepository)
        {
            _itemRepository = itemRepository;
            this.catalogItemRepository = catalogItemRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> Get(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();
            var catalogItems = await catalogItemRepository.GetAllAsync();
            var items = (await _itemRepository.GetAllAsync(item => item.UserId == userId))
            .Select(inventoryItem =>
            {
                var item = catalogItems.SingleOrDefault(s => s.ItemId == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(item.Name, item.Description);
            });

            return items.ToList();
        }
        [HttpPost]
        public async Task<IActionResult> Post(GrantItemsDto grantItemsDto)
        {
            var existingInventoryItem = await _itemRepository.GetAsync(item => item.UserId == grantItemsDto.UserId
            && item.CatalogItemId == grantItemsDto.CatalogItemId);

            if (existingInventoryItem == null)
            {
                await _itemRepository.CreateAsync(new InventoryItem
                {
                    UserId = grantItemsDto.UserId,
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    Quantity = grantItemsDto.Quantity
                });
            }
            else
            {
                existingInventoryItem.Quantity += grantItemsDto.Quantity;
                await _itemRepository.UpdateAsync(existingInventoryItem);
            }
            return Ok();
        }
    }
}

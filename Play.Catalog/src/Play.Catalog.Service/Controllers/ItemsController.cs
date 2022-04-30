using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;
        private readonly IPublishEndpoint publishEndPoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndPoint)
        {
            this.itemsRepository = itemsRepository;
            this.publishEndPoint = publishEndPoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get()
        {
            return Ok((await itemsRepository.GetAllAsync()).Select(item => item.AsDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            var item = (await itemsRepository.GetAsync(id))?.AsDto();
            return item == null ? NotFound() : item;
        }
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto createItemDto)
        {
            var item = new Item { Name = createItemDto.Name, Description = createItemDto.Description, Price = createItemDto.price, CreatedDate = DateTimeOffset.UtcNow };
            await itemsRepository.CreateAsync(item);
            await publishEndPoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateItemDto updateItemDto)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
                return NotFound();


            item.Name = updateItemDto.Name;
            item.Description = updateItemDto.Description;
            item.Price = updateItemDto.price;

            await itemsRepository.UpdateAsync(item);
            await publishEndPoint.Publish(new CatalogItemUpdated(item.Id, item.Name, item.Description));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
                return NotFound();

            await itemsRepository.RemoveAsync(id);
            await publishEndPoint.Publish(new CatalogItemDeleted(item.Id));

            return NoContent();
        }
    }
}
using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> catalogItemRepository;
        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            this.catalogItemRepository = catalogItemRepository;
        }
        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await catalogItemRepository.GetAsync(message.ItemId);

            if (item == null)
                await catalogItemRepository.CreateAsync(message.AsCatalogItem());
                
            await catalogItemRepository.UpdateAsync(message.AsCatalogItem());

        }
    }
}
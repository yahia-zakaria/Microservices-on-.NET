using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> catalogItemRepository;
        public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            this.catalogItemRepository = catalogItemRepository;
        }
        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            var item = await catalogItemRepository.GetAsync(message.ItemId);

            if (item != null)
                return;
                
            await catalogItemRepository.CreateAsync(message.AsCatalogItem());

        }
    }
}
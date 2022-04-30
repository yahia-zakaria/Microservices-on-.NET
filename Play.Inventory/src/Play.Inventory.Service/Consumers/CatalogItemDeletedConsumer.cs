using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> catalogItemRepository;
        public CatalogItemDeeletedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            this.catalogItemRepository = catalogItemRepository;
        }
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await catalogItemRepository.GetAsync(message.ItemId);

            if (item == null)
                return;
                
            await catalogItemRepository.RemoveAsync(message.ItemId);

        }
    }
}
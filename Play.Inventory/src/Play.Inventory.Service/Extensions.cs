using System;
using Play.Catalog.Contracts;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem inventoryItem, string Name, string Description)
        {
            return new InventoryItemDto(inventoryItem.CatalogItemId, Name, Description, inventoryItem.Quantity, inventoryItem.AcquiredDate);
        }

        public static CatalogItem AsCatalogItem(this CatalogItemCreated catalogItemCreated)
        {
            return new CatalogItem { ItemId = catalogItemCreated.ItemId, Name = catalogItemCreated.Name, Description = catalogItemCreated.Description };
        }
        public static CatalogItem AsCatalogItem(this CatalogItemUpdated catalogItemUpdated)
        {
            return new CatalogItem { ItemId = catalogItemUpdated.ItemId, Name = catalogItemUpdated.Name, Description = catalogItemUpdated.Description };
        }
    }
}

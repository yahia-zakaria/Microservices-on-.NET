using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Play.Common;

namespace Play.Inventory.Service.Entities
{
    public class CatalogItem : IEntity
    {
        public Guid Id { get ; set ; }
        public Guid ItemId { get ; set ; }
        public string Name { get ; set ; }
        public string Description { get ; set ; }
    }
}

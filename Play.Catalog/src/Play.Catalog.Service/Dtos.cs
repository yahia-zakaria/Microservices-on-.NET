using System;
namespace Play.Catalog.Service.Dtos{
    public record ItemDto(Guid Id, string Name, string Description, decimal price, DateTimeOffset createdDate);
    public record CreateItemDto(string Name, string Description, decimal price);
    public record UpdateItemDto(string Name, string Description, decimal price);
}
namespace Domain.Entities.Events;

public record ProductCreated(
    Guid Id,
    DateTime CreatedDate,
    string Name,
    int Stock,
    float Price
 );
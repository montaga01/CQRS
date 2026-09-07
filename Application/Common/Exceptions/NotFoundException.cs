namespace Application.Common.Exceptions;

public class NotFoundException (string entity, Guid id) : Exception($"the entity {entity} with id: '{id}' was not found");
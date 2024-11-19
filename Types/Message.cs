namespace NCGraphQL.Types;

public record Message(
    string id, 
    string? campaignCode, 
    string? campaignVariable,
    string? title,
    string? body,
    Cta? cta,
    DateTime? ttl,
    string? createdOn);
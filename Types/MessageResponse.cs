public record MessageResponse(
    string? id,
    string? userId,
    string? body,
    string? title,
    string? campaignCode,
    string? campaignVariant,
    string? createdOn,
    string? ctaUrl,
    string? ctaLabel,
    DateTime? ttl
);
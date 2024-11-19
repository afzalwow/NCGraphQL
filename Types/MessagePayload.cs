public record class MessagePayload(
    string? Id,
    string? UserId,
    string? Body,
    string? Title,
    string? CampaignCode,
    string? CampaignVariant,
    string? CreatedOn,
    string? CtaUrl,
    string? CtaLabel,
    DateTime? Ttl
);
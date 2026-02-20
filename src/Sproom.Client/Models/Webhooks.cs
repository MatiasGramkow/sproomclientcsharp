using System;

namespace Sproom.Client.Models;

public class WebhookDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}

public class WebhookRequest
{
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class GetKeyResponse
{
    public string PublicKey { get; set; } = string.Empty;
}

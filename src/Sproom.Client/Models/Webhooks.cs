using System;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WebhookTypes
{
    Unspecified,
    DocumentStatusChanged,
    DocumentReceived,
    Test,
    PeppolParticipantVerificationChanged,
    ChildCompanyEnrollmentAccepted,
    ReportProcessingCompleted
}

public class WebhookDto
{
    public Guid Id { get; set; }
    public string? PublicKey { get; set; }
    public WebhookTypes Type { get; set; }
    public string? Url { get; set; }
}

public class WebhookRequest
{
    public WebhookTypes Type { get; set; }
    public string Url { get; set; } = string.Empty;
}

public class GetKeyResponse
{
    public string? PublicKey { get; set; }
    public string? SignatureAlgorithm { get; set; }
}

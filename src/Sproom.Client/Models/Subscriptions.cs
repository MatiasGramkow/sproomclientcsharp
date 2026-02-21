using System;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

public class CreateSubscriptionRequest
{
    public string Service { get; set; } = string.Empty;
    public Guid? CompanyId { get; set; }
    public Guid? ServicePayerCompanyId { get; set; }
}

public class CreateSubscriptionResponse
{
    public Guid SubscriptionId { get; set; }
}

public class GetSubscriptionResponse
{
    public Guid Id { get; set; }
    public DateTime StartsOnUtc { get; set; }
    public DateTime EndsOnUtc { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ServicePayerCompanyId { get; set; }
    public string? Service { get; set; }
    [Obsolete("This field is deprecated.")]
    public bool Trial { get; set; }
    [Obsolete("This field is deprecated.")]
    public DateTime? TrialExpiresOnUtc { get; set; }
}

using System;

namespace Sproom.Client.Models;

public class CreateSubscriptionRequest
{
    public string ServiceType { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
    public Guid? PayingCompanyId { get; set; }
}

public class CreateSubscriptionResponse
{
    public Guid SubscriptionId { get; set; }
}

public class GetSubscriptionResponse
{
    public Guid SubscriptionId { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public DateTime SubscribedOnUtc { get; set; }
}

public class SubscriptionError
{
    public string? ErrorCode { get; set; }
    public string? Reason { get; set; }
}

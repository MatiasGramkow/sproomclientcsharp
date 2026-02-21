using System;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

public class ChildCompanyGet
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public OrganizationIdentifierModel OrganizationIdentifier { get; set; } = new();
}

public class ChildCompanyCreate
{
    public string CompanyName { get; set; } = string.Empty;
    public OrganizationIdentifierModel OrganizationIdentifier { get; set; } = new();
    public string? GlnNumber { get; set; }
}

public class ChildCompanyToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName(".issued")]
    public string Issued { get; set; } = string.Empty;

    [JsonPropertyName(".expires")]
    public string Expires { get; set; } = string.Empty;
}

public class PostCompanyEnrollment
{
    public string ChildCompanyName { get; set; } = string.Empty;
    public OrganizationIdentifierModel OrganizationIdentifier { get; set; } = new();
    public string UserEmail { get; set; } = string.Empty;
    public string ParentCompanyName { get; set; } = string.Empty;
    public bool ShouldSendEmail { get; set; }
}

public class EnrollmentSuccessful
{
    public string? EnrollmentLink { get; set; }
}

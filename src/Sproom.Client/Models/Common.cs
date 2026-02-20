using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

public class OrganizationIdentifier
{
    public string SchemeId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class Party
{
    public string? CompanyName { get; set; }
    public OrganizationIdentifier? CompanyIdentifier { get; set; }
}

public class ErrorResponse
{
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
}

public class SproomApiException : Exception
{
    public int StatusCode { get; }
    public string? ErrorCode { get; }

    public SproomApiException(int statusCode, string? errorCode, string? message)
        : base(message ?? $"Sproom API returned status {statusCode}")
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

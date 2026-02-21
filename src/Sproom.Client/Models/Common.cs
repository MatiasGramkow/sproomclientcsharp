using System;

namespace Sproom.Client.Models;

/// <summary>
/// Simple organization identifier used by new registration, child company, and enrollment APIs.
/// </summary>
public class OrganizationIdentifierModel
{
    public string SchemeId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Nested identification scheme used by legacy OrganizationIdentifier (document/party APIs).
/// </summary>
public class IdentificationScheme
{
    public int Icd { get; set; }
    public string? SchemeId { get; set; }
    public string? Name { get; set; }
}

/// <summary>
/// Legacy organization identifier with nested IdentificationScheme, used by document party and deprecated APIs.
/// </summary>
public class OrganizationIdentifier
{
    public IdentificationScheme? SchemeId { get; set; }
    public string? Value { get; set; }
}

/// <summary>
/// Party information on a document (sender or recipient).
/// </summary>
public class ApiDocumentParty
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

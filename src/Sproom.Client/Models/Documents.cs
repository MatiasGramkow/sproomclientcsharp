using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

public class ApiDocument
{
    public Guid DocumentId { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime? IssuedOnUtc { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Currency { get; set; }
    public Party? SenderParty { get; set; }
    public Party? RecipientParty { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApiDocumentFormat
{
    Binary,
    Html,
    Pdf,
    Xml
}

public class DocumentStateEntry
{
    public DateTime? DateTime { get; set; }
    public string? State { get; set; }
    public int? StatusCode { get; set; }
    public string? DeliveryType { get; set; }
    public string? Message { get; set; }
    public List<string>? FailedProperties { get; set; }
}

public class StateChangeRequest
{
    public string State { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

public class ResponseStatus
{
    public string? State { get; set; }
    public string? Reason { get; set; }
    public DateTime? SetOnUtc { get; set; }
}

public class ResponseStateChangeRequest
{
    public string State { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

public class SetStatusResult
{
    public bool IsRequested { get; set; }
}

public class EnrichField
{
    public string Xpath { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class DocumentValidationException
{
    public string? Message { get; set; }
    public List<ValidationError>? Errors { get; set; }
}

public class ValidationError
{
    public string? Context { get; set; }
    public string? Pattern { get; set; }
    public string? Text { get; set; }
    public bool IsWarning { get; set; }
}

public class ConversionErrorResult
{
    public List<string>? SchemaValidationErrors { get; set; }
    public List<string>? SchematronValidationErrors { get; set; }
    public string? ErrorMessage { get; set; }
    public string? TargetFormat { get; set; }
    public string? SourceFormat { get; set; }
    public string? DocumentSenderName { get; set; }
}

using System;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SigningTypes
{
    NemId,
    NorwegianBankId,
    NemIdMoces,
    SwedishBankId,
    AcceptButton
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StateType
{
    Pending,
    Signed,
    Expired,
    Rejected,
    Revoked
}

public class InitiateRequest
{
    public string SignerEmail { get; set; } = string.Empty;
    public string SignerName { get; set; } = string.Empty;
    public SigningTypes SigningMethod { get; set; }
    public string? Cvr { get; set; }
}

public class InitiateResponse
{
    public Guid Id { get; set; }
}

public class VerificationDto
{
    public Guid Id { get; set; }
    public string? SignerEmail { get; set; }
    public string? SignerName { get; set; }
    public string? Cvr { get; set; }
    public SigningTypes SigningMethod { get; set; }
    public DateTime? InitiatedAt { get; set; }
    public DateTime? SignedAt { get; set; }
    public StateType StateType { get; set; }
    public bool CanBeRevoked { get; set; }
}

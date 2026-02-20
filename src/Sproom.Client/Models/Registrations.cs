using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

public class RegistrationModel
{
    public Guid NetworkId { get; set; }
    public OrganizationIdentifier OrganizationIdentifier { get; set; } = new();
    public Guid CompanyId { get; set; }
    public List<RegistryEntry>? RegistryEntries { get; set; }
}

public class RegistryEntry
{
    public string? RegistryType { get; set; }
    public DateTime StartsOnUtc { get; set; }
    public DateTime? ExpiresOnUtc { get; set; }
    public string? SupportedProfileRoles { get; set; }
}

public class NemHandelRegistrationRequest
{
    public OrganizationIdentifier OrganizationIdentifier { get; set; } = new();
}

public class PeppolRegistrationRequest
{
    public OrganizationIdentifier OrganizationIdentifier { get; set; } = new();
}

public class RegistrationResult
{
    public Guid NetworkId { get; set; }
    public string? Message { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NetworkType
{
    NemHandel,
    Peppol
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NetworkModel
{
    NemHandel,
    Peppol
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NemHandelProfile
{
    Nes5Customer,
    BilSimSupplier,
    BilSimCustomer,
    UtsCustomer,
    OrdSimBilSimSupplier,
    OrdSimBilSimRSupplier,
    OrdSimBilSimCustomer,
    OrdSimRBilSimSupplier,
    OrdSimRBilSimRSupplier,
    OrdAdvBilSimCustomer,
    OrdAdvBilSimSupplier,
    OrdSimCustomer,
    OrdSimSupplier,
    OrdSimRSupplier,
    BilSimRCustomer,
    UtsSupplier,
    Nes3Supplier
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PeppolProfile
{
    PeppolBis3Billing
}

public class RegistrationModel
{
    public Guid NetworkId { get; set; }
    public OrganizationIdentifierModel EndpointId { get; set; } = new();
    public NetworkModel Network { get; set; }
    public DateTime RegisteredOnUtc { get; set; }
    public List<string> Profiles { get; set; } = new();
}

public class NemHandelRegistrationModel
{
    public List<NemHandelProfile> Profiles { get; set; } = new();
    public OrganizationIdentifierModel EndpointId { get; set; } = new();
}

public class PeppolRegistrationModel
{
    public List<PeppolProfile> Profiles { get; set; } = new();
    public OrganizationIdentifierModel EndpointId { get; set; } = new();
}

public class RegistrationResult
{
    public Guid NetworkId { get; set; }
}

// ─── Legacy/Deprecated models ────────────────────────────────────

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NetworkType
{
    NemHandel,
    Peppol
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OioublProfileRole
{
    Nes5Customer,
    BilSimSupplier,
    BilSimCustomer,
    UtsCustomer,
    OrdSimBilSimSupplier,
    PeppolBis3Billing,
    OrdSimBilSimRSupplier,
    OrdSimBilSimCustomer,
    OrdSimRBilSimSupplier,
    OrdSimRBilSimRSupplier,
    OrdAdvBilSimCustomer,
    OrdAdvBilSimSupplier,
    OrdSimCustomer,
    OrdSimSupplier,
    OrdSimRSupplier,
    BilSimRCustomer,
    UtsSupplier,
    Nes3Supplier
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CreateRegistrationProfileRole
{
    Nes5Customer,
    BilSimSupplier,
    BilSimCustomer,
    UtsCustomer,
    OrdSimBilSimSupplier
}

public class RegistrationRead
{
    public DateTime? DeletedOnUtc { get; set; }
    public Guid Id { get; set; }
    public OrganizationIdentifier? OrganizationIdentifier { get; set; }
    public Guid CompanyId { get; set; }
    public List<RegistryEntryRead>? RegistryEntries { get; set; }
}

public class RegistryEntryRead
{
    public Guid Id { get; set; }
    public NetworkType Network { get; set; }
    public DateTime StartsOnUtc { get; set; }
    public DateTime? ExpiresOnUtc { get; set; }
    public OioublProfileRole SupportedProfileRoles { get; set; }
}

public class CreateRegistrationRequest
{
    public string SchemeId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<CreateRegistrationProfileRole>? SupportedProfileRoles { get; set; }
}

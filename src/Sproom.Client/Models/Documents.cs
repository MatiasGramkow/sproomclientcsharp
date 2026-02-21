using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sproom.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DocumentRole
{
    ApplicationResponse,
    Catalogue,
    CatalogueDeletion,
    CatalogueItemSpecificationUpdate,
    CataloguePricingUpdate,
    CatalogueRequest,
    CreditNote,
    Invoice,
    Order,
    OrderCancellation,
    OrderChange,
    OrderResponse,
    OrderResponseSimple,
    Reminder,
    Statement,
    UtilityStatement,
    DespatchAdvice,
    MessageLevelResponse,
    InvoiceResponse
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DocumentStatusType
{
    Created,
    EndpointNotFound,
    EndpointAdded,
    SendLimitExceeded,
    SendLimitIncreased,
    IncompletePackage,
    Timeout,
    ReceiveLimitExceeded,
    ReceiveLimitIncreased,
    ReturnedToSchematronEnrichment,
    SchematronEnrichmentIsDone,
    Incomplete,
    IncompleteReturned,
    TransmissionStarted,
    Sent,
    Received,
    TransmissionCompleted,
    PendingApproval,
    Approved,
    Rejected,
    Error,
    ErrorMax,
    ErrorMin,
    OIOSchemaValidationError,
    SchematronValidationError,
    DuplicateFileError,
    SendNemHandelError,
    RuntimeError,
    SendToFinishOperatorError,
    SendSproomError,
    ErrorProcessingAttachments,
    CustomValidationError,
    Canceled,
    Deleted,
    TestModeError,
    SenderMismatchError,
    SendPageroError,
    SendBaswareError,
    SendInExChangeError,
    SendLetterError,
    DeliveryRestrictionError,
    SendIbisticError,
    SendError,
    ApplicationReponseProfileReject,
    ApplicationReponseTechnicalReject,
    CustomerNotSubscribedToBilsim,
    CustomerNotSubscribedToUts,
    ApplicationReponseBusinessReject,
    Internal
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CurrencyCode
{
    Undefined,
    AUD,
    CAD,
    HRK,
    CZK,
    DKK,
    DEM,
    HKD,
    HUF,
    ISK,
    JPY,
    NZD,
    NOK,
    RUB,
    SEK,
    CHF,
    AED,
    GBP,
    USD,
    TRY,
    EUR,
    PLN
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApiDocumentFormat
{
    OioUbl2,
    PeppolBis3
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UpdateableState
{
    TransmissionCompleted,
    ApplicationReponseBusinessReject
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ResponseType
{
    MessageAcknowledgement,
    InProcess,
    UnderQuery,
    ConditionallyAccepted,
    Rejected,
    Accepted,
    PartiallyPaid,
    FullyPaid
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeliveryType
{
    Auto,
    Nemhandel,
    EdirAdapter,
    Email,
    Sproom,
    FinnishOperators,
    Postponed,
    Sftp,
    Pagero,
    Basware,
    SproomConnector,
    InExchange,
    Letter,
    Ibistic,
    EmailB2C,
    LetterB2C,
    EmailInvoiceCopy,
    AutoInvoice,
    NemhandelEDelivery
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DocumentFamily
{
    OioXml,
    OioXmlPaper,
    OioUbl2,
    Pdf,
    SapIDoc,
    E2B,
    Svefaktura,
    ComCare,
    SapIDoc_AirLiquideToVolvo,
    SapIDoc_CelsaSteelService,
    EHF_2_0,
    EHF_1_6,
    MerckSap,
    SapIDoc_Neopost,
    Finvoice_2_1,
    Panduro,
    Kruger,
    WINI,
    Danfoss,
    DubaB8,
    PeppolBis1,
    PeppolBis2,
    Canon,
    GS1,
    EDIFACT,
    SapIDoc_AMBA,
    HTML,
    PeppolBis3
}

public class ApiDocument
{
    public Guid DocumentId { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime? IssuedOnUtc { get; set; }
    public DocumentRole Type { get; set; }
    public DocumentStatusType Status { get; set; }
    public decimal TotalAmount { get; set; }
    public CurrencyCode Currency { get; set; }
    public ApiDocumentParty? SenderParty { get; set; }
    public ApiDocumentParty? RecipientParty { get; set; }
}

public class StateRead
{
    public DateTime DateTime { get; set; }
    public DocumentStatusType State { get; set; }
    public int StatusCode { get; set; }
    public DeliveryType? DeliveryType { get; set; }
    public string? Message { get; set; }
    public List<FailedProperty>? FailedProperties { get; set; }
}

public class FailedProperty
{
    public string? Name { get; set; }
    public string? AttemptedValue { get; set; }
    public List<ValidationRule>? ValidationRules { get; set; }
}

public class ValidationRule
{
    public string? Type { get; set; }
    public List<ErrorMessage>? ErrorMessages { get; set; }
    public Dictionary<string, object>? ValidationParameters { get; set; }
}

public class ErrorMessage
{
    public string? Language { get; set; }
    public string? Message { get; set; }
}

public class StateChange
{
    public UpdateableState? State { get; set; }
    public string? Reason { get; set; }
}

public class ResponseStatus
{
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public DateTime SetOnUtc { get; set; }
}

public class ResponseStateChange
{
    public ResponseType? Response { get; set; }
    public string? Reason { get; set; }
}

public class SetStatusResult
{
    public bool IsRequested { get; set; }
}

public class EnrichField
{
    public string? FieldName { get; set; }
    public string? Value { get; set; }
}

public class ValidationError
{
    public string? Context { get; set; }
    public string? Pattern { get; set; }
    public string? Text { get; set; }
    public string? Xpath { get; set; }
    public bool IsWarning { get; set; }
}

public class DocumentValidationExceptionModel
{
    public string? Message { get; set; }
    public List<ValidationError>? Errors { get; set; }
}

public class ConversionErrorResult
{
    public List<ValidationError>? SchemaValidationErrors { get; set; }
    public List<ValidationError>? SchematronValidationErrors { get; set; }
    public string? ErrorMessage { get; set; }
    public DocumentFamily TargetFormat { get; set; }
    public DocumentFamily SourceFormat { get; set; }
    public string? DocumentSenderName { get; set; }
}

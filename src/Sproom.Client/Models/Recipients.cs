using System.Collections.Generic;

namespace Sproom.Client.Models;

public class LookupResult
{
    public string? OrgId { get; set; }
    public bool CanReceiveInvoices { get; set; }
    public bool CanReceiveCreditNotes { get; set; }
    public List<object>? Endpoints { get; set; }
}

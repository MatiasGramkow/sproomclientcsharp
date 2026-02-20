using System;

namespace Sproom.Client.Models;

public class ReportRequest
{
    public string Format { get; set; } = "json";
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
}

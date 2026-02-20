using System;

namespace Sproom.Client.Models;

public class CompanyDto
{
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public OrganizationIdentifier? OrganizationIdentifier { get; set; }
}

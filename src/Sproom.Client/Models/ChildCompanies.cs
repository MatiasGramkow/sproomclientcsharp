using System;
using System.Collections.Generic;

namespace Sproom.Client.Models;

public class ChildCompanyDto
{
    public Guid ChildCompanyId { get; set; }
    public string? CompanyName { get; set; }
    public OrganizationIdentifier? OrganizationIdentifier { get; set; }
}

public class CreateChildCompanyRequest
{
    public OrganizationIdentifier OrganizationIdentifier { get; set; } = new();
    public string? CompanyName { get; set; }
}

public class ChildCompanyTokenResponse
{
    public string Token { get; set; } = string.Empty;
}

public class EnrollmentRequest
{
    public OrganizationIdentifier OrganizationIdentifier { get; set; } = new();
}

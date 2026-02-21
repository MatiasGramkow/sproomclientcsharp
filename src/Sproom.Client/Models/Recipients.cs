namespace Sproom.Client.Models;

public class LookupResult
{
    public string? OrganizationIdentifier { get; set; }
    public bool? CanRoute { get; set; }
    public string? ErrorMessage { get; set; }
}

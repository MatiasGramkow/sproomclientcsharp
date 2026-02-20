namespace Sproom.Client.Models;

public class InitiateVerificationRequest
{
    public string SignerEmail { get; set; } = string.Empty;
    public string SignerName { get; set; } = string.Empty;
}

public class InitiateVerificationResponse
{
    public Guid VerificationId { get; set; }
}

public class VerificationDto
{
    public Guid Id { get; set; }
    public string? State { get; set; }
    public DateTime InitiatedOnUtc { get; set; }
    public string? SignerName { get; set; }
    public string? SignerEmail { get; set; }
}

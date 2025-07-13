namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Represents a request to authenticate a user in the system.
/// </summary>
public class AuthenticateUserRequest
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// This serves as the username for authentication.
    /// Must be a valid email format.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's password for authentication.
    /// Must match the stored password after hashing.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

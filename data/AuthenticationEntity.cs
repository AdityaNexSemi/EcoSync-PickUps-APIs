public class User
{
    public string Id { get; set; }  // User ID (GUID)
    public string Name { get; set; }  // User Name
    public string Email { get; set; }  // User Email
    public string PasswordHash { get; set; }  // Hashed Password
    public string Role { get; set; }  // User Role
    public string ProfilePicture { get; set; }  // Profile Picture URL (or "none")
    public DateTime CreatedAt { get; set; }  // Account Creation Date
    public DateTime UpdatedAt { get; set; }  // Account Update Date
}

public class CreateUserRequest
{
    public string Id { get; set; }  // User ID passed from the client
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "user";  // Default role to 'user'
    public string ProfilePicture { get; set; } = "none";  // Default profile picture as 'none'
}



public class LoginRequest
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}

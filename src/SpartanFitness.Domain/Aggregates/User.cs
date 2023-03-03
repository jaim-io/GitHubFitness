using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string ProfileImage { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public byte[] Salt { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private User(
        UserId id,
        string firstName,
        string lastName,
        string profileImage,
        string email,
        string password,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        byte[] salt)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        Email = email;
        Password = password;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        Salt = salt;
    }

#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618

    public static User Create(
        string firstName,
        string lastName,
        string profileImage,
        string email,
        string password,
        byte[] salt)
    {
        return new(
            UserId.CreateUnique(),
            firstName,
            lastName,
            profileImage,
            email,
            password,
            DateTime.UtcNow,
            DateTime.UtcNow,
            salt);
    }

    public static User Create(
        User user,
        string? firstName = null,
        string? lastName = null,
        string? profileImage = null,
        string? email = null,
        string? password = null,
        byte[]? salt = null)
    {
        return new(
            user.Id,
            firstName ?? user.FirstName,
            lastName ?? user.LastName,
            profileImage ?? user.ProfileImage,
            email ?? user.Email,
            password ?? user.Password,
            DateTime.UtcNow,
            DateTime.UtcNow,
            salt ?? user.Salt);
    }
}
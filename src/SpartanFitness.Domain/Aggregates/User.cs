using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class User : AggregateRoot<UserId, Guid>
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
    var user = new User(
      UserId.CreateUnique(),
      firstName,
      lastName,
      profileImage,
      email,
      password,
      DateTime.UtcNow,
      DateTime.UtcNow,
      salt);
    
    // user.AddDomainEvent(new ...);
    
    return user;
  }
}
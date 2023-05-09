using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class User : AggregateRoot<UserId, Guid>
{
  private List<ExerciseId> _savedExerciseIds = new();
  private List<MuscleId> _savedMuscleIds = new();
  private List<MuscleGroupId> _savedMuscleGroupIds = new();
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string ProfileImage { get; private set; }
  public string Email { get; private set; }
  public string Password { get; private set; }
  public byte[] Salt { get; private set; }
  public IReadOnlyList<ExerciseId> SavedExerciseIds => _savedExerciseIds.AsReadOnly();
  public IReadOnlyList<MuscleId> SavedMuscleIds => _savedMuscleIds.AsReadOnly();
  public IReadOnlyList<MuscleGroupId> SavedMuscleGroupIds => _savedMuscleGroupIds.AsReadOnly();
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
    byte[] salt,
    List<ExerciseId> savedExerciseIds,
    List<MuscleId> savedMuscleIds,
    List<MuscleGroupId> savedMuscleGroupIds)
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
    _savedExerciseIds = savedExerciseIds;
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
    byte[] salt,
    List<ExerciseId>? savedExerciseIds = null,
    List<MuscleId>? savedMuscleIds = null,
    List<MuscleGroupId>? savedMuscleGroupIds = null)
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
      salt,
      savedExerciseIds ?? new(),
      savedMuscleIds ?? new(),
      savedMuscleGroupIds ?? new());

    // user.AddDomainEvent(new ...);

    return user;
  }
}
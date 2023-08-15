using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Events;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class User : AggregateRoot<UserId, Guid>
{
  private readonly List<ExerciseId> _savedExerciseIds = new();
  private readonly List<MuscleId> _savedMuscleIds = new();
  private readonly List<MuscleGroupId> _savedMuscleGroupIds = new();
  private readonly List<WorkoutId> _savedWorkoutIds = new();
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string ProfileImage { get; private set; }
  public string Email { get; private set; }
  public bool EmailConfirmed { get; private set; }
  public string Password { get; private set; }
  public byte[] Salt { get; private set; }
  public IReadOnlyList<ExerciseId> SavedExerciseIds => _savedExerciseIds.AsReadOnly();
  public IReadOnlyList<MuscleId> SavedMuscleIds => _savedMuscleIds.AsReadOnly();
  public IReadOnlyList<MuscleGroupId> SavedMuscleGroupIds => _savedMuscleGroupIds.AsReadOnly();
  public IReadOnlyList<WorkoutId> SavedWorkoutIds => _savedWorkoutIds.AsReadOnly();
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
    List<MuscleGroupId> savedMuscleGroupIds,
    List<WorkoutId> savedWorkoutIds)
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
    _savedMuscleGroupIds = savedMuscleGroupIds;
    _savedExerciseIds = savedExerciseIds;
    _savedMuscleIds = savedMuscleIds;
    _savedWorkoutIds = savedWorkoutIds;
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
    List<MuscleGroupId>? savedMuscleGroupIds = null,
    List<WorkoutId>? savedWorkoutIds = null)
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
      savedMuscleGroupIds ?? new(),
      savedWorkoutIds ?? new());

    return user;
  }

  public void ConfirmEmail()
  {
    this.AddDomainEvent(new EmailConfirmed(this));
    EmailConfirmed = true;
  }

  public void SaveExercise(ExerciseId id)
  {
    if (!_savedExerciseIds.Contains(id))
    {
      _savedExerciseIds.Add(id);
    }
  }

  public void UnSaveExercise(ExerciseId id)
    => _savedExerciseIds.Remove(id);

  public void UnSaveExerciseRange(List<ExerciseId> ids)
    => _savedExerciseIds.RemoveAll(ids.Contains);

  public void SaveMuscleGroup(MuscleGroupId id)
  {
    if (!_savedMuscleGroupIds.Contains(id))
    {
      _savedMuscleGroupIds.Add(id);
    }
  }

  public void UnSaveMuscleGroup(MuscleGroupId id)
    => _savedMuscleGroupIds.Remove(id);

  public void UnSaveMuscleGroupRange(List<MuscleGroupId> ids)
    => _savedMuscleGroupIds.RemoveAll(ids.Contains);

  public void SaveMuscle(MuscleId id)
  {
    if (!_savedMuscleIds.Contains(id))
    {
      _savedMuscleIds.Add(id);
    }
  }

  public void UnSaveMuscle(MuscleId id)
    => _savedMuscleIds.Remove(id);

  public void UnSaveMuscleRange(List<MuscleId> ids)
    => _savedMuscleIds.RemoveAll(ids.Contains);

  public void SaveWorkout(WorkoutId id)
  {
    if (!_savedWorkoutIds.Contains(id))
    {
      _savedWorkoutIds.Add(id);
    }
  }

  public void UnSaveWorkout(WorkoutId id)
    => _savedWorkoutIds.Remove(id);

  public void UnSaveWorkoutRange(List<WorkoutId> ids)
    => _savedWorkoutIds.RemoveAll(ids.Contains);

  public void SetFirstName(string firstName) => FirstName = firstName;
  public void SetLastName(string lastName) => LastName = lastName;
  public void SetProfileImage(string profileImage) => ProfileImage = profileImage;
  public void SetUpdatedDateTime() => UpdatedDateTime = DateTime.UtcNow;
}
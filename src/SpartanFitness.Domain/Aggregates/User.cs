using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class User : AggregateRoot<UserId, Guid>
{
  private List<ExerciseId> _savedExerciseIds = new();
  private List<MuscleId> _savedMuscleIds = new();
  private List<MuscleGroupId> _savedMuscleGroupIds = new();
  private List<WorkoutId> _savedWorkoutIds = new();
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public string ProfileImage { get; private set; }
  public string Email { get; private set; }
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

    // user.AddDomainEvent(new ...);

    return user;
  }

  public void SaveExercise(Exercise exercise)
  {
    var id = ExerciseId.Create(exercise.Id.Value);
    if (!_savedExerciseIds.Contains(id))
    {
      _savedExerciseIds.Add(id);
    }
  }

  public void UnSaveExercise(Exercise exercise)
  {
    var id = ExerciseId.Create(exercise.Id.Value);
    _savedExerciseIds.Remove(id);
  }

  public void SaveMuscleGroup(MuscleGroup muscleGroup)
  {
    var id = MuscleGroupId.Create(muscleGroup.Id.Value);
    if (!_savedMuscleGroupIds.Contains(id))
    {
      _savedMuscleGroupIds.Add(id);
    }
  }

  public void UnSaveMuscleGroup(MuscleGroup muscleGroup)
  {
    var id = MuscleGroupId.Create(muscleGroup.Id.Value);
    _savedMuscleGroupIds.Remove(id);
  }

  public void SaveMuscle(Muscle muscle)
  {
    var id = MuscleId.Create(muscle.Id.Value);
    if (!_savedMuscleIds.Contains(id))
    {
      _savedMuscleIds.Add(id);
    }
  }

  public void UnSaveMuscle(Muscle muscle)
  {
    var id = MuscleId.Create(muscle.Id.Value);
    _savedMuscleIds.Remove(id);
  }

  public void SaveWorkout(Workout workout)
  {
    var id = WorkoutId.Create(workout.Id.Value);
    if (!_savedWorkoutIds.Contains(id))
    {
      _savedWorkoutIds.Add(id);
    }
  }

  public void UnSaveWorkout(Workout workout)
  {
    var id = WorkoutId.Create(workout.Id.Value);
    _savedWorkoutIds.Remove(id);
  }
}
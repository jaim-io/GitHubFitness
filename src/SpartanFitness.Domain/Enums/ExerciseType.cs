using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.Enums;

public class ExerciseType
  : Enumeration
{
  public static ExerciseType Default = new(0, ExerciseTypes.Default);
  public static ExerciseType Dropset = new(1, ExerciseTypes.Dropset);
  public static ExerciseType Superset = new(2, ExerciseTypes.Superset);

  private ExerciseType(int id, string name)
    : base(id, name)
  {
  }
}

public static class ExerciseTypes
{
  public const string Default = "Default";
  public const string Dropset = "Dropset";
  public const string Superset = "Superset";
}
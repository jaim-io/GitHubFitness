using GitHubFitness.Domain.Common.Models;

namespace GitHubFitness.Domain.Enums;

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

  public static explicit operator ExerciseType(int id)
  {
    var type = typeof(ExerciseType);

    var fieldInfos = type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

    foreach (var fieldInfo in fieldInfos)
    {
      var staticField = (ExerciseType)(fieldInfo.GetValue(null)!);
      if (staticField.Id == id)
      {
        return staticField;
      }
    }

    throw new InvalidCastException("Cast from Int to ExerciseType is not valid: Invalid id-integer");
  }
}

public static class ExerciseTypes
{
  public const string Default = "Default";
  public const string Dropset = "Dropset";
  public const string Superset = "Superset";
}
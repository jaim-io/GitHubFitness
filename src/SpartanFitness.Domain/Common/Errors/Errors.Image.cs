using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Image
  {
    public static Error NotFound => Error.NotFound(
      code: "File.NotFound",
      description: "File not found");
  }
}
using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Page
  {
    public static Error NotFound => Error.NotFound(
      code: "Page.NotFound",
      description: "Page does not exist");
  }
}
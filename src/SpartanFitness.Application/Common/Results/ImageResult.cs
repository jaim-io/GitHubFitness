namespace SpartanFitness.Application.Common.Results;

public record ImageResult(
  byte[] FileContents,
  string ContentType);
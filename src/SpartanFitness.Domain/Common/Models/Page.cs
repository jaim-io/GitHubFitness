namespace SpartanFitness.Domain.Common.Models;

public record Page<T>(
  List<T> Content,
  int PageNumber,
  int PageCount);
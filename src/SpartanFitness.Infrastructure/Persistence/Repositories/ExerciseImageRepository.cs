using Microsoft.AspNetCore.Http;

using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class ExerciseImageRepository
{
  public async Task AddAsync(ExerciseId exerciseId, IFormFile image)
  {
    await Task.CompletedTask;
    return;
  }

  public async Task<(byte[] FileContents, string ContentType)> GetAsync(ExerciseId exerciseId)
  {
    await Task.CompletedTask;
    return (new byte[1], string.Empty);
  }
}
using Microsoft.AspNetCore.Http;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class ImageRepository : IImageRepository
{
  private const string RootPath = "./image";

  public async Task SaveAsync<T>(AggregateRootId<Guid> id, IFormFile image)
    where T : AggregateRootId<Guid>
  {
    var path = GetPath(typeof(T));
    var filePath = $"{path}/{id.Value}";

    using (var stream = File.Create(filePath))
    {
      await image.CopyToAsync(stream);
    }
  }

  public async Task<(byte[] FileContents, string ContentType)> GetAsync<T>(AggregateRootId<Guid> id)
    where T : AggregateRootId<Guid>
  {
    var path = GetPath(typeof(T));
    var filePath = $"{path}/{id.Value}";

    var fileContents = await File.ReadAllBytesAsync(filePath);
    var contentType = Path.GetExtension(filePath);

    return (fileContents, contentType);
  }

  public void Delete<T>(AggregateRootId<Guid> id)
    where T : AggregateRootId<Guid>
  {
    var path = GetPath(typeof(T));
    var filePath = $"{path}/{id.Value}";

    File.Delete(filePath);
  }

  public bool Exists<T>(AggregateRootId<Guid> id)
    where T : AggregateRootId<Guid>
  {
    var path = GetPath(typeof(T));
    var filePath = $"{path}/{id.Value}";

    return Path.Exists(filePath);
  }

  private static string GetPath(Type type)
  {
    string path = RootPath;

    if (type == typeof(UserId))
    {
      path += "/users";
    }
    else if (type == typeof(ExerciseId))
    {
      path += "/exercises";
    }
    else if (type == typeof(MuscleId))
    {
      path += "/muscles";
    }
    else if (type == typeof(MuscleGroupId))
    {
      path += "/muscle-groups";
    }
    else
    {
      throw new ArgumentException("Invalid type");
    }

    if (!Path.Exists(path))
    {
      Directory.CreateDirectory(path);
    }

    return path;
  }
}
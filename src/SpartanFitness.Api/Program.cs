using SpartanFitness.Api;
using SpartanFitness.Application;
using SpartanFitness.Domain;
using SpartanFitness.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddDomain();
}

var app = builder.Build();
{
  app.UseExceptionHandler("/error");

  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spartan Fitness' API V1");
    });
  }

  app.UseCors(policy => policy
    .WithOrigins("http://localhost:8000")
    .AllowAnyMethod()
    .AllowAnyHeader());

  // app.UseHttpsRedirection();
  app.UseAuthentication();
  app.UseAuthorization();
  app.MapControllers();
  app.Run();
}
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .

# For added security, you can opt out of the diagnostic pipeline. When you opt-out this allows the container to run as read-only.
ENV DOTNET_EnableDiagnostics=0

ENTRYPOINT [ "dotnet", "SpartanFitness.Api.dll" ]
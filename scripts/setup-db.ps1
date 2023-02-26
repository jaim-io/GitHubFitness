$connection_string = "Server=localhost;Database=SpartanFitness;User Id=SA;Password=jaim123!;"
$command = "dotnet ef database update -p ./src/SpartanFitness.Infrastructure/ -s ./src/SpartanFitness.Api/ --connection $connection_string"
$seconds = 30

docker compose up -d
Write-Host "Waiting for $seconds before calling '$command' "
Start-Sleep -Seconds 30
Invoke-Expression $command

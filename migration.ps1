# Check if arguments are provided and the number of arguments is sufficient
if (-not $args -or $args.length -lt 2) {
    Write-Host "Provide execution type and project name: ./migration.ps1 execution_type project_name migration_name"
    Write-Host "Example for 'add' and 'update': ./migration.ps1 add project_name migration_name"
    Write-Host "Example for 'remove': ./migration.ps1 remove project_name"
    Write-Host "Available execution types: 'add', 'remove', 'update'"
} else {
    $execution_type = $args[0]
    $project_name = $args[1]
    $api_project_path = "src/services/$project_name/Ride23.$project_name.API/Ride23.$project_name.API.csproj"
    $infra_project_path = "src/services/$project_name/Ride23.$project_name.Infrastructure/Ride23.$project_name.Infrastructure.csproj"

    # Check if both project paths exist
    if ((Test-Path $api_project_path) -and (Test-Path $infra_project_path)) {
        # Perform action based on the specified execution type
        switch ($execution_type) {
            "add" {
                if ($args.length -lt 3) {
                    Write-Host "Provide migration name for 'add' execution type."
                } else {
                    $migration_name = $args[2]
                    dotnet ef migrations add `
                        -s $api_project_path `
                        -p $infra_project_path `
                        -o Persistence/Migrations `
                        $migration_name 

                    Write-Host "Migration '$migration_name' created successfully for '$project_name' project."
                }
            }
            "remove" {
                dotnet ef migrations remove `
                    -s $api_project_path `
                    -p $infra_project_path

                Write-Host "Last migration removed successfully for '$project_name' project."
            }
            "update" {
                dotnet ef database update `
                    -s $api_project_path `
                    -p $infra_project_path

                Write-Host "Database updated successfully for '$project_name' project."
            }
            default {
                Write-Host "Unknown execution type '$execution_type'."
            }
        }
    } else {
        Write-Host "One or both of the projects for '$project_name' not found in specified location."
    }
}

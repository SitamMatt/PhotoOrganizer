if(Get-Module -Name MDO){

}else{
    $currentDir = Get-Location
    $scriptpath = $MyInvocation.MyCommand.Path
    $dir = Split-Path $scriptpath
    Set-Location -Path $dir

    cd MDO.CLI
    dotnet publish --configuration Debug
    cd bin/Debug/netcoreapp3.0/publish
    $publishPath = Get-Location
    cd ../../../../..
    [Environment]::SetEnvironmentVariable("Path", [Environment]::GetEnvironmentVariable("Path", [EnvironmentVariableTarget]::Machine) + ";$publishPath", [System.EnvironmentVariableTarget]::User)

    If(!(Test-Path -Path $PROFILE)){
        New-Item -Path $PROFILE -Force
    }
    $modulePath = Join-Path -Path $currentDir -ChildPath "MDO.psm1"
    "Import-Module $modulePath" | Out-File -FilePath $PROFILE -Append
    Set-Location -Path $currentDir
}

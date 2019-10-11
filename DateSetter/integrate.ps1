If(!(Test-Path -Path $PROFILE)){
    New-Item -Path $PROFILE -Force
}
Get-Content .\integration.ps1 | Out-File -FilePath $PROFILE -Append
# EnsureDebugModeOff.ps1
param (
    [string]$FilePath
)

if (-Not (Test-Path $FilePath)) {
    Write-Output "File not found: $FilePath"
    exit 1
}

$json = Get-Content $FilePath | ConvertFrom-Json
$json.Debug = $false
$json.SavePath = $null
$json | ConvertTo-Json -Depth 10 | Set-Content $FilePath
$check = Get-Content $FilePath | ConvertFrom-Json
if ($json.Debug) {
    Write-Output "Error saving changes to json file."
    exit 1
}

Write-Output "DebugMode set to false in $FilePath"

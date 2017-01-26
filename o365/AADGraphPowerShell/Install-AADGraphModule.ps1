$myDocumentsModuleFolderIsInPSModulePath = $false
[Environment]::GetEnvironmentVariable("PSModulePath") -Split ';' | % {
  if ($_.ToLower() -eq ([Environment]::GetFolderPath("MyDocuments") + "\WindowsPowerShell\Modules").ToLower()){
    $myDocumentsModuleFolderIsInPSModulePath = $true
  }
}
if(-not $myDocumentsModuleFolderIsInPSModulePath){
  $newPSModulePath = [Environment]::GetEnvironmentVariable("PSModulePath") + ";" + [Environment]::GetFolderPath("MyDocuments") + "\WindowsPowerShell\Modules";
  [Environment]::SetEnvironmentVariable("PSModulePath",$newPSModulePath, "Process")
  [Environment]::SetEnvironmentVariable("PSModulePath",$newPSModulePath, "User")
}

$filesDirPath = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$moduleDirPath = [Environment]::GetFolderPath("MyDocuments") + "\WindowsPowerShell\Modules"
$modulePath = $moduleDirPath + "\AADGraph"

if (Test-Path $modulePath)
{
    Write-Host "Removing existing module directory under "$moduleDirPath -ForegroundColor Green
    Remove-Item -Path $modulePath -Recurse -Force | Out-Null
}

Write-Host "Creating module directory under "$moduleDirPath -ForegroundColor Green
New-Item -Path $modulePath -Type "Directory" -Force | Out-Null
New-Item -Path $modulePath"\Nugets" -Type "Directory" -Force | Out-Null
New-Item -Path $modulePath"\Cmdlets" -Type "Directory" -Force | Out-Null

Write-Host "Installing Active Directory Authentication Library Nuget in " $modulePath"\Nugets" -ForegroundColor Green
Write-Host "Downloading nuget.exe from http://www.nuget.org/nuget.exe" -ForegroundColor Green
$wc = New-Object System.Net.WebClient
$wc.DownloadFile("http://www.nuget.org/nuget.exe",$modulePath + "\Nugets\nuget.exe");
$nugetDownloadExpression = $modulePath + "\Nugets\nuget.exe install Microsoft.IdentityModel.Clients.ActiveDirectory -OutputDirectory " + $modulePath + "\Nugets | out-null"
Invoke-Expression $nugetDownloadExpression

Write-Host "Copying module files to the module directory" -ForegroundColor Green
Copy-Item $filesDirPath"\AADGraph.psd1" -Destination $modulePath -Force 
Copy-Item $filesDirPath"\AADGraph.psm1" -Destination $modulePath -Force 
Copy-Item $filesDirPath"\Cmdlets\*.psm1" -Destination $modulePath"\Cmdlets" -Force 

Import-Module AADGraph

Get-Command -Module AADGraph
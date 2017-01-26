function Load-ActiveDirectoryAuthenticationLibrary(){
  $moduleDirPath = [Environment]::GetFolderPath("MyDocuments") + "\WindowsPowerShell\Modules"
  $modulePath = $moduleDirPath + "\AADGraph"
  if(-not (Test-Path ($modulePath+"\Nugets"))) {New-Item -Path ($modulePath+"\Nugets") -ItemType "Directory" | out-null}
  $adalPackageDirectories = (Get-ChildItem -Path ($modulePath+"\Nugets") -Filter "Microsoft.IdentityModel.Clients.ActiveDirectory*" -Directory)
  if($adalPackageDirectories.Length -eq 0){
    Write-Host "Active Directory Authentication Library Nuget doesn't exist. Downloading now ..." -ForegroundColor Yellow
    if(-not(Test-Path ($modulePath + "\Nugets\nuget.exe")))
    {
      Write-Host "nuget.exe not found. Downloading from http://www.nuget.org/nuget.exe ..." -ForegroundColor Yellow
      $wc = New-Object System.Net.WebClient
      $wc.DownloadFile("http://www.nuget.org/nuget.exe",$modulePath + "\Nugets\nuget.exe");
    }
    $nugetDownloadExpression = $modulePath + "\Nugets\nuget.exe install Microsoft.IdentityModel.Clients.ActiveDirectory -Version 2.14.201151115 -OutputDirectory " + $modulePath + "\Nugets | out-null"
    Invoke-Expression $nugetDownloadExpression
  }
  $adalPackageDirectories = (Get-ChildItem -Path ($modulePath+"\Nugets") -Filter "Microsoft.IdentityModel.Clients.ActiveDirectory*" -Directory)
  $ADAL_Assembly = (Get-ChildItem "Microsoft.IdentityModel.Clients.ActiveDirectory.dll" -Path $adalPackageDirectories[$adalPackageDirectories.length-1].FullName -Recurse)
  $ADAL_WindowsForms_Assembly = (Get-ChildItem "Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll" -Path $adalPackageDirectories[$adalPackageDirectories.length-1].FullName -Recurse)
  if($ADAL_Assembly.Length -gt 0 -and $ADAL_WindowsForms_Assembly.Length -gt 0){
    Write-Host "Loading ADAL Assemblies ..." -ForegroundColor Green
    [System.Reflection.Assembly]::LoadFrom($ADAL_Assembly[0].FullName) | out-null
    [System.Reflection.Assembly]::LoadFrom($ADAL_WindowsForms_Assembly.FullName) | out-null
    return $true
  }
  else{
    Write-Host "Fixing Active Directory Authentication Library package directories ..." -ForegroundColor Yellow
    $adalPackageDirectories | Remove-Item -Recurse -Force | Out-Null
    Write-Host "Not able to load ADAL assembly. Delete the Nugets folder under" $modulePath ", restart PowerShell session and try again ..."
    return $false
  }
}

function Get-AuthenticationResult($tenant = "common", $env="prod"){
  $clientId = "1950a258-227b-4e31-a9cf-717495945fc2"
  $redirectUri = "urn:ietf:wg:oauth:2.0:oob"
  $resourceClientId = "00000002-0000-0000-c000-000000000000"
  $resourceAppIdURI = "https://graph.windows.net/"
  $authority = "https://login.windows.net/" + $tenant
  if($env.ToLower() -eq "ppe"){$resourceAppIdURI = "https://graph.ppe.windows.net/"; $authority = "https://login.windows-ppe.net/" + $tenant}
  elseif($env.ToLower() -eq "china"){$resourceAppIdURI = "https://graph.chinacloudapi.cn/"; $authority = "https://login.chinacloudapi.cn/" + $tenant}
  $authContext = New-Object "Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext" -ArgumentList $authority,$false
  $authResult = $authContext.AcquireToken($resourceAppIdURI, $clientId, $redirectUri, [Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior]::Always)
  return $authResult
}

function Connect-AAD ($tenant = "common", $env="prod", $graphVer="1.5") {
  PROCESS {
    $global:aadGPoShAuthResult = $null
    $global:aadGPoShEnv = $env
    $global:aadGPoShGraphVer = $graphVer
    $global:aadGPoShGraphUrl = "https://graph.windows.net/"
    if($env.ToLower() -eq "ppe") {$global:aadGPoShGraphUrl = "https://graph.ppe.windows.net/"}
    elseif($env.ToLower() -eq "china") {$global:aadGPoShGraphUrl = "https://graph.chinacloudapi.cn/"}
    $global:aadGPoShAuthResult = Get-AuthenticationResult -Tenant $tenant -Env $env
  }
}


function Execute-AADQuery ($Base, $HTTPVerb, $Query, $Data, [switch] $Silent) {
  $return = $null
  if($global:aadGPoShAuthResult -ne $null) {
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $headers = @{"Authorization"=$header;"Content-Type"="application/json"}
    $uri = [string]::Format("{0}{1}/{2}?api-version={3}{4}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId, $base, $global:aadGPoShGraphVer, $query)
    if($data -ne $null){
      $enc = New-Object "System.Text.ASCIIEncoding"
      $body = ConvertTo-Json -InputObject $Data -Depth 10
      $byteArray = $enc.GetBytes($body)
      $contentLength = $byteArray.Length
      $headers.Add("Content-Length",$contentLength)
    }
    if(-not $Silent){
      Write-Host HTTP $HTTPVerb $uri -ForegroundColor Cyan
      Write-Host
    }
    
    $headers.GetEnumerator() | % {
      if(-not $Silent){
        Write-Host $_.Key: $_.Value -ForegroundColor Cyan
        }
      }
    if($data -ne $null){
      if(-not $Silent){
        Write-Host
        Write-Host $body -ForegroundColor Cyan
      }
    }
    $result = Invoke-WebRequest -Method $HTTPVerb -Uri $uri -Headers $headers -Body $body
    if($result.StatusCode -ge 200 -and $result.StatusCode -le 399){
      if(-not $Silent){
        Write-Host
        Write-Host "Query successfully executed." -ForegroundColor Cyan
      }
      if($result.Content -ne $null){
        $json = (ConvertFrom-Json $result.Content)
        if($json -ne $null){
          $return = $json
          if($json.value -ne $null){$return = $json.value}
        }
      }
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
  return $return
}

Load-ActiveDirectoryAuthenticationLibrary
function Get-AADObject([string]$Type, [string]$Query="", [switch] $All, [switch] $Silent) {
  $objects = $null
  if($global:aadGPoShAuthResult -ne $null){
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = [string]::Format("{0}{1}/{2}?api-version={3}{4}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type.Trim(),$global:aadGPoShGraphVer,$Query)
    if(-not $Silent){
      Write-Host HTTP GET $uri -ForegroundColor Cyan
    }
    $result = Invoke-WebRequest -Method Get -Uri $uri -Headers @{"Authorization"=$header;"Content-Type"="application/json"}
    if($result.StatusCode -eq 200){
      if(-not $Silent){
        Write-Host "Get succeeded." -ForegroundColor Cyan
      }
      $json = (ConvertFrom-Json $result.Content)
      if($json -ne $null){
        $objects = $json.value
        $nextLink = $json."odata.nextLink"
        if($nextLink -ne $null){
          if($all){
            $getNextPage = $true
            do{
              if(-not $Silent){
                Write-Host "Getting the next page of results." -ForegroundColor Cyan
                Write-Host HTTP GET ($uri + "&" + $nextLink.Split('?')[1]) -ForegroundColor Cyan
              }
              $result = Invoke-WebRequest -Method Get -Uri ($uri + "&" + $nextLink.Split('?')[1]) -Headers @{"Authorization"=$header;"Content-Type"="application/json"}
              if($result.StatusCode -eq 200){
                $json = (ConvertFrom-Json $result.Content)
                if($json -ne $null){
                  $objects += $json.value
                  $nextLink = $json."odata.nextLink"
                  if($nextLink -ne $null){$getNextPage = $true}
                  else{$getNextPage = $false}
                }
              }
            }
            until(-not $getNextPage)
          }
        }
      }
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
  return $objects
}

function Get-AADObjectById([string]$Type, [string]$Id, [switch] $Silent) {
  $object = $null
  if($global:aadGPoShAuthResult -ne $null){
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = [string]::Format("{0}{1}/{2}/{3}?api-version={4}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type.Trim(), $Id.Trim(),$global:aadGPoShGraphVer)
    if(-not $Silent){
      Write-Host HTTP GET $uri -ForegroundColor Cyan
    }
    $result = Invoke-WebRequest -Method Get -Uri $uri -Headers @{"Authorization"=$header;"Content-Type"="application/json"}
    if($result.StatusCode -eq 200)
    {
      if(-not $Silent){
        Write-Host "Get succeeded." -ForegroundColor Cyan
      }
      $object = (ConvertFrom-Json $result.Content)
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
  return $object
}

function New-AADObject([string]$Type, [object]$Object, [switch] $Silent) {
  $newObject = $null
  if($global:aadGPoShAuthResult -ne $null) {
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = [string]::Format("{0}{1}/{2}?api-version={3}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type.Trim(),$global:aadGPoShGraphVer)
    if(-not $Silent){
      Write-Host HTTP POST $uri -ForegroundColor Cyan
    }
    $enc = New-Object "System.Text.ASCIIEncoding"
    $body = ConvertTo-Json -InputObject $Object -Depth 10
    if(-not $Silent){
      Write-Host $body -ForegroundColor Cyan
    }
    $byteArray = $enc.GetBytes($body)
    $contentLength = $byteArray.Length
    $headers = @{"Authorization"=$header;"Content-Type"="application/json";"Content-Length"=$contentLength}
    $result = Invoke-WebRequest -Method Post -Uri $uri -Headers $headers -Body $body
    if($result.StatusCode -eq 201){
      if(-not $Silent){
        Write-Host "Create succeeded." -ForegroundColor Cyan
      }
      $newObject = (ConvertFrom-Json $result.Content)
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD."
  }
  return $newObject
}

function Set-AADObject([string]$Type, [string]$Id, [object]$Object, [switch] $Silent) {
  if($global:aadGPoShAuthResult -ne $null) {
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = [string]::Format("{0}{1}/{2}/{3}?api-version={4}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type.Trim(), $Id.Trim(),$global:aadGPoShGraphVer)
    if(-not $Silent){
      Write-Host HTTP PATCH $uri -ForegroundColor Cyan
    }
    $enc = New-Object "System.Text.ASCIIEncoding"
    $body = ConvertTo-Json -InputObject $Object -Depth 10
    if(-not $Silent){
      Write-Host $body -ForegroundColor Cyan
    }
    $byteArray = $enc.GetBytes($body)
    $contentLength = $byteArray.Length
    $headers = @{"Authorization"=$header;"Content-Type"="application/json";"Content-Length"=$contentLength}
    $result = Invoke-WebRequest -Method Patch -Uri $uri -Headers $headers -Body $body
    if($result.StatusCode -eq 204){
      if(-not $Silent){
        Write-Host "Update succeeded." -ForegroundColor Cyan
      }
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
}

function Remove-AADObject([string]$Type, [string]$Id, [switch] $Silent) {
  if($global:aadGPoShAuthResult -ne $null) {
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = [string]::Format("{0}{1}/{2}/{3}?api-version={4}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type.Trim(), $Id.Trim(),$global:aadGPoShGraphVer)
    if(-not $Silent){
      Write-Host HTTP DELETE $uri -ForegroundColor Cyan
    }
    $headers = @{"Authorization"=$header;"Content-Type"="application/json"}
    $result = Invoke-WebRequest -Method Delete -Uri $uri -Headers $headers
    if($result.StatusCode -eq 204){
      if(-not $Silent){
        Write-Host "Delete succeeded." -ForegroundColor Cyan
      }
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
}

function Get-AADLinkedObject([string]$Type, [string] $Id, [string]$Relationship, [switch]$GetLinksOnly, [switch]$Binary, [switch]$All, [switch]$Silent) {
  $objects = $null
  if($global:aadGPoShAuthResult -ne $null){
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = $null
    if($GetLinksOnly) {
      $uri = [string]::Format("{0}{1}/{2}/{3}/`$links/{4}?api-version={5}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId, $Type, $Id, $Relationship,$global:aadGPoShGraphVer)
    }
    else {
      $uri = [string]::Format("{0}{1}/{2}/{3}/{4}?api-version={5}",$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId, $Type, $Id, $Relationship,$global:aadGPoShGraphVer)
    }
    if(-not $Silent) {
      Write-Host HTTP GET $uri -ForegroundColor Cyan
    }
    $result = Invoke-WebRequest -Method Get -Uri $uri -Headers @{"Authorization"=$header;"Content-Type"="application/json"}
    if($result.StatusCode -eq 200){
      if(-not $Silent) {
        Write-Host "Get succeeded." -ForegroundColor Cyan
      }
      if(-not $Binary) {
        $json = (ConvertFrom-Json $result.Content)
        if($json -ne $null){
          $objects = $json.value
          $nextLink = $json."odata.nextLink"
          if($nextLink -ne $null){
            if($all){
              $getNextPage = $true
              do{
                if(-not $Silent){
                  Write-Host "Getting the next page of results." -ForegroundColor Cyan
                  Write-Host HTTP GET ($uri + "&" + $nextLink.Split('?')[1]) -ForegroundColor Cyan
                }
                $result = Invoke-WebRequest -Method Get -Uri ($uri + "&" + $nextLink.Split('?')[1]) -Headers @{"Authorization"=$header;"Content-Type"="application/json"}
                if($result.StatusCode -eq 200){
                  $json = (ConvertFrom-Json $result.Content)
                  if($json -ne $null){
                    $objects += $json.value
                    $nextLink = $json."odata.nextLink"
                    if($nextLink -ne $null){$getNextPage = $true}
                    else{$getNextPage = $false}
                  }
                }
              }
              until(-not $getNextPage)
            }
          }
        }
      }
      else {
        $objects = $result.Content
      }
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
  return $objects
}

function Set-AADObjectProperty([string]$Type, [string] $Id, [string]$Property, [object]$Value, [bool]$IsLinked, [string]$ContentType, [ValidateSet("PUT", "POST", ignorecase=$true)][string]$HTTPMethod = "PUT", [switch] $Silent) {
  if($global:aadGPoShAuthResult -ne $null) {
    $header = $global:aadGPoShAuthResult.CreateAuthorizationHeader()
    $uri = $null
    if($IsLinked) {
      $uri = [string]::Format('{0}{1}/{2}/{3}/$links/{4}?api-version={5}',$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type, $Id, $Property,$global:aadGPoShGraphVer)
    }
    else {
      $uri = [string]::Format('{0}{1}/{2}/{3}/{4}?api-version={5}',$global:aadGPoShGraphUrl,$global:aadGPoShAuthResult.TenantId,$Type, $Id, $Property,$global:aadGPoShGraphVer)
    }
    
    if(-not $Silent){
      Write-Host HTTP $HTTPMethod.ToUpper() $uri -ForegroundColor Cyan
    }
    $body = $null
    $byteArray = $null
    
    if($contentType.Trim() -eq "" -or $contentType -eq $null -or $contentType.ToLower() -eq "application/json") {
      $contentType = "application/json"
      $enc = New-Object "System.Text.ASCIIEncoding"
      $body = ConvertTo-Json -InputObject $Value -Depth 10
      $byteArray = $enc.GetBytes($body)
      if(-not $Silent){
        Write-Host $body -ForegroundColor Cyan
      }
    }
    elseif ($contentType.ToLower() -eq "image/jpeg" -or $contentType.ToLower() -eq "image/png" -or $contentType.ToLower() -eq "image/gif") {
      $contentType = $contentType.ToLower()
      $body = $Value
      $byteArray = $Value
      if(-not $Silent){
        Write-Host "Body of the request is binary data." -ForegroundColor Cyan
      }
    }
    $contentLength = $byteArray.Length
    $headers = @{"Authorization"=$header;"Content-Type"=$contentType;"Content-Length"=$contentLength}
    $result = Invoke-WebRequest -Method $HTTPMethod -Uri $uri -Headers $headers -Body $body
    if($result.StatusCode -eq 204){
      if(-not $Silent){
        Write-Host "Update succeeded." -ForegroundColor Cyan
      }
    }
  }
  else{
    Write-Host "Not connected to an AAD tenant. First run Connect-AAD." -ForegroundColor Yellow
  }
}
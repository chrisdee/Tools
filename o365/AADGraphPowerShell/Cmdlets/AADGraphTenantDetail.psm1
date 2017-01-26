function Get-AADTenantDetail {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent
  )
  PROCESS {
    if($Silent){Get-AADObject -Type "tenantDetails" -Silent}
    else{Get-AADObject -Type "tenantDetails"}
  }
}

function Set-AADTenantDetail {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$false,
    HelpMessage="A list of additional email addresses for the user.")]
    [string[]]
    $marketingNotificationMails,

    [parameter(Mandatory=$false,
    HelpMessage="A list of additional email addresses for the user.")]
    [string[]]
    $technicalNotificationMails,
    
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent
  )
  PROCESS {
    $updatedTenantDetail = New-Object System.Object
               
    foreach($psbp in $PSBoundParameters.GetEnumerator()){
      $key = $psbp.Key
      $value = $psbp.Value
      if($key -eq "marketingNotificationMails" -or $key -eq "technicalNotificationMails") {
        Add-Member -InputObject $updatedTenantDetail -MemberType NoteProperty -Name $key -Value $value
      }
    }
    if($Silent){Set-AADObject -Type tenantDetails -Object $updatedTenantDetail -Silent}
    else{Set-AADObject -Type tenantDetails -Object $updatedTenantDetail}
  }
}
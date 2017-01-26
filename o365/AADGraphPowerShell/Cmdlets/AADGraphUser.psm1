function Get-AADUser {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$false,
    ValueFromPipeline=$true,
    HelpMessage="Either the ObjectId or the UserPrincipalName of the User.")]
    [string]
    $Id,
    
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent
  )
  PROCESS {
    if($Id -ne $null -and $Id -ne "") {
      if($Silent){Get-AADObjectById -Type "users" -Id $id -Silent}
      else{Get-AADObjectById -Type "users" -Id $id}
    }
    else {
      if($Silent){Get-AADObject -Type "users" -Silent}
      else{Get-AADObject -Type "users"}
    }
  }
}

function New-AADUser {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="Controls whether the new user account is created enabled or disabled. The default value is true.")]
    [bool]
    $accountEnabled = $true, 
    
    [parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The name displayed in the address book for the user.")]
    [string]
    $displayName, 
    
    [parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The email alias of the new user.")]
    [string]
    $mailNickname, 
    
    [parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true,
    HelpMessage="This is the user name that the new user will use for login. By convention, this should map to the user's email name. The general format is alias@domain, where domain must be present in the tenant’s collection of verified domains.")]
    [string]
    $userPrincipalName, 
    
    [parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The display name of the new user.")]
    [string]
    $password, 

    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="Controls whether the new user will be required to change their password at the next interactive login. The default value is true.")]
    [bool]
    $forceChangePasswordNextLogin = $true,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The city in which the user is located.")]
    [string]
    $city,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The country/region in which the user is located.")]
    [string]
    $country,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The name for the department in which the user works.")]
    [string]
    $department,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="Indicates whether this object was synced from the on-premises directory.")]
    [bool]
    $dirSyncEnabled,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The telephone number of the user's business fax machine.")]
    [alias("Fax")]
    [string]
    $facsimileTelephoneNumber,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The given name of the user.")]
    [alias("FirstName")]
    [string]
    $givenName,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The user’s job title.")]
    [string]
    $jobTitle,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The emailaddress for the user, for example, 'jeff@contoso.onmicrosoft.com'.")]
    [alias("Email","EmailAddress")]
    [string]
    $mail,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The primary cellular telephone number for the user.")]
    [string]
    $mobile,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="A list of additional email addresses for the user.")]
    [string[]]
    $otherMails,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="Specifies password policies for the user, with one possible value being 'DisableStrongPassword', which allows weaker passwords than the default policy to be specified.")]
    [ValidateSet("DisableStrongPassword")] 
    [string]
    $passwordPolicies,

    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The office location in the user's place of business.")]
    [alias("Office")]
    [string]
    $physicalDeliveryOfficeName, 
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The postal code in the user's postal address.")]
    [alias("ZipCode")]
    [string]
    $postalCode,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The preferred language for the user.")]
    [string]
    $preferredLanguage,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The state or province in the user's postal address.")]
    [string]
    $state,      

    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The street address in the user's postal address.")]
    [string]
    $streetAddress,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The user's surname (family name or last name).")]
    [alias("LastName","FamilyName")]
    [string]
    $surname,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="The telephone number of the user.")]
    [string]
    $telephoneNumber,
    
    [parameter(Mandatory=$false, ValueFromPipelineByPropertyName=$true,
    HelpMessage="Not sure what this is :).")]
    [string]
    $usageLocation,
    
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent
  )
  PROCESS {
    # Mandatory properties of a new User
    $newUserPasswordProfile = "" | Select password, forceChangePasswordNextLogin
    $newUserPasswordProfile.password = $password
    $newUserPasswordProfile.forceChangePasswordNextLogin = $forceChangePasswordNextLogin
    
    $newUser = "" | Select accountEnabled, displayName, mailNickname, passwordProfile, userPrincipalName
    $newUser.accountEnabled = $accountEnabled
    $newUser.displayName = $displayName
    $newUser.mailNickname = $mailNickname
    $newUser.passwordProfile = $newUserPasswordProfile
    $newUser.userPrincipalName = $userPrincipalName
           
    #Optional parameters/properties
    foreach($psbp in $PSBoundParameters.GetEnumerator()){
      $key = $psbp.Key
      $value = $psbp.Value
      if($key -eq "city" -or $key -eq "country" -or $key -eq "department" -or $key -eq "dirSyncEnabled" -or $key -eq "facsimileTelephoneNumber" -or `
      $key -eq "givenName" -or $key -eq "jobTitle" -or $key -eq "mail" -or $key -eq "mobile" -or $key -eq "otherMails" -or `
      $key -eq "passwordPolicies" -or $key -eq "physicalDeliveryOfficeName" -or $key -eq "postalCode" -or $key -eq "preferredLanguage" -or `
      $key -eq "state" -or $key -eq "streetAddress" -or $key -eq "surname" -or $key -eq "telephoneNumber"  -or $key -eq "usageLocation") {
        if($value -ne "" -and $value -ne $null){
          Add-Member -InputObject $newUser –MemberType NoteProperty –Name $key –Value $value
        }
      }
    }
    if($Silent){New-AADObject -Type users -Object $newUser -Silent}
    else{New-AADObject -Type users -Object $newUser}
  }
}

function Remove-AADUser {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$true,
    ValueFromPipeline=$true,
    HelpMessage="Either the ObjectId or the UserPrincipalName of the User.")]
    [string]
    $Id,
    
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent
  )
  PROCESS {
    if($Silent){Remove-AADObject -Type "users" -Id $id -Silent}
    else{Remove-AADObject -Type "users" -Id $id}
  }
}

function Set-AADUser {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$true,
    ValueFromPipeline=$true,
    HelpMessage="Either the ObjectId or the UserPrincipalName of the User.")]
    [string]
    $Id,
    
    [parameter(Mandatory=$false, 
    HelpMessage="Controls whether the user account is enabled or disabled. The default value is true.")]
    [string]
    $accountEnabled, 
    
    [parameter(Mandatory=$false,
    HelpMessage="The name displayed in the address book for the user. DisplayName can't be cleared on update.")]
    [string]
    $displayName, 
    
    [parameter(Mandatory=$false,
    HelpMessage="The email alias of the user.")]
    [string]
    $mailNickname, 
    
    [parameter(Mandatory=$false,
    HelpMessage="This is the user name that the user will use for login. By convention, this should map to the user's email name. The general format is alias@domain, where domain must be present in the tenant's collection of verified domains.")]
    [string]
    $userPrincipalName, 
    
    [parameter(Mandatory=$false,
    HelpMessage="The password of the user account.")]
    [string]
    $password, 

    [parameter(Mandatory=$false,
    HelpMessage="If a new password is specified, this parameter controls whether the user will be required to change their password at the next interactive login. The default value is true.")]
    [string]
    $forceChangePasswordNextLogin = $true,
    
    [parameter(Mandatory=$false,
    HelpMessage="The city in which the user is located.")]
    [string]
    $city,
    
    [parameter(Mandatory=$false,
    HelpMessage="The country/region in which the user is located.")]
    [string]
    $country,
    
    [parameter(Mandatory=$false,
    HelpMessage="The name for the department in which the user works.")]
    [string]
    $department,
    
    [parameter(Mandatory=$false,
    HelpMessage="Indicates whether this object was synced from the on-premises directory.")]
    [bool]
    $dirSyncEnabled,
    
    [parameter(Mandatory=$false,
    HelpMessage="The telephone number of the user's business fax machine.")]
    [alias("Fax")]
    [string]
    $facsimileTelephoneNumber,
    
    [parameter(Mandatory=$false,
    HelpMessage="The given name of the user.")]
    [alias("FirstName")]
    [string]
    $givenName,
    
    [parameter(Mandatory=$false,
    HelpMessage="The user's job title.")]
    [string]
    $jobTitle,
    
    [parameter(Mandatory=$false,
    HelpMessage="The emailaddress for the user, for example, 'jeff@contoso.onmicrosoft.com'.")]
    [alias("Email","EmailAddress")]
    [string]
    $mail,
    
    [parameter(Mandatory=$false,
    HelpMessage="The primary cellular telephone number for the user.")]
    [string]
    $mobile,
    
    [parameter(Mandatory=$false,
    HelpMessage="A list of additional email addresses for the user.")]
    [string[]]
    $otherMails,
    
    [parameter(Mandatory=$false,
    HelpMessage="Specifies password policies for the user, with one possible value being 'DisableStrongPassword', which allows weaker passwords than the default policy to be specified.")]
    [ValidateSet("DisableStrongPassword")] 
    [string]
    $passwordPolicies,

    [parameter(Mandatory=$false,
    HelpMessage="The office location in the user's place of business.")]
    [alias("Office")]
    [string]
    $physicalDeliveryOfficeName,
    
    [parameter(Mandatory=$false,
    HelpMessage="The postal code in the user's postal address.")]
    [alias("ZipCode")]
    [string]
    $postalCode,
    
    [parameter(Mandatory=$false,
    HelpMessage="The preferred language for the user.")]
    [string]
    $preferredLanguage,
    
    [parameter(Mandatory=$false,
    HelpMessage="The state or province in the user's postal address.")]
    [string]
    $state,      

    [parameter(Mandatory=$false,
    HelpMessage="The street address in the user's postal address.")]
    [string]
    $streetAddress,
    
    [parameter(Mandatory=$false,
    HelpMessage="The user's surname (family name or last name).")]
    [alias("LastName","FamilyName")]
    [string]
    $surname,
    
    [parameter(Mandatory=$false,
    HelpMessage="The telephone number of the user.")]
    [string]
    $telephoneNumber,
    
    [parameter(Mandatory=$false,
    HelpMessage="Not sure what this is :).")]
    [string]
    $usageLocation,
    
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent
  )
  PROCESS {
    $updatedUser = New-Object System.Object
               
    foreach($psbp in $PSBoundParameters.GetEnumerator()){
      $key = $psbp.Key
      $value = $psbp.Value
      if($key -eq "accountEnabled" -or $key -eq "displayName" -or $key -eq "mailNickname" -or $key -eq "userPrincipalName" -or `
      $key -eq "city" -or $key -eq "country" -or $key -eq "department" -or $key -eq "dirSyncEnabled" -or $key -eq "facsimileTelephoneNumber" -or `
      $key -eq "givenName" -or $key -eq "jobTitle" -or $key -eq "mail" -or $key -eq "mobile" -or $key -eq "otherMails" -or `
      $key -eq "passwordPolicies" -or $key -eq "physicalDeliveryOfficeName" -or $key -eq "postalCode" -or $key -eq "preferredLanguage" -or `
      $key -eq "state" -or $key -eq "streetAddress" -or $key -eq "surname" -or $key -eq "telephoneNumber" -or $key -eq "usageLocation") {
        Add-Member -InputObject $updatedUser -MemberType NoteProperty -Name $key -Value $value
      }
    }
    if($PSBoundParameters.ContainsKey('password')){
      $updatedUserPasswordProfile = "" | Select password, forceChangePasswordNextLogin
      $updatedUserPasswordProfile.password = $PSBoundParameters['password'].Value
      $updatedUserPasswordProfile.forceChangePasswordNextLogin = $forceChangePasswordNextLogin
      $updatedUser.passwordProfile = $updatedUserPasswordProfile
    }
    if($Silent){Set-AADObject -Type users -Id $Id -Object $updatedUser -Silent}
    else{Set-AADObject -Type users -Id $Id -Object $updatedUser}
  }
}

function Set-AADUserThumbnailPhoto {
  [CmdletBinding()]
  param (
    [parameter(Mandatory=$true,
    ValueFromPipeline=$true,
    HelpMessage="Either the ObjectId or the UserPrincipalName of the User.")]
    [string]
    $Id,
    
    #cmdlet allows multiple ways to specify the thumbnail photo, using parameter sets.
    [parameter(Mandatory=$true, 
    HelpMessage="File path of the thumbnail photo of the user.",
    ParameterSetName='FilePath')]
    [ValidateScript({ Test-Path $_})]
    [string]
    $ThumbnailPhotoFilePath,  
   
    [parameter(Mandatory=$true, 
    HelpMessage="Byte array representation of the thumbnail photo of the user.",
    ParameterSetName='ByteArray')]
    [byte[]]
    $ThumbnailPhotoByteArray,
    
    [parameter(Mandatory=$false,
    HelpMessage="Suppress console output.")]
    [switch]
    $Silent  
  )
  PROCESS {
    $value = $null
    if($PSBoundParameters.ContainsKey('ThumbnailPhotoFilePath')){$value = [System.IO.File]::ReadAllBytes($ThumbnailPhotoFilePath)}
    else {$value = $ThumbnailPhotoByteArray}
    if($Silent){Set-AADObjectProperty -Type "users" -Id $Id -Property "thumbnailPhoto" -Value $value -IsLinked $false -ContentType "image/jpeg" -Silent}
    else{Set-AADObjectProperty -Type "users" -Id $Id -Property "thumbnailPhoto" -Value $value -IsLinked $false -ContentType "image/jpeg"}
  }
}

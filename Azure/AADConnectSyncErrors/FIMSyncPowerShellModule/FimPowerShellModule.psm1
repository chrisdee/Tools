$ProgressPreference = "SilentlyContinue"
###
### Load the FIMAutomation Snap-In
###
if(-not (Get-PSSnapin | Where-Object {$_.Name -eq 'FIMAutomation'})) 
{
	try 
	{
		Add-PSSnapin FIMAutomation -ErrorAction SilentlyContinue -ErrorVariable err		
	}
	catch 
	{
	}

	if ($err) 
    {
    	if($err[0].ToString() -imatch "has already been added") 
		{
			Write-Verbose "FIMAutomation snap-in has already been loaded." 
		}
		else
		{
			Write-Error "FIMAutomation snap-in could not be loaded." 
		}
	}
	else
	{
		Write-Verbose "FIMAutomation snap-in loaded successfully." 
	}
}

function New-FimImportObject
{
<#
	.SYNOPSIS 
	Creates a new ImportObject for the FIM Configuration Migration Cmdlets

	.DESCRIPTION
	The New-FimImportObject function makes it easier to use Import-FimConfig by providing an easier way to create ImportObject objects.
	This makes it easier to perform CRUD operations in the FIM Service.
   
	.OUTPUTS
 	the FIM ImportObject is returned by this function.  The next logical step is take this output and feed it to Import-FimConfig.
   
   	.EXAMPLE
	PS C:\$createRequest = New-FimImportObject -ObjectType Person -State Create -Changes @{
		AccountName='Bob' 
		DisplayName='Bob the Builder'
		}
	PS C:\$createRequest | Import-FIMConfig
	
	DESCRIPTION
	-----------
   	Creates an ImportObject for creating a new Person object with AccountName and DisplayName.
	The above sample uses a hashtable for the Changes parameter.
	
	.EXAMPLE
	PS C:\$createRequest = New-FimImportObject -ObjectType Person -State Create -Changes @(
		New-FimImportChange -Operation None -AttributeName 'Bob' -AttributeValue 'foobar' 
		New-FimImportChange -Operation None -AttributeName 'DisplayName' -AttributeValue 'Bob the Builder'  )
	PS C:\$createRequest | Import-FIMConfig
	
	DESCRIPTION
	-----------
   	Creates an ImportObject for creating a new Person object with AccountName and DisplayName.
	The above sample uses an array of ImportChange objects for the Changes parameter.
	
	NOTE: the attribute 'Operation' type of 'None' works when the object 'State' is set to 'Create'.
	
	.EXAMPLE
	PS C:\$updateRequest = New-FimImportObject -ObjectType Person -State Put -AnchorPairs @{AccountName='Bob'} -Changes @(
		New-FimImportChange -Operation Replace -AttributeName 'FirstName' -AttributeValue 'Bob' 
		New-FimImportChange -Operation Replace -AttributeName 'LastName' -AttributeValue 'TheBuilder'  )
	PS C:\$updateRequest | Import-FIMConfig
	
	DESCRIPTION
	-----------
   	Creates an ImportObject for updating an existing Person object with FirstName and LastName.

	.EXAMPLE
	PS C:\$deleteRequest = New-FimImportObject -ObjectType Person -State Delete -AnchorPairs @{AccountName='Bob'} 
	PS C:\$deleteRequest | Import-FIMConfig
	
	DESCRIPTION
	-----------
   	Creates an ImportObject for deleting an existing Person object.	
#>
	param
	( 
	<#
	.PARAMETER ObjectType
	The object type for the target object.
	NOTE: this is case sensitive
	NOTE: this is the ResourceType's 'name' attribute, which often does NOT match what is seen in the FIM Portal.
	#>
	[parameter(Mandatory=$true)] 
	[String]
	$ObjectType,

	<#
	.PARAMETER State
	The operation to perform on the target, must be one of:
	-Create
	-Put
	-Delete
	-Resolve
	-None
	#>
	[parameter(Mandatory=$true)]
    [String]
	[ValidateSet(“Create”, “Put”, “Delete”, "Resolve", "None")]
	$State,

	<#
	.PARAMETER AnchorPairs
 	A name:value pair used to find a target object for Put, Delete and Resolve operations.  The AchorPairs is used in conjunction with the ObjectType by the FIM Import-FimConfig cmdlet to find the target object.
	#>
	[parameter(Mandatory=$false)] 
	[ValidateScript({$_ -is [Hashtable] -or $_ -is [Microsoft.ResourceManagement.Automation.ObjectModel.JoinPair[]] -or $_ -is [Microsoft.ResourceManagement.Automation.ObjectModel.JoinPair]})]
	$AnchorPairs,

	<#	
	.PARAMETER SourceObjectIdentifier
	Not intelligently used or tested yet...  
	#>
	[parameter(Mandatory=$false)] 
	$SourceObjectIdentifier = [Guid]::Empty,

	<#
	.PARAMETER TargetObjectIdentifier
 	The ObjectID of the object to operate on.
	Defaults to an empty GUID
	#>
	[parameter(Mandatory=$false)] 
	$TargetObjectIdentifier = [Guid]::Empty,

	<#
	.PARAMETER Changes
	The changes to make to the target object.  This parameter accepts a Hashtable or FIM ImportChange objects as input. If a Hashtable is supplied as input then it will be converted into FIM ImportChange objects.  You're welcome.
	#>
	[parameter(Mandatory=$false)]
	[ValidateScript({($_ -is [Array] -and $_[0] -is [Microsoft.ResourceManagement.Automation.ObjectModel.ImportChange]) -or $_ -is [Hashtable] -or $_ -is [Microsoft.ResourceManagement.Automation.ObjectModel.ImportChange]})]
	$Changes,
	
	<#
	.PARAMETER ApplyNow
	When specified, will submit the request to FIM
	#>
	[Switch]
	$ApplyNow = $false,
	
	<#
	.PARAMETER PassThru
	When specified, will return the ImportObject as output
	#>
	[Switch]
	$PassThru = $false,    
    
    <#
	.PARAMETER SkipDuplicateCheck
	When specified, will skip the duplicate create request check
	#>
	[Switch]
	$SkipDuplicateCheck = $false,
    
    <#
	.PARAMETER AllowAuthorizationException
	When specified, will swallow Auth Z Exception
	#>
	[Switch]
	$AllowAuthorizationException = $false,    

    <#
	.PARAMETER Uri
	The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	#>
	[String]
	$Uri = "http://localhost:5725"
    		
	) 
	end
	{
       $importObject = New-Object Microsoft.ResourceManagement.Automation.ObjectModel.ImportObject
        $importObject.SourceObjectIdentifier = $SourceObjectIdentifier
        $importObject.TargetObjectIdentifier = $TargetObjectIdentifier
        $importObject.ObjectType = $ObjectType
        $importObject.State = $State
        
        ###
        ### Process the Changes parameter
        ###
        if ($Changes -is [Hashtable])
        {            
            foreach ($c in $changes.Keys)
            {
                try
                {
                    $importObject.Changes += New-FimImportChange -Uri $Uri -AttributeName $c -AttributeValue $changes[$c] -Operation Replace
                }
                catch
                {
                    $outerException = New-Object System.InvalidOperationException -ArgumentList "Attribute $c could not be added to the change set", ($_.Exception)                    
                    $PSCmdlet.ThrowTerminatingError((New-Object System.Management.Automation.ErrorRecord -ArgumentList ($outerException),"InvalidAttribute",([System.Management.Automation.ErrorCategory]::InvalidArgument),($changes[$c])))
                }
            }        
        }
        else
        {
            $importObject.Changes = $Changes
        }
        
        ###
        ### Handle Reslove and Join Pairs
        ###
        if ($AnchorPairs)
        {
            if ($AnchorPairs -is [Microsoft.ResourceManagement.Automation.ObjectModel.JoinPair[]] -or $AnchorPairs -is [Microsoft.ResourceManagement.Automation.ObjectModel.JoinPair])
            {
                $importObject.AnchorPairs = $AnchorPairs
            }
            else
            {
                $AnchorPairs.GetEnumerator() | 
                ForEach{
                    $anchorPair = New-Object Microsoft.ResourceManagement.Automation.ObjectModel.JoinPair
                    $anchorPair.AttributeName = $_.Key
                    $anchorPair.AttributeValue = $_.Value
                    $importObject.AnchorPairs += $anchorPair
                }        
            }    
        }
        
        ###
        ### Handle Put and Delete
        ###
        if (($State -ieq 'Put' -or $State -ieq 'Delete') -and $importObject.AnchorPairs.Count -eq 1)
        {
            $errorVariable = $null
		    $targetID = Get-FimObjectID -ObjectType $ObjectType -Uri $Uri -AttributeName @($importObject.AnchorPairs)[0].AttributeName -AttributeValue @($importObject.AnchorPairs)[0].AttributeValue -ErrorAction SilentlyContinue -ErrorVariable errorVariable

            if ($errorVariable)
            {
                Write-Error $errorVariable[1]
                return
            }
            
			$importObject.TargetObjectIdentifier = $targetID
        }     
       
        ###
        ### Handle Duplicate Values on a Put request
        ###
        if ($State -ieq 'Put')# -and $Operation -ieq 'Add')
        {
            ### Get the Target object
            $currentFimObject = Export-FIMConfig -Uri $Uri -OnlyBaseResources -CustomConfig ("/*[ObjectID='{0}']" -F $importObject.TargetObjectIdentifier) | Convert-FimExportToPSObject
            
          ### Create a new array containing only valid ADDs
            $uniqueImportChanges = @($importObject.Changes | Where-Object {$_.Operation -ne 'Add'})
            $importObject.Changes | 
                Where-Object {$_.Operation -eq 'Add'} |
                ForEach-Object {
                    Write-Verbose ("Checking to see if attribute '{0}' already has a value of '{1}'" -F $_.AttributeName, $_.AttributeValue)
                    if ($currentFimObject.($_.AttributeName) -eq $_.AttributeValue)
                    {
                        Write-Warning ("Duplicate attribute found: '{0}' '{1}'" -F $_.AttributeName, $_.AttributeValue)
                    }
                    else
                    {
                        $uniqueImportChanges += $_
                    }
                }
            ### Replace the Changes array with our validated array
            $importObject.Changes = $uniqueImportChanges
            $importObject.Changes = $importObject.Changes | Where {$_ -ne $null}
            
            if (-not ($importObject.Changes))
            {
                Write-Warning "No changes left on this Put request."
            }
        }
        
        if ($ApplyNow -eq $true)
        {
            if (-not $SkipDuplicateCheck)
            {
                $importObject = $importObject | Skip-DuplicateCreateRequest -Uri $Uri 
            }
            
			if(-not $AllowAuthorizationException)
            {
                $importObject | Import-FIMConfig -Uri $Uri                
            } 
			else
			{
				try
				{
					###
					### We do this inside a try..catch because we need prevent Import-FimConfig from throwing an error
					### When Import-FimConfig submits a Request that hits an AuthZ policy, it raises an error
					### We want to eat that specific error to prevent the FIM Request from failing
					###	

					$importObject | Import-FIMConfig -Uri $Uri

				}
				catch
				{
					if ($_.Exception.Message -ilike '*While creating a new object, the web service reported the object is pending authorization*')
					{
						Write-Verbose ("FIM reported the object is pending authorization:`n`t {0}" -f $_.Exception.Message) 
					}
					else
					{
						throw
					}
				}
			}
        }
        
        if ($PassThru -eq $true)
        {
            Write-Output $importObject
        }     
	}
}

Function New-FimImportChange
{
    Param
    (                              
        [parameter(Mandatory=$true)] 
		[String]
        $AttributeName,
        
        [parameter(Mandatory=$false)] 
		[ValidateScript({($_ -is [Array] -and $_.Count -eq 3) -or $_ -is [String] -or $_ -is [DateTime] -or $_ -is [Bool] -or $_ -is [Int] -or ($_ -is [Guid])})]
        $AttributeValue,
		
		[parameter(Mandatory=$true)]
		[ValidateSet("Add", "Replace", "Delete", "None")]
        $Operation,
		
		[parameter(Mandatory=$false)]  
		[Boolean]
        $FullyResolved = $true,
        
        [parameter(Mandatory=$false)]  
		[String]
        $Locale = "Invariant",

        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725"
    ) 
    END
    {
        $importChange = New-Object Microsoft.ResourceManagement.Automation.ObjectModel.ImportChange
        $importChange.Operation = $Operation
        $importChange.AttributeName = $AttributeName
        $importChange.FullyResolved = $FullyResolved
        $importChange.Locale = $Locale
        
        ###
        ### Process the AttributeValue Parameter
        ###
        if ($AttributeValue -is [String])
        {
            $importChange.AttributeValue = $AttributeValue
        }
		elseif ($AttributeValue -is [DateTime])
		{
			$importChange.AttributeValue = $AttributeValue.ToString() #"yyyy'-'MM'-'dd'T'HH':'mm':'ss'.000'")
		}
        elseif (($AttributeValue -is [Boolean]) -or ($AttributeValue -is [Int]) -or ($AttributeValue -is [Guid]))
        {
            $importChange.AttributeValue = $AttributeValue.ToString()
        }
        elseif ($AttributeValue -is [Array])
        {
            ###
            ### Resolve Resolve Resolve
            ###
            if ($AttributeValue.Count -ne 3)
            {
                Write-Error "For the 'Resolve' option to work, the AttributeValue parameter requires 3 values in this order: ObjectType, AttributeName, AttributeValue"
            }
			$objectId = Get-FimObjectID -Uri $Uri -ObjectType $AttributeValue[0] -AttributeName $AttributeValue[1] -AttributeValue $AttributeValue[2]
            
			if (-not $objectId)
            {
                Throw (@"
                FIM Resolve operation failed for: {0}:{1}:{2}
                Could not find an object of type '{0}' with an attribute '{1}' value equal to '{2}'
"@ -F $AttributeValue[0],$AttributeValue[1],$AttributeValue[2])
            }
            else
            {
				$importChange.AttributeValue = $objectId              
            }            
        }
        else
        {
            Write-Verbose "Null or unsupported `$attributeValue provided"
        }
        $importChange
    }
}

Function Skip-DuplicateCreateRequest
{
<#
	.SYNOPSIS 
	Detects a duplicate 'Create' request then removes it from the pipeline

	.DESCRIPTION
	The Skip-DuplicateCreateRequest function makes it easier to use Import-FimConfig by providing preventing a duplicate Create request.
	In most cases FIM allows the creation of duplicate objects since it mostly does not enforce uniqueness.  When loading configuration objects this can easily lead to the accidental duplication of MPRs, Sets, Workflows, etc.	

	.PARAMETER ObjectType
	The object type for the target object.
	NOTE: this is case sensitive
	NOTE: this is the ResourceType's 'name' attribute, which often does NOT match what is seen in the FIM Portal.
   
	.OUTPUTS
 	the FIM ImportObject is returned by this function ONLY if a duplicate was not fount.
   
   	.EXAMPLE
	PS C:\$createRequest = New-FimImportObject -ObjectType Person -State Create -Changes @{
		AccountName='Bob' 
		DisplayName='Bob the Builder'
		}
	PS C:\$createRequest | Skip-DuplicateCreateRequest | Import-FIMConfig
	
	DESCRIPTION
	-----------
   	Creates an ImportObject for creating a new Person object with AccountName and DisplayName.
	If an object with the DisplayName 'Bob the Builder' already exists, then a warning will be displayed, and no input will be provided to Import-FimConfig because the Skip-DuplicateCreateRequest would have filtered it from the pipeline.
#>
   	Param
    ( 
		<#
		AnchorAttributeName is used to detect the duplicate in the FIM Service.  It defaults to the 'DisplayName' attribute.		
		#>
        [parameter(Mandatory=$true, ValueFromPipeline = $true)]        
        $ImportObject,
        [String]
        $AnchorAttributeName = 'DisplayName',
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725"
    )
    Process
    {
        if ($ImportObject.State -ine 'Create')
        {
            Write-Output $ImportObject
            return
        }
        
        $anchorAttributeValue = $ImportObject.Changes | where {$_.AttributeName -eq $AnchorAttributeName} | select -ExpandProperty AttributeValue
        
		###
		### If the anchor attribute is not present on the ImportObject, then we can't detect a duplicate
		### Behavior in this case is to NOT filter
		###
        if (-not $anchorAttributeValue)
        {
            Write-Warning "Skipping duplicate detection for this Create Request because we do not have an anchor attribute to search with."
            Write-Output $ImportObject
            return
        }
        
        $objectId = Get-FimObjectID -Uri $Uri -ObjectType $ImportObject.ObjectType -AttributeName $AnchorAttributeName -AttributeValue $anchorAttributeValue -ErrorAction SilentlyContinue
            
		if ($objectId)
        {
            ### This DID resolve to an object on the target system
            ### so it is NOT safe to create
            ### do NOT put the object back on the pipeline
            Write-Warning "An object matches this object in the target system, so skipping the Create request"
        } 
        else
        {
            ### This did NOT resolve to an object on the target system
            ### so it is safe to create
            ### put the object back on the pipeline
            Write-Output $ImportObject     
        }
     }
}

Function Wait-FimRequest
{
   	Param
    ( 
        [parameter(Mandatory=$true, ValueFromPipeline = $true)]
        [Microsoft.ResourceManagement.Automation.ObjectModel.ImportObject]
        [ValidateScript({$_.TargetObjectIdentifier -like "urn:uuid:*"})]
        $ImportObject,
        
        [parameter(Mandatory=$false)]
        $RefreshIntervalInSeconds = 5,
        
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725"   
    )
    Process
    { 
        ###
    	### Loop while the Request.RequestStatus is not any of the Final status values
        ###
    	Do{
            ###
            ### Get the FIM Request object by querying for a Request by Target
            ###
            $xpathFilter = @" 
                /Request 
                    [ 
                        Target='{0}'
                        and RequestStatus != 'Denied' 
                        and RequestStatus != 'Failed' 
                        and RequestStatus != 'Canceled' 
                        and RequestStatus != 'CanceledPostProcessing' 
                        and RequestStatus != 'PostProcessingError' 
                        and RequestStatus != 'Completed' 
                    ] 
"@ -F $ImportObject.TargetObjectIdentifier.Replace('urn:uuid:','')
            
    	    $requests = Export-FIMConfig -Uri $Uri -OnlyBaseResources -CustomConfig $xpathFilter
    	    
    	    if ($requests -ne $null)
    	    {
    	        Write-Verbose ("Number of pending requests: {0}" -F $requests.Count)
    	        Start-Sleep -Seconds $RefreshIntervalInSeconds
    	    }
    	} 
    	While ($requests -ne $null)
    } 
}

Function Convert-FimExportToPSObject
{
    Param
    (
        [parameter(Mandatory=$true, ValueFromPipeline = $true)]
        [Microsoft.ResourceManagement.Automation.ObjectModel.ExportObject]
        $ExportObject
    )
    Process
    {        
        $psObject = New-Object PSObject
        $psObject.PSTypeNames.Insert(0, 'FIMPowerShellModule.FimObject')

        $ExportObject.ResourceManagementObject.ResourceManagementAttributes | ForEach-Object{
            if ($_.Value -ne $null)
            {
                $value = $_.Value
            }
            elseif($_.Values -ne $null)
            {
                $value = $_.Values
            }
            else
            {
                $value = $null
            }
            $psObject | Add-Member -MemberType NoteProperty -Name $_.AttributeName -Value $value
        }
        Write-Output $psObject
    }
}

Function Get-FimObjectID
{
   	Param
    (       
        $ObjectType,
		
        [parameter(Mandatory=$true)]
        [String]
        $AttributeName = 'DisplayName',
		
        [parameter(Mandatory=$true)]
        [alias(“searchValue”)]
        [String]
        $AttributeValue,

        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725"
    )
    Process
    {   
		$resolver = New-Object Microsoft.ResourceManagement.Automation.ObjectModel.ImportObject
        $resolver.SourceObjectIdentifier = [Guid]::Empty
        $resolver.TargetObjectIdentifier = [Guid]::Empty
        $resolver.ObjectType 			 = $ObjectType
        $resolver.State 				 = 'Resolve'
		
        $anchorPair = New-Object Microsoft.ResourceManagement.Automation.ObjectModel.JoinPair
        $anchorPair.AttributeName  = $AttributeName
        $anchorPair.AttributeValue = $AttributeValue
                    
        $resolver.AnchorPairs = $anchorPair
        
        try
        {
            Import-FIMConfig $resolver -Uri $Uri -ErrorAction Stop | Out-Null
     
            if ($resolver.TargetObjectIdentifier -eq [Guid]::Empty)
            {
                ### This did NOT resolve to an object on the target system
                Write-Error ("An object was not found with this criteria: '{0}:{1}:{2}'"   -F  $ObjectType, $AttributeName,  $AttributeValue)
            }
            else
            {
                ### This DID resolve to an object on the target system
                Write-Output ($resolver.TargetObjectIdentifier -replace 'urn:uuid:')
            }         
        }
        catch
        {
            if ($_.Exception.Message -ilike '*the target system returned no matching object*')
            {
                ### This did NOT resolve to an object on the target system
                Write-Error ("An object was not found with this criteria: '{0}:{1}:{2}'" -F  $ObjectType, $AttributeName,  $AttributeValue)
            }
            elseif ($_.Exception.Message -ilike '*cannot filter as requested*')
            {
                ### This is a bug in Import-FIMConfig whereby it does not escape single quotes in the XPath filter
                ### Try again using Export-FIMConfig
                $exportResult = Export-FIMConfig -Uri $Uri -OnlyBaseResources -CustomConfig ("/{0}[{1}=`"{2}`"]" -F $resolver.ObjectType, $resolver.AnchorPairs[0].AttributeName, $resolver.AnchorPairs[0].AttributeValue ) -ErrorAction SilentlyContinue
                
                if ($exportResult -eq $null)
                {
                    Write-Error ("An object was not found with this criteria: '{0}:{1}:{2}'" -F  $ObjectType, $AttributeName,  $AttributeValue)
                }
                else
                {
                    Write-Output ($exportResult.ResourceManagementObject.ObjectIdentifier -replace 'urn:uuid:' )
                }            
            }
            else
            {
               Write-Error ("Import-FimConfig produced an error while resolving this object in the target system. The exception thrown was: {0}" -F $_.Exception.Message)
            } 
        }
    }
}

function Get-ObjectSid
{
<#
.SYNOPSIS 
Gets the ObjectSID as Base64 Encoded String

.DESCRIPTION
GetSidAsBase64 tries to find the object, then translate it into a Base64 encoded string

.OUTPUTS
a string containing the Base64 encoded ObjectSID

.EXAMPLE
Get-ObjectSid -AccountName v-crmart -Verbose

OUTPUT
------
VERBOSE: Finding the SID for account: v-crmart
AQUAAAXXXAUVAAAAoGXPfnyLm1/nfIdwyoM6AA==  
	
DESCRIPTION
-----------
Gets the objectSID for 'v-crmart'
Does not supply a value for Domain

.EXAMPLE
Get-ObjectSid -AccountName v-crmart -Domain Redmond -Verbose

OUTPUT
------
VERBOSE: Finding the SID for account: Redmond\v-crmart
AQUAAAXXXAUVAAAAoGXPfnyLm1/nfIdwyoM6AA==  
	
DESCRIPTION
-----------
Gets the objectSID for 'v-crmart'
Does not supply a value for Domain
#>
   	param
    ( 
		<#
		A String containing the SamAccountName
		#>
        [parameter(Mandatory=$true)]
		[String]		
        $AccountName,
		<#
		A String containing the NetBIOS Domain Name
		#>		
        [parameter(Mandatory=$false)]
        [String]
        $Domain
	)
   	END
    {
		###
		### Construct the Account 
		###
		if ([String]::IsNullOrEmpty($Domain))
		{
			$account = $AccountName
		}
		else
		{
			$account = "{0}\{1}" -f $Domain, $AccountName
		}
		
        Write-Verbose "Finding the SID for account: $account"
		###
		### Get the ObjectSID
		###
		$ntaccount = New-Object System.Security.Principal.NTAccount $account
		try
		{
		    $binarySid = $ntaccount.Translate([System.Security.Principal.SecurityIdentifier]) 
		}
		catch
		{    
		    Throw @"
		Account could not be resolved to a SecurityIdentifier
"@  
		}
		
		$bytes = new-object System.Byte[] -argumentList $binarySid.BinaryLength
		$binarySid.GetBinaryForm($bytes, 0)
		$stringSid = [System.Convert]::ToBase64String($bytes)

		Write-Output $stringSid
    }
}

function Get-FimRequestParameter
{
<#
	.SYNOPSIS 
	Gets a RequestParameter from a FIM Request into a PSObject

	.DESCRIPTION
	The Get-FimRequestParameter function makes it easier to view FIM Request Parameters by converting them from XML into PSObjects
	This makes it easier view the details for reporting, and for turning a FIM Request back into a new FIM Request to repro fubars
   
	.OUTPUTS
 	a PSObject with the following properties:
		1. PropertyName
		2. Value
		3. Operation
   
   	.EXAMPLE
	$request = Export-FIMConfig -only -CustomConfig ("/Request[TargetObjectType = 'Person']") | 
    	Select -First 1 |
    	Convert-FimExportToPSObject |
		Get-FimRequestParameter
	
		OUTPUT
		------
		Value                                PropertyName                            Operation
		-----                                ------------                            ---------
        CraigMartin                          AccountName                             Create
        CraigMartin                          DisplayName                             Create
        Craig                                FirstName                               Create
        Martin                               LastName                                Create
		Person                               ObjectType                              Create   
		4ba58a6e-5953-4c03-af83-7dbfb94691d4 ObjectID                                Create   
		7fb2b853-24f0-4498-9534-4e10589723c4 Creator                                 Create   
		
		DESCRIPTION
		-----------
		Gets one Request object from FIM, converts it to a PSOBject
#>
   	param
    ( 
		<#
		A String containing the FIM RequestParameter XML
        or
        A PSObject containing the RequestParameter property
		#>
        [parameter(Mandatory=$true, ValueFromPipeline = $true)]
        [ValidateScript({
		($_ -is [String] -and $_ -like "<RequestParameter*") `
		-or  `
		($_ -is [PSObject] -and $_.RequestParameter)})]
        $RequestParameter
    )
    process
    { 
        ### If the input is a PSObject then get just the RequestParameter property
        if ($RequestParameter -is [PSObject])
        {
            $RequestParameter = $RequestParameter.RequestParameter
        }
        
        $RequestParameter | foreach-Object{
            New-Object PSObject -Property @{
                PropertyName = ([xml]$_).RequestParameter.PropertyName
                Value = ([xml]$_).RequestParameter.Value.'#text'
                Operation = ([xml]$_).RequestParameter.Operation
                Mode = ([xml]$_).RequestParameter.Mode
            } | 
            Write-Output
        }
    }
}

##TODO: Add parameter sets and help to this
Function New-FimSynchronizationRule
{
    [CmdletBinding()]
    [OutputType([Guid])]
   	param
    (
        [parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $DisplayName,
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Description,
        [parameter(Mandatory=$true)]        
		$ManagementAgentID,
        [parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("ConnectedObjectType")]
        [string]
		$ExternalSystemResourceType,
        [parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [Alias("ILMObjectType")]
        [string]
		$MetaverseResourceType,
        [parameter(Mandatory=$false)]
        [Alias("DisconnectConnectedSystemObject")]
		[Switch]
        $EnableDeprovisioning,
        [parameter(Mandatory=$false)]
        [Alias("CreateConnectedSystemObject")]
		[Switch]
		$CreateResourceInExternalSystem,
        [parameter(Mandatory=$false)]
        [Alias("CreateILMObject")]
		[Switch]
        $CreateResourceInFIM,
        [parameter(Mandatory=$true)]
        [ValidateSet("Inbound", "Outbound","InboundAndOutbound")]
        [Alias("FlowType")]
		$Direction,
        [parameter(Mandatory=$false)]	
        [ValidateRange(1, 1000)] ##TODO: Using 1000 instead of [Int32]::MaxValue because module import fails otherwise
        [int]
        $Precedence = 1,
        [parameter(Mandatory=$false)] # Thanks to Aneesh Varghese at NZ Ministry of Social Development who pointed out 
        [Alias("Dependency")]         # we need to include dependency as well
        $DependencyRef, 
        [parameter(Mandatory=$false)]
		$RelationshipCriteria = @{},
        [parameter(Mandatory=$false)]
        [Switch]
        [Alias("msidmOutboundIsFilterBased")]
        $OutboundFilterBased = $false,
        [parameter(Mandatory=$false)]
        [Alias("msidmOutboundScopingFilters")]
        $OutboundScopingFilter,
        [parameter(Mandatory=$false)]
        [Alias("ExistenceTest")]   ## Added by Aneesh Varghese, need to include ExistenceTest flow rules as well
        $ExistenceTestFlowRules =  @(),
        [parameter(Mandatory=$false)]
        [Alias("PersistentFlow")]
        [string[]]
		$PersistentFlowRules = @(),
        [parameter(Mandatory=$false)]
        [Alias("InitialFlow")]
        [string[]]
		$InitialFlowRules = @(),
        [parameter(Mandatory=$false)]
        [Alias("ConnectedSystemScope")]
        [string[]]
		$ExternalSystemScopingFilter = @(),
        [parameter(Mandatory=$false)]
        [string[]]
        [Alias("SynchronizationRuleParameters")]
        $WorkflowParameters = @(),
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of the FIM Service. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru
    )
    
    $changeSet = @{
	    DisplayName 						= $DisplayName	    
	    ManagementAgentID 					= $ManagementAgentID
		ConnectedObjectType 				= $ExternalSystemResourceType
	    ILMObjectType 						= $MetaverseResourceType
		Precedence 							= $Precedence
        CreateConnectedSystemObject         = $CreateResourceInExternalSystem.ToBool()
    }

    if ($Direction -eq "Inbound")
    {
        $changeSet.Add("FlowType", 0)
    }
    elseif ($Direction -eq "Outbound")
    {
        $changeSet.Add("FlowType", 1)
    }
    elseif ($Direction -eq "InboundAndOutbound")
    {
        $changeSet.Add("FlowType", 2)
    }
    else
    {
        throw "Unsupported value for `$Direction"
    }

    if ($OutboundFilterBased.ToBool() -eq $false)
    {
        $changeSet.Add("DisconnectConnectedSystemObject", $EnableDeprovisioning.ToBool())		
		$changeSet.Add("CreateILMObject", $CreateResourceInFIM.ToBool())	
        $changeSet.Add("msidmOutboundIsFilterBased", $false)
    }
    else
    {
        $changeSet.Add("DisconnectConnectedSystemObject", $false)
		$changeSet.Add("CreateILMObject", $false)
        $changeSet.Add("msidmOutboundIsFilterBased", $true)
    
        if ($OutboundScopingFilter)
    {
            $changeSet.Add("msidmOutboundScopingFilters", $OutboundScopingFilter)
        }
    }

    $srImportObject = New-FimImportObject -ObjectType SynchronizationRule -State Create -Changes $changeSet -PassThru
    
    if ($Description)
    {
        $srImportObject.Changes += New-FimImportChange -AttributeName Description -Operation None -AttributeValue $Description         						
    }
    
    if ($RelationshipCriteria -and $OutboundFilterBased.ToBool() -ne $true)
    {
        $localRelationshipCriteria = "<conditions>"
        foreach ($key in $RelationshipCriteria.Keys)
        {
            $localRelationshipCriteria += ("<condition><ilmAttribute>{0}</ilmAttribute><csAttribute>{1}</csAttribute></condition>" -f $key, $RelationshipCriteria[$key])
        }
        $localRelationshipCriteria += "</conditions>"

        $srImportObject.Changes += New-FimImportChange -AttributeName RelationshipCriteria -Operation None -AttributeValue $localRelationshipCriteria
    }
    else
    {
        $srImportObject.Changes += New-FimImportChange -AttributeName RelationshipCriteria -Operation None -AttributeValue "<conditions/>"    
    }
   
    if ($WorkflowParameters -and ($OutboundFilterBased.ToBool() -eq $false) -and ($Direction -ne "Inbound"))
    {
        foreach ($w in $WorkflowParameters)
    {
            $srImportObject.Changes += New-FimImportChange -AttributeName SynchronizationRuleParameters -Operation Add -AttributeValue $w
        }
    }
        
    if ($ExternalSystemScopingFilter)
    {
        foreach ($filter in $ExternalSystemScopingFilter)
        {
            $srImportObject.Changes += New-FimImportChange -AttributeName ConnectedSystemScope -Operation Add -AttributeValue $filter
        }
    }

    ## Added by Aneesh Varghese, need to include Dependency attribute
    if($DependencyRef)
    {
        $srImportObject.Changes += New-FimImportChange -AttributeName Dependency -Operation Add -AttributeValue $DependencyRef
    }
    
    ## Added by Aneesh Varghese, need to include ExistenceTest flow rules as well
    $ExistenceTestFlowRules | ForEach-Object {
        $srImportObject.Changes += New-FimImportChange -AttributeName ExistenceTest -Operation Add -AttributeValue $_
    }  
	
	$PersistentFlowRules | ForEach-Object {
		$srImportObject.Changes += New-FimImportChange -AttributeName PersistentFlow -Operation Add -AttributeValue $_
	}
    
    $InitialFlowRules | ForEach-Object {
		$srImportObject.Changes += New-FimImportChange -AttributeName InitialFlow -Operation Add -AttributeValue $_
	}
	
	$srImportObject | Skip-DuplicateCreateRequest -Uri $Uri | Import-FIMConfig -Uri $Uri

    if ($PassThru.ToBool())
    {
        Write-Output [guid](Get-FimObjectID -ObjectType SynchronizationRule -AttributeName DisplayName -AttributeValue $DisplayName)
    }
}

function Start-SQLAgentJob
{
	param
	(
		[parameter(Mandatory=$false)]
		[String]
		$JobName = "FIM_TemporalEventsJob",
		[parameter(Mandatory=$true)]
		[String]
		$SQLServer,	
		[parameter(Mandatory=$false)]
		[Switch]
		$Wait
	)
	
	$connection = New-Object System.Data.SQLClient.SQLConnection
	$Connection.ConnectionString = "server={0};database=FIMService;trusted_connection=true;" -f $SQLServer
	$connection.Open()
	
	$cmd = New-Object System.Data.SQLClient.SQLCommand
	$cmd.Connection = $connection
	$cmd.CommandText = "exec msdb.dbo.sp_start_job '{0}'" -f $JobName
	
	Write-Verbose "Executing job $JobName on $SQLServer"
	$cmd.ExecuteNonQuery()
	
	if ($Wait)
	{
		$cmd.CommandText = "exec msdb.dbo.sp_help_job @job_name='{0}', @execution_status = 4" -f $JobName
		
		$reader = $cmd.ExecuteReader()
		
		while ($reader.HasRows -eq $false)
		{
			Write-Verbose "Job is still executing. Sleeping..."
			Start-Sleep -Milliseconds 1000
			
			$reader.Close()
			$reader = $cmd.ExecuteReader()
		}
	}
	$connection.Close()
}

function New-FimSchemaBinding
{
    [CmdletBinding()]
   	param
  	(
        [parameter(Mandatory=$true)]
        [ValidateScript({ ($_ -is [Guid]) -or ($_ -is [String]) })]
   		$ObjectType, 
        [parameter(Mandatory=$true)]
        [ValidateScript({ ($_ -is [Guid]) -or ($_ -is [String]) })]
		$AttributeType, 
        [parameter(Mandatory=$false)]
        [Switch]
		$Required = $false,
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [string]
		$DisplayName = $AttributeType,
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
	    [String]
		$Description,
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
	    [String]
	    $Uri = "http://localhost:5725"

   	)     
	if (Get-FimSchemaBinding $AttributeType $ObjectType $Uri)
	{
		Write-Warning "Binding Already Exists for $objectType and $attributeType"
        return
	}

    $changeSet = @{
        DisplayName	= $DisplayName 
        Required	= $Required.ToBool()
    }
    
    if ($Description)
    {
        $changeSet.Add("Description", $Description)
    }

    if ($ObjectType -is [Guid])
    {
        $changeSet.Add("BoundObjectType", $ObjectType)
    }
    elseif ($ObjectType -is [String])
    {
        $changeSet.Add("BoundObjectType", ('ObjectTypeDescription', 'Name', $ObjectType))
    }
    else
    {
        throw "Unsupported input format for -ObjectType"
    }

    if ($AttributeType -is [Guid])
    {
        $changeSet.Add("BoundAttributeType", $ObjectType)
    }
    elseif ($AttributeType -is [String])
    {
        $changeSet.Add("BoundAttributeType", ('AttributeTypeDescription', 'Name', $AttributeType))
    }
    else
    {
        throw "Unsupported input format for -AttributeType"
    }

    New-FimImportObject -ObjectType BindingDescription -State Create -Uri $Uri -Changes $changeSet -SkipDuplicateCheck -ApplyNow 
} 

function New-FimSchemaAttribute
{
    [CmdletBinding()]
    [OutputType([Guid])]
  	param
   	(
        [parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
   		$Name,
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [String]
		$DisplayName = $Name,
        [parameter(Mandatory=$true)]
        [ValidateSet("Binary","Boolean","DateTime","Integer","String","Reference","Text")]
        [String]
		$DataType,
        [parameter(Mandatory=$false)]
        [switch]
		$Multivalued = $false,
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Description,
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
        [parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
	    [String]
	    $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $false
   	)     

    $changeSet= @{
		DisplayName = $DisplayName
		Name		= $Name
		DataType	= $DataType
		Multivalued	= $Multivalued.ToBool()
    }

    if ($Description)
    {
        $changeSet.Add("Description", $Description)
    }

    New-FimImportObject -ObjectType AttributeTypeDescription -State Create -Uri $Uri -Changes $changeSet -ApplyNow

    if ($PassThru.ToBool())
    {
        Write-Output ([guid](Get-FimObjectID -ObjectType AttributeTypeDescription -AttributeName Name -AttributeValue $Name -Uri $Uri))
    }
} 
 
function New-FimSchemaObjectType
{
    [CmdletBinding()]
    [OutputType([Guid])]
    param
    (
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]
    $Name, 
    [parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [String]
    $DisplayName = $Name,
    [parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [string]
    $Description,
    <#
    .PARAMETER Uri
    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
    #>
    [parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [String]
    $Uri = "http://localhost:5725",
    [parameter(Mandatory=$false)]
    [Switch]
    $PassThru = $false    
    )             

    $changeSet = @{
        DisplayName = $DisplayName
        Name		= $Name
    }

    if ($Description)
    {
        $changeSet.Add("Description", $Description)
    }
    

    New-FimImportObject -ObjectType ObjectTypeDescription -State Create -Uri $Uri -Changes $changeSet -ApplyNow   
    
    if ($PassThru.ToBool())
    {
        Write-Output ([guid](Get-FimObjectID -ObjectType ObjectTypeDescription -AttributeName Name -AttributeValue $Name))
    }
} 

function Get-FimSchemaBinding
{
  	Param
   	(        
        [String]		
        $AttributeType,
		
        [String]		
        $ObjectType,
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725"

    )  
	$attributeTypeID 	= Get-FimObjectID AttributeTypeDescription 	Name $AttributeType
	$objectTypeID 		= Get-FimObjectID ObjectTypeDescription 	Name $ObjectType
	
    $xPathFilter = "/BindingDescription[BoundObjectType='{0}' and BoundAttributeType='{1}']" -f $objectTypeID, $attributeTypeID
    Export-FIMConfig -OnlyBaseResources -CustomConfig $xPathFilter -Uri $Uri | Convert-FimExportToPSObject           
}

function New-FimSet
{
    [CmdletBinding()]
    [OutputType([Guid])]
  	param
   	(
		[String]
		[Parameter(Mandatory = $True)]
		$DisplayName = $Name,
		[Parameter()]
        $Description,
		[Parameter()]
		[String]
        $Filter, ##TODO - make sure we were passed JUST the XPath filter
		[Parameter()]
		[Array]
		$ManuallyManagedMembers,
        <#
	    .PARAMETER Uri
	    The Uniform Resource Identifier (URI) of themmsshortService. The following example shows how to set this parameter: -uri "http://localhost:5725"
	    #>
	    [String]
	    $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $true
   	)
	$changeSet = @()
	$changeSet += New-FimImportChange -Operation Replace -AttributeName "DisplayName" -AttributeValue $DisplayName
	
	if ([String]::IsNullOrEmpty($Description) -eq $false)
	{
		$changeSet += New-FimImportChange -Operation Replace -AttributeName "Description" -AttributeValue $Description		
	}
	
	if ([String]::IsNullOrEmpty($Filter) -eq $false)
	{
		# this is all one line to make the filter backwards compatible with FIM 2010 RTM  
		$setXPathFilter = "<Filter xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' Dialect='http://schemas.microsoft.com/2006/11/XPathFilterDialect' xmlns='http://schemas.xmlsoap.org/ws/2004/09/enumeration'>{0}</Filter>" -F $Filter

		$changeSet += New-FimImportChange -Operation Replace -AttributeName "Filter" -AttributeValue $setXPathFilter
	}
	
	if (($ManuallyManagedMembers -ne $null) -and ($ManuallyManagedMembers.Count -gt 0))
	{
		foreach ($m in $ManuallyManagedMembers)
		{
			$changeSet += New-FimImportChange -Operation Add -AttributeName "ExplicitMember" -AttributeValue $m
		}		
	}
	
    New-FimImportObject -ObjectType Set -State Create -Uri $Uri -Changes $changeSet -ApplyNow
	
    if ($PassThru.ToBool())
    {
	    Write-Output (Get-FimObjectId -ObjectType Set -AttributeName DisplayName -AttributeValue $DisplayName)
    }
 }
function Submit-FimRequest
{
<#
.SYNOPSIS 
Sumits a FIM ImportObject to the server using Import-FimConfig

.DESCRIPTION
The Submit-FimRequest function makes it easier to use Import-FimConfig by optionally waiting for the request to complete
The function submits the request, then searches FIM for the Request.
   
.EXAMPLE
$importCraigFoo = New-FimImportObject -ObjectType Person -State Create -Changes @{DisplayName='CraigFoo';AccountName='CraigFoo'} 
$importCraigFoo | Submit-FimRequest -Wait -Verbose

VERBOSE: FIM reported the object is pending authorization:
While creating a new object, the web service reported the object is pending authorization.  The import cannot continue until the object exists.  Please approve the object and then replace all subsequent references to this object with its object id.  Once the references are up to date, please resume the import by providing the output from this stream as input.

Request ID = urn:uuid:eff37dbb-9bdf-4c8b-8aa3-b246f28de411
ObjectType = Person
SourceObjectID = d4cfbffb-1c51-40b8-83f6-894318b6722c
VERBOSE: Number of pending requests: 1
VERBOSE: Number of pending requests: 1
VERBOSE: Number of pending requests: 1
VERBOSE: Number of pending requests: 1
	
DESCRIPTION
-----------
Creates an ImportObject then submits it to this function, and waits for the request to finish

#>

	param
	( 
	[parameter(Mandatory=$true, ValueFromPipeline = $true)]
	[Microsoft.ResourceManagement.Automation.ObjectModel.ImportObject]	
	$ImportObject,

    [switch]
    $Wait = $false,
	
	[System.Management.Automation.PSCredential]
	$Credential,

	[parameter(Mandatory=$false)]
	$RefreshIntervalInSeconds = 5 
	)
	begin
	{
		if ($Credential)
		{
			### Dirty trick for force the FIM cmdlets to use the supplied Creds
			Export-FimConfig -only -custom "/Person[DisplayName='hoofhearted']" -credential $Credential -ErrorAction SilentlyContinue | Out-Null
		}
	}
	process
	{
        try
        {
            Import-FIMConfig $ImportObject -ErrorAction Stop | Out-Null
        } ### CLOSING: Try
        catch
        {
            if ($_.Exception.Message -ilike '*While creating a new object, the web service reported the object is pending authorization*')
            {
                Write-Verbose ("FIM reported the object is pending authorization:`n`t {0}" -f $_.Exception.Message) 
                $requestGuid = $_.Exception.Message | 
                    Select-String  -pattern "Request ID = urn:uuid:[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}" |
                    Select-Object -ExpandProperty Matches |                
                    Select-Object -First 1 |
                    Select-Object -ExpandProperty value | 
                    ForEach-Object {$_ -replace 'Request ID = urn:uuid:'}
            }
        }### CLOSING: Catch               
        
        ###
        ### Get the Request
        ###
        if ($requestGuid)
        {
            $xpathFilter = @"
            /Request
            [
                    ObjectID='{0}'
                and RequestStatus != 'Denied' 
    			and RequestStatus != 'Failed' 
    			and RequestStatus != 'Canceled' 
    			and RequestStatus != 'CanceledPostProcessing' 
    			and RequestStatus != 'PostProcessingError' 
    			and RequestStatus != 'Completed' 
            ]
"@ -F $requestGuid
        }
        elseif ($ImportObject.TargetObjectIdentifier -ne [Guid]::Empty)
        {
            $xpathFilter = @" 
                /Request 
    			[ 
    			Target='{0}'
    			and RequestStatus != 'Denied' 
    			and RequestStatus != 'Failed' 
    			and RequestStatus != 'Canceled' 
    			and RequestStatus != 'CanceledPostProcessing' 
    			and RequestStatus != 'PostProcessingError' 
    			and RequestStatus != 'Completed' 
    			] 
"@ -F $ImportObject.TargetObjectIdentifier.Replace('urn:uuid:','')
        }
        else
        {
            $xpathFilter = @" 
                /Request 
    			[ 
    			TargetObjectType   = '{0}'
                and Operation      = 'Create'
    			and RequestStatus != 'Denied' 
    			and RequestStatus != 'Failed' 
    			and RequestStatus != 'Canceled' 
    			and RequestStatus != 'CanceledPostProcessing' 
    			and RequestStatus != 'PostProcessingError' 
    			and RequestStatus != 'Completed' 
    			] 
"@ -F $ImportObject.ObjectType, $ImportObject.State
        }
   
		if (-not $Wait)
		{
			Export-FIMConfig -OnlyBaseResources -CustomConfig $xpathFilter | Convert-FimExportToPSObject	
		} 
		else
		{
			###
			### Loop while the Request.RequestStatus is not any of the Final status values
			###
			do{
				###
				### Get the FIM Request object by querying for a Request using the xPath we constructed
				###            
				$requests = Export-FIMConfig -OnlyBaseResources -CustomConfig $xpathFilter

				if ($requests -ne $null)
				{
					Write-Verbose ("Number of pending requests: {0}" -f @($requests).Count)
					Start-Sleep -Seconds $RefreshIntervalInSeconds
				}
			} 
			while ($requests -ne $null)
		}
	}### CLOSING: process 
}### CLOSING: function Submit-FimRequest

function New-FimEmailTemplate
{
<#
.SYNOPSIS 
Creates a new email template in FIM

.PARAMETER DisplayName
The DisplayName value to set for the object. If this is not specified, the subject is used.

.PARAMETER Suject
The email subject of the message generated by the template.

.PARAMETER Body
The email body content of the message generated by the template.

.PARAMETER Type
The type of template to create (Notification, Approval, Complete, Denied, or Timeout)

.PARAMETER Uri
The URI to the FIM Service. Defaults to localhost. 

.OUTPUTS
A GUID representing the ObjectID of the new email template   
#>
    [CmdletBinding()]
    [OutputType([Guid])]
    Param
    (
        [Parameter(Mandatory=$false)]
        [ValidateNotNull()]
        [String]
        $DisplayName = $Subject,
        [Parameter(Mandatory=$true)]
        [String]
        $Subject,
        [Parameter(Mandatory=$true)]
        [String]
        $Body,
        [Parameter(Mandatory=$true)]
        [ValidateSet("Notification","Approval","Complete","Denied","Timeout")]
        [String]
        $Type,
        [Parameter(Mandatory=$false)]
        [ValidateNotNull()]
        [String]
        $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $true
    )
    process
    {
        New-FimImportObject -ObjectType EmailTemplate -State Create -Uri $Uri -Changes @{
	        DisplayName       = $DisplayName
            EmailTemplateType = $Type
	        EmailSubject      = $Subject
	        EmailBody         = $Body
        } -ApplyNow

        if ($PassThru.ToBool())
        {
            $objectID = Get-FimObjectID -ObjectType EmailTemplate -AttributeName DisplayName -AttributeValue $DisplayName    
            Write-Output $objectID
        }
    }
}

function New-FimWorkflowDefinition
{
<#
.SYNOPSIS 
Creates a new workflow definition object inFIM

.PARAMETER DisplayName
The DisplayName value to set for the workflow.

.PARAMETER Description
The description of the workflow.

.PARAMETER RequestPhase
The type of workflow to create (Action, Authorization, or Authentication)

.PARAMETER RunOnPolicyUpdate
Whether or not to enable ROPU for an action workflow

.PARAMETER Xoml
The workflow definition XOML data

.PARAMETER Uri
The URI to the FIM Service. Defaults to localhost. 
#>
    [CmdletBinding()]
    [OutputType([Guid])]
    param
    (
        [Parameter(Mandatory=$true)]
        [String]
        $DisplayName,
        [Parameter(Mandatory=$false)]
        [String]
        $Description,
        [Parameter(Mandatory=$true)]
        [String]
        [ValidateSet("Action", "Authorization", "Authentication")]
        $RequestPhase,
        ##TODO Figure out how to exclude this switch except for Action WFs
        [Parameter(Mandatory=$false)]
        [Switch]
        $RunOnPolicyUpdate = $false,
        [Parameter(Mandatory=$true)]
        [String]
        $Xoml,
        [Parameter(Mandatory=$false)]
        [ValidateNotNull()]
        [String]
        $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $true              
    )
    process
    {
        $changeSet = @{
	        DisplayName 			= $DisplayName	        
	        RequestPhase 			= $RequestPhase	        
	        XOML 					= $XOML
        }

        if ([String]::IsNullOrEmpty($Description) -eq $false)
        {
            $changeSet.Add("Description", $Description)
        }

        if ($RequestPhase -eq "Action")
        {
            $changeSet.Add("RunOnPolicyUpdate", $RunOnPolicyUpdate.ToString())
        }

        New-FimImportObject -ObjectType WorkflowDefinition -State Create -Uri $Uri -Changes $changeSet -ApplyNow

        if ($PassThru.ToBool())
        {
            $objectID = Get-FimObjectID -ObjectType WorkflowDefinition -AttributeName DisplayName -AttributeValue $DisplayName
            Write-Output $objectID
        }
    }
}

function New-FimManagementPolicyRule
{
    [CmdletBinding()]
    [OutputType([Guid])]
    param
    (
        [Parameter(Mandatory=$true)]
        [String]
        $DisplayName,
        [Parameter(Mandatory=$false)]
        [String]
        [ValidateNotNullOrEmpty()]
        $Description,
        [Parameter(Mandatory=$false)]
        [Switch]
        $Enabled = $true,

        [Parameter(Mandatory=$true, ParameterSetName="TransitionIn")]
        [Switch]
        $TransitionIn,
        [Parameter(Mandatory=$true, ParameterSetName="TransitionOut")]
        [Switch]
        $TransitionOut,
        [Parameter(Mandatory=$true, ParameterSetName="TransitionIn")]        
        [Parameter(ParameterSetName="TransitionOut")]
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Array] -and $_.Length -eq 3) })]
        $TransitionSet,
        
        [Parameter(Mandatory=$true, ParameterSetName="Request")]
        [Switch]
        $Request,
        [Parameter(Mandatory=$true, ParameterSetName="Request")]
        [ValidateSet('Read','Create','Modify','Delete','Add','Remove')]
        [String[]]
        $RequestType,
        [Parameter(Mandatory=$false, ParameterSetName="Request")]
        [Switch]
        $GrantPermission = $false,
        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        [String[]]
        $ResourceAttributeNames,

        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Array] -and $_.Length -eq 3) })]        
        $RequestorSet,

        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        [String]
        [ValidateNotNullOrEmpty()]
        $RelativeToResourceAttributeName,

        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Array] -and $_.Length -eq 3) })]
        $ResourceSetBeforeRequest,
        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Array] -and $_.Length -eq 3) })]
        $ResourceSetAfterRequest,

        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        [Parameter(ParameterSetName="TransitionIn")]
        [Parameter(ParameterSetName="TransitionOut")]
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Guid[]]) -or ($_ -is [Array] -and $_.Length -eq 3) -or ($_ -is [Array] -and $_[0].Length -eq 3) })]
        $AuthenticationWorkflowDefinition,
        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        [Parameter(ParameterSetName="TransitionIn")]
        [Parameter(ParameterSetName="TransitionOut")]
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Guid[]]) -or ($_ -is [Array] -and $_.Length -eq 3) -or ($_ -is [Array] -and $_[0].Length -eq 3) })]
        $AuthorizationWorkflowDefinition,
        [Parameter(Mandatory=$false, ParameterSetName="Request")]        
        [Parameter(ParameterSetName="TransitionIn")]
        [Parameter(ParameterSetName="TransitionOut")]
        #[ValidateScript({ ($_ -is [Guid]) -or ($_ -is [Guid[]]) -or ($_ -is [Array] -and $_.Length -eq 3) -or ($_ -is [Array] -and $_[0].Length -eq 3) })]
        $ActionWorkflowDefinition,

        [Parameter(Mandatory=$false)]
        [ValidateNotNull()]
        [String]
        $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $false           
    )
    begin
    {
		# Create a table of request types we received
		$requestTypeSet = New-Object -TypeName "System.Collections.Generic.HashSet[string]" -ArgumentList ([StringComparer]::OrdinalIgnoreCase)
		$RequestType | % { [void]($requestTypeSet.Add($_)) }

        if ($PSCmdlet.ParameterSetName -eq "Request")
        {
            if ((
                $requestTypeSet.Contains("Read") -or 
                $requestTypeSet.Contains("Modify") -or
                $requestTypeSet.Contains("Remove") -or
                $requestTypeSet.Contains("Add") -or
                $requestTypeSet.Contains("Delete")
                ) -eq $false)
            {
                if ($ResourceSetBeforeRequest)
                {
                    throw "-ResourceSetBeforeRequest is only necessary for Read, Modify, Remove, and Delete requests"
                }                

            }
            else
            {
                if (-not $ResourceSetBeforeRequest)
                {
                    throw "-ResourceSetBeforeRequest is required for Read, Modify, Add, Remove, and Delete requests"
                }
            }

            if ((
                $requestTypeSet.Contains("Modify") -or
                $requestTypeSet.Contains("Add") -or
                $requestTypeSet.Contains("Remove") -or
                $requestTypeSet.Contains("Create")
                ) -eq $false)
            {
                if ($ResourceSetAfterRequest)
                {
                    throw "-ResourceSetAfterRequest is only necessary for Create, Modify, Add, and Remove requests"
                }                

            }
            else
            {
                if (-not $ResourceSetAfterRequest)
                {
                    throw "-ResourceSetAfterRequest is required for Create, Modify, Add, and Remove requests"
                }
            }

            if (($requestTypeSet.Count -eq 1) -and ($requestTypeSet.Contains("Delete")))
            {
                if ($ResourceAttributeNames)
                {
                    throw "-ResourceAttributeNames is only necessary for  Create, Modify, Add, and Remove requests"
                }
            }
            else
            {
                if (-not $ResourceAttributeNames)
                {
                    throw "-ResourceSetAfterRequest is required for Create, Modify, Add, and Remove requests"
                }
            }

            if ($RequestorSet -and $RelativeToResourceAttributeName)
            {
                throw "-RequestorSet and -RelativeToResourceAttributeName cannot both be specified"
            }

            if (($RequestorSet -eq $null) -and ($RelativeToResourceAttributeName -eq $null))
            {
                throw "Specify either -RequestorSet or -RelativeToResourceAttributeName"
            }
        }

        $changeSet = @()

        $changeSet += New-FimImportChange -Operation Replace -AttributeName "DisplayName" -AttributeValue $DisplayName
        #this is required on set transition MPRs as well as Request MPRs
        #it will always be false on set transitions (as expected) do to parameter sets
        $changeSet += New-FimImportChange -Operation Replace -AttributeName "GrantRight" -AttributeValue $GrantPermission.ToBool()

        if ($Description)
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "Description" -AttributeValue $Description
        }

        $disableValue = (-not $Enabled.ToBool())        
        $changeSet += New-FimImportChange -Operation Replace -AttributeName "Disabled" -AttributeValue $disableValue

        if (($PSCmdlet.ParameterSetName -eq "TransitionIn") -or
            ($PSCmdlet.ParameterSetName -eq "TransitionOut")
           )
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "ManagementPolicyRuleType" -AttributeValue "SetTransition"
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "ActionType" -AttributeValue $PSCmdlet.ParameterSetName            
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "ActionParameter" -AttributeValue "*"

            if ($PSCmdlet.ParameterSetName -eq "TransitionIn")
            {
                $changeSet += New-FimImportChange -Operation Replace -AttributeName "ResourceFinalSet" -AttributeValue $TransitionSet
            }
            elseif ($PSCmdlet.ParameterSetName -eq "TransitionOut")
            {
                $changeSet += New-FimImportChange -Operation Replace -AttributeName "ResourceCurrentSet" -AttributeValue $TransitionSet
            }
            else
            {
                throw "Unsupported parameter set name"
            }
        }

        if ($PSCmdlet.ParameterSetName -eq "Request")
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "ManagementPolicyRuleType" -AttributeValue "Request"            

            foreach ($type in $requestTypeSet)
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "ActionType" -AttributeValue $type
            }

            $actionParameters = $ResourceAttributeNames
            if (($requestTypeSet.Count -eq 1) -and ($requestTypeSet.Contains("Delete")))
            {
                $actionParameters = @("*")
            }

            foreach ($param in $actionParameters)
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "ActionParameter" -AttributeValue $param
            }

            if ($RelativeToResourceAttributeName)
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "PrincipalRelativeToResource" -AttributeValue $RelativeToResourceAttributeName
            }
            elseif ($RequestorSet)
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "PrincipalSet" -AttributeValue $RequestorSet
            }
            else
            {
                throw "Requestor not defined"
            }

            if ($ResourceSetBeforeRequest)
            {
                $changeSet += New-FimImportChange -Operation Replace -AttributeName "ResourceCurrentSet" -AttributeValue $ResourceSetBeforeRequest
            }

            if ($ResourceSetAfterRequest)
            {
                $changeSet += New-FimImportChange -Operation Replace -AttributeName "ResourceFinalSet" -AttributeValue $ResourceSetAfterRequest
            }
        }


        if ($AuthenticationWorkflowDefinition)
        {
			if (($AuthenticationWorkflowDefinition -is [Guid]) -or
                (($AuthenticationWorkflowDefinition -is [Array]) -and ($AuthenticationWorkflowDefinition.Length -eq 3) -and (-not (($AuthenticationWorkflowDefinition[0] -is [Guid]) -and ($AuthenticationWorkflowDefinition[1] -is [Guid]) -and ($AuthenticationWorkflowDefinition[2] -is [Guid]))))
               )
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "AuthenticationWorkflowDefinition" -AttributeValue $AuthenticationWorkflowDefinition
            }
            elseif (($AuthenticationWorkflowDefinition -is [Guid[]]) -or
                    ($AuthenticationWorkflowDefinition -is [Array])
                   )
            {
                foreach ($wf in $AuthenticationWorkflowDefinition)
                {
                    $changeSet += New-FimImportChange -Operation Add -AttributeName "AuthenticationWorkflowDefinition" -AttributeValue $wf
                }
            }
            else
            {
                throw "Unsupported input for -AuthenticationWorkflowDefinition"
            }
        }

        if ($AuthorizationWorkflowDefinition)
        {
			if (($AuthorizationWorkflowDefinition -is [Guid]) -or
                (($AuthorizationWorkflowDefinition -is [Array]) -and ($AuthorizationWorkflowDefinition.Length -eq 3) -and (-not (($AuthorizationWorkflowDefinition[0] -is [Guid]) -and ($AuthorizationWorkflowDefinition[1] -is [Guid]) -and ($AuthorizationWorkflowDefinition[2] -is [Guid]))))
               )
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "AuthorizationWorkflowDefinition" -AttributeValue $AuthorizationWorkflowDefinition
            }
            elseif (($AuthorizationWorkflowDefinition -is [Guid[]]) -or
                    ($AuthorizationWorkflowDefinition -is [Array])
                   )
            {
                foreach ($wf in $AuthorizationWorkflowDefinition)
                {
                    $changeSet += New-FimImportChange -Operation Add -AttributeName "AuthorizationWorkflowDefinition" -AttributeValue $wf
                }
            }
            else
            {
                throw "Unsupported input for -AuthorizationWorkflowDefinition"
            }
        }

        if ($ActionWorkflowDefinition)
        {
			if (($ActionWorkflowDefinition -is [Guid]) -or
                (($ActionWorkflowDefinition -is [Array]) -and ($ActionWorkflowDefinition.Length -eq 3) -and (-not (($ActionWorkflowDefinition[0] -is [Guid]) -and ($ActionWorkflowDefinition[1] -is [Guid]) -and ($ActionWorkflowDefinition[2] -is [Guid]))))
               )
            {
                $changeSet += New-FimImportChange -Operation Add -AttributeName "ActionWorkflowDefinition" -AttributeValue $ActionWorkflowDefinition
            }
            elseif (($ActionWorkflowDefinition -is [Guid[]]) -or
                    ($ActionWorkflowDefinition -is [Array])
                   )
            {
                foreach ($wf in $ActionWorkflowDefinition)
                {
                    $changeSet += New-FimImportChange -Operation Add -AttributeName "ActionWorkflowDefinition" -AttributeValue $wf
                }
            }
            else
            {
                throw "Unsupported input for -ActionWorkflowDefinition"
            }
        }
        
        New-FimImportObject -ObjectType ManagementPolicyRule -State Create -Changes $changeSet -Uri $Uri -ApplyNow
        
        if ($PassThru.ToBool())
        {
            Write-Output (Get-FimObjectID -ObjectType ManagementPolicyRule -AttributeName DisplayName -AttributeValue $DisplayName)       
        }
    }
}

function New-FimSearchScope
{
    [CmdletBinding()]
    [OutputType([Guid])]
    param
    (
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $DisplayName,
        [parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Description,
        
        [parameter(Mandatory = $true)]
        [ValidateCount(1, 100)]
        [string[]]
        $UsageKeywords,
        [parameter(Mandatory = $true)]
        [ValidateRange(0, 100000)]
        [Int]
        $Order,
        
        [parameter(Mandatory = $true)]
        [ValidateCount(1, 100)]
        [string[]]
        $AttributesToSearch,
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Filter, ##
        [parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $ResultType = "Resource",
        [parameter(Mandatory = $false)]
        [ValidateCount(1, 100)]
        [string[]]
        $AttributesToDisplay,
        [parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $RedirectingUrl,

        [Parameter(Mandatory=$false)]
        [ValidateNotNull()]
        [String]
        $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $false           
    )
    begin
    {
        $changeSet = @(
            New-FimImportChange -Operation Replace -AttributeName "DisplayName" -AttributeValue $DisplayName
            New-FimImportChange -Operation Replace -AttributeName "Order" -AttributeValue $Order
            New-FimImportChange -Operation Replace -AttributeName "SearchScope" -AttributeValue $Filter
            New-FimImportChange -Operation Replace -AttributeName "SearchScopeResultObjectType" -AttributeValue $ResultType            
            New-FimImportChange -Operation Replace -AttributeName "SearchScopeColumn" -AttributeValue ($AttributesToDisplay -join ";")
            New-FimImportChange -Operation Replace -AttributeName "IsConfigurationType" -AttributeValue $true
        )

        if ($RedirectingUrl)
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "SearchScopeTargetURL" -AttributeValue $RedirectingUrl
        }

        if ($Description)
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "Description" -AttributeValue $Description
        }

        foreach ($keyword in $UsageKeywords)
        {
            $changeSet += New-FimImportChange -Operation Add -AttributeName "UsageKeyword" -AttributeValue $keyword
        }

        foreach ($attr in $AttributesToSearch)
        {
            $changeSet += New-FimImportChange -Operation Add -AttributeName "SearchScopeContext" -AttributeValue $attr
        }

        New-FimImportObject -ObjectType SearchScopeConfiguration -State Create -Changes $changeSet -Uri $Uri -ApplyNow

        if ($PassThru.ToBool())
        {
            Write-Output (Get-FimObjectID -ObjectType SearchScopeConfiguration -AttributeName DisplayName -AttributeValue $DisplayName)
        }
    }
}

function New-FimNavigationBarLink
{
    [CmdletBinding(DefaultParameterSetName="ChildLink")]
    [OutputType([Guid])]
    param
    (
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $DisplayName,
        [parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Description, 
        [parameter(Mandatory = $true)]
        [ValidateCount(1, 100)]
        [string[]]
        $UsageKeywords,

        [parameter(Mandatory = $true, ParameterSetName = "TopLevel")]
        [switch]
        $TopLevel,
        [parameter(Mandatory = $true)]
        [Int]
        $ParentOrder,
        [parameter(Mandatory = $true, ParameterSetName = "ChildLink")]        
        [Int]
        $ChildOrder,
        
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NavigationUrl,
        [parameter(Mandatory = $false)]
        [ValidateNotNullOrEmpty()]
        [string]
        $ResourceCountFilter,

        [Parameter(Mandatory=$false)]
        [ValidateNotNull()]
        [String]
        $Uri = "http://localhost:5725",
        [parameter(Mandatory=$false)]
        [Switch]
        $PassThru = $false
    )
    begin
    {
        $order = -1

        if ($PSCmdlet.ParameterSetName -eq "Toplevel")
        {
            $order = 0
        }
        else
        {
            $order = $ChildOrder
        }

        $changeSet = @(
            New-FimImportChange -Operation Replace -AttributeName "DisplayName" -AttributeValue $DisplayName
            New-FimImportChange -Operation Replace -AttributeName "NavigationUrl" -AttributeValue $NavigationUrl
            New-FimImportChange -Operation Replace -AttributeName "Order" -AttributeValue $order
            New-FimImportChange -Operation Replace -AttributeName "ParentOrder" -AttributeValue $ParentOrder
            New-FimImportChange -Operation Replace -AttributeName "IsConfigurationType" -AttributeValue $true
        )

        if ($Description)
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "Description" -AttributeValue $Description
        }

        foreach ($keyword in $UsageKeywords)
        {
            $changeSet += New-FimImportChange -Operation Add -AttributeName "UsageKeyword" -AttributeValue $keyword
        }

        if ($ResourceCountFilter)
        {
            $changeSet += New-FimImportChange -Operation Replace -AttributeName "CountXPath" -AttributeValue $ResourceCountFilter
        }

        New-FimImportObject -ObjectType NavigationBarConfiguration -State Create -Uri $Uri -Changes $changeSet -ApplyNow

        if ($PassThru.ToBool())
        {
            Write-Output (Get-FimObjectID -ObjectType NavigationBarConfiguration -AttributeName DisplayName -AttributeValue $DisplayName)
        }
    }
}

<#
    .SYNOPSIS 
   Writes a tree structure of the synchronization rule hierarchy to the console

    .PARAMETER Direction
    The type of synchronization rule to filter on

    .OUTPUTS
    Text representing the rule hierachy

#>
function Get-FimSynchronizationRuleDependencyTree
{
    param(
        [ValidateSet('Inbound','Outbound', 'InboundAndOutbound')]
        $Direction,
	    [String]
	    $Uri = 'http://localhost:5725'
    )

    $flowType = $null    
    if ($Direction -eq 'Inbound')
    {
        $flowType = 0
    }
    elseif ($Direction -eq 'Outbound')
    {
        $flowType = 1
    }
    elseif ($Direction -eq 'InboundAndOutbound')
    {
        $flowType = 2
    }
    else
    {
        throw "Unsupported value for `$Direction"
    }

    $syncRules = Export-FIMConfig -OnlyBaseResources -CustomConfig "/SynchronizationRule[FlowType='$flowType']" -Uri $Uri | Convert-FimExportToPSObject    

    foreach ($rule in ($syncRules | Sort-Object -Property 'DisplayName'))
    {
        if ($rule.Dependency -eq $null)
        {
            InternalPrintSyncRule -Rule $rule -RuleSet $syncRules -Level 0
        }
    }    
}

<#
.SYNOPSIS
    This is an internal function that supports Write-FimSynchronizationRuleDependencyTree. It is not intended for consumption

.NOTES
    This is exported due to legacy compatibility issues with the module.
#>
function InternalPrintSyncRule
{
    param(
    
        $Rule,
        $RuleSet,
        [int]
        $Level
    )   

    if ($Level -eq 0)
    {
        Write-Host $Rule.DisplayName
    }
    else
    {
        Write-Host ('|' + '_' * $Level + $Rule.DisplayName)
    }

    foreach ($dependent in ($RuleSet | Where {$_.Dependency -eq $Rule.ObjectID} | Sort-Object -Property 'DisplayName')){
        PrintSyncRule -Rule $dependent -RuleSet $RuleSet -Level ($Level + 1)
    }
}

 # backwards compat for the old names of these functions
 New-Alias -Name Add-FimSchemaBinding -Value New-FimSchemaBinding
 New-Alias -Name Add-FimSchemaAttribute -Value New-FimSchemaAttribute
 New-Alias -Name Add-FimSchemaObject -Value New-FimSchemaObjectType
 New-Alias -Name Add-FimSet -Value New-FimSet

 # this is required because aliases aren't
 # exported by default
 Export-ModuleMember -Function * -Alias *
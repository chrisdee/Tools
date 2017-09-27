<#
    .SYNOPSIS 
    Gets the Import Attribute Flow Rules from Sync Server Configuration

    .DESCRIPTION
    Reads the server configuration from the XML files, and outputs the Import Attribute Flow rules as PSObjects

    .OUTPUTS
    PSObjects containing the synchronization server import attribute flow rules
   
    .EXAMPLE
    Get-ImportAttributeFlow -ServerConfigurationFolder "E:\ServerConfiguration" | out-gridview
#>
Function Get-ImportAttributeFlow
{
  Param
  (        
    [parameter(Mandatory=$false)]
    [String]
    [ValidateScript({Test-Path $_})]
    $ServerConfigurationFolder
  ) 
  End
  {         
    ### This is where the rules will be aggregated before we output them
    $rules = @()

    ###
    ### Loop through the management agent XMLs to get the Name:GUID mapping
    ###
    $maList = @{}
    $maFiles = (get-item (join-path $ServerConfigurationFolder "*.xml"))
    foreach ($maFile in $maFiles)
    {
      ### Skip the file if it does NOT contain an ma-data node
      if (select-xml $maFile -XPath "//ma-data" -ErrorAction 0)
      {
        ### Get the MA Name and MA ID
        $maName = (select-xml $maFile -XPath "//ma-data/name").Node.InnerText
        $maID = (select-xml $maFile -XPath "//ma-data/id").Node.InnerText  
                       
        $maList.Add($maID,$maName)
      }
    }

    ###
    ### Get:
    ###    mv-object-type
    ###      mv-attribute
    ###        src-ma
    ###        cd-object-type
    ###          src-attribute
    ###
    [xml]$mv = get-content (join-path $ServerConfigurationFolder "MV.xml")

    foreach($importFlowSet in $mv.selectNodes("//import-flow-set"))
    {
      $mvObjectType = $importFlowSet.'mv-object-type'
                          
      foreach($importFlows in $importFlowSet.'import-flows')
      {
        $mvAttribute = $importFlows.'mv-attribute'        
        $precedenceType = $importFlows.type
        $precedenceRank = 0
                        
        foreach($importFlow in $importFlows.'import-flow')
        {
          $cdObjectType = $importFlow.'cd-object-type'
          $srcMA = $maList[$importFlow.'src-ma']
          $maID = $importFlow.'src-ma'
          $maName = $maList[$maID]                 
                                     
          if ($importFlow.'direct-mapping' -ne $null)
          {
            if ($precedenceType -eq 'ranked')
            {
              $precedenceRank += 1
            }
            else
            {
              $precedenceRank = $null
            }
                                 
            ###
            ### Handle src-attribute that are intinsic (<src-attribute intrinsic="true">dn</src-attribute>)
            ###
            if ($importFlow.'direct-mapping'.'src-attribute'.intrinsic)
            {
              $srcAttribute = "<{0}>" -F $importFlow.'direct-mapping'.'src-attribute'.'#text'
            }
            else
            {
              $srcAttribute = $importFlow.'direct-mapping'.'src-attribute'
            }
            $rule = New-Object PSObject
            $rule | Add-Member -MemberType noteproperty -name 'RuleType' -value 'DIRECT'
            $rule | Add-Member -MemberType noteproperty -name 'SourceMA' -value $srcMA
            $rule | Add-Member -MemberType noteproperty -name 'CDObjectType' -value $cdObjectType
            $rule | Add-Member -MemberType noteproperty -name 'CDAttribute' -value $srcAttribute
            $rule | Add-Member -MemberType noteproperty -name 'MVObjectType' -value $mvObjectType
            $rule | Add-Member -MemberType noteproperty -name 'MVAttribute' -value $mvAttribute
            $rule | Add-Member -MemberType noteproperty -name 'ScriptContext' -value $null
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceType' -value $precedenceType
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceRank' -value $precedenceRank
                             
            $rules += $rule                               
          }
          elseif ($importFlow.'scripted-mapping' -ne $null)
          {                
            $scriptContext = $importFlow.'scripted-mapping'.'script-context'  

            ###
            ### Handle src-attribute that are intrinsic (<src-attribute intrinsic="true">dn</src-attribute>)
            ###              
            $srcAttributes = @()
            $importFlow.'scripted-mapping'.'src-attribute' | ForEach-Object {
              if ($_.intrinsic)
              {
                $srcAttributes += "<{0}>" -F $_.'#text'
              }
              else
              {
                $srcAttributes += $_
              }
            }
            if ($srcAttributes.Count -eq 1)
            {
              $srcAttributes = $srcAttributes -as [String]
            }
                                       
            if ($precedenceType -eq 'ranked')
            {
              $precedenceRank += 1
            }
            else
            {
              $precedenceRank = $null
            }
                             
            $rule = New-Object PSObject
            $rule | Add-Member -MemberType noteproperty -name 'RuleType' -value 'SCRIPTED'
            $rule | Add-Member -MemberType noteproperty -name 'SourceMA' -value $srcMA
            $rule | Add-Member -MemberType noteproperty -name 'CDObjectType' -value $cdObjectType
            $rule | Add-Member -MemberType noteproperty -name 'CDAttribute' -value $srcAttributes
            $rule | Add-Member -MemberType noteproperty -name 'MVObjectType' -value $mvObjectType
            $rule | Add-Member -MemberType noteproperty -name 'MVAttribute' -value $mvAttribute
            $rule | Add-Member -MemberType noteproperty -name 'ScriptContext' -value $scriptContext
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceType' -value $precedenceType
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceRank' -value $precedenceRank
                                             
            $rules += $rule                        
          }   
          elseif ($importFlow.'sync-rule-mapping' -ne $null)
          {                
            $scriptContext = $null 
            $ruleType = ("ISR - {0}" -f $importFlow.'sync-rule-mapping'.'mapping-type')
            $srcAttributes = $importFlow.'sync-rule-mapping'.'src-attribute'    
                                       
            if ($precedenceType -eq 'ranked')
            {
              $precedenceRank += 1
            }
            else
            {
              $precedenceRank = $null
            }
                                       
            if ($importFlow.'sync-rule-mapping'.'mapping-type' -ieq 'expression')
            {
              $scriptContext = $importFlow.'sync-rule-mapping'.'sync-rule-value'.'import-flow'.InnerXml
            }
            elseif ($importFlow.'sync-rule-mapping'.'mapping-type' -ieq 'constant')
            {
              $scriptContext = $importFlow.'sync-rule-mapping'.'sync-rule-value'
            }
                             
            $rule = New-Object PSObject
            $rule | Add-Member -MemberType noteproperty -name 'RuleType' -value $ruleType
            $rule | Add-Member -MemberType noteproperty -name 'SourceMA' -value $srcMA
            $rule | Add-Member -MemberType noteproperty -name 'CDObjectType' -value $cdObjectType
            $rule | Add-Member -MemberType noteproperty -name 'CDAttribute' -value $srcAttributes
            $rule | Add-Member -MemberType noteproperty -name 'MVObjectType' -value $mvObjectType
            $rule | Add-Member -MemberType noteproperty -name 'MVAttribute' -value $mvAttribute
            $rule | Add-Member -MemberType noteproperty -name 'ScriptContext' -value $scriptContext
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceType' -value $precedenceType
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceRank' -value $precedenceRank
                                             
            $rules += $rule                        
          }
          elseif ($importFlow.'constant-mapping' -ne $null)
          {
            if ($precedenceType -eq 'ranked')
            {
              $precedenceRank += 1
            }
            else
            {
              $precedenceRank = $null
            }

                                 
            $constantValue = $importFlow.'constant-mapping'.'constant-value'
                                       
            $rule = New-Object PSObject
            $rule | Add-Member -MemberType noteproperty -name 'RuleType' -value "CONSTANT"
            $rule | Add-Member -MemberType noteproperty -name 'SourceMA' -value $srcMA
            $rule | Add-Member -MemberType noteproperty -name 'CDObjectType' -value $cdObjectType
            $rule | Add-Member -MemberType noteproperty -name 'CDAttribute' -value $null
            $rule | Add-Member -MemberType noteproperty -name 'MVObjectType' -value $mvObjectType
            $rule | Add-Member -MemberType noteproperty -name 'MVAttribute' -value $mvAttribute
            $rule | Add-Member -MemberType noteproperty -name 'ScriptContext' -value $null
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceType' -value $precedenceType
            $rule | Add-Member -MemberType noteproperty -name 'PrecedenceRank' -value $precedenceRank
            $rule | Add-Member -MemberType noteproperty -name 'ConstantValue' -value $constantValue
                                             
            $rules += $rule                        
                                       
          }
        }#foreach($importFlow in $importFlows.'import-flow')
      }#foreach($importFlows in $importFlowSet.'import-flows')
    }#foreach($importFlowSet in $mv.selectNodes("//import-flow-set"))
             
    Write-Output $rules
  }#End
}


<#
    .SYNOPSIS 
    Gets the Export Attribute Flow Rules from Sync Server Configuration

    .DESCRIPTION
    Reads the server configuration from the XML files, and outputs the Export Attribute Flow rules as PSObjects

    .OUTPUTS
    PSObjects containing the synchronization server export attribute flow rules
   
    .EXAMPLE
    Get-ExportAttributeFlow -ServerConfigurationFolder "E:\sd\IAM\ITAuthorize\Source\Configuration\FimSync\ServerConfiguration"

#>
Function Get-ExportAttributeFlow
{
  Param
  (        
    [parameter(Mandatory=$false)]
    [String]
    [ValidateScript({Test-Path $_})]
    $ServerConfigurationFolder
  ) 
  End
  {         
    ### This is where the rules will be aggregated before we output them
    $rules = @()
             
    ### Export attribute flow rules are contained in the ma-data nodes of the MA*.XML files
    $maFiles = @(get-item (Join-Path $ServerConfigurationFolder "MA-*.xml"))
             
              
    foreach ($maFile in $maFiles)
    {
      ### Get the MA Name and MA ID
      $maName = (select-xml $maFile -XPath "//ma-data/name").Node.InnerText
                
      foreach($exportFlowSet in (Select-Xml -path $maFile -XPath "//export-flow-set" | select -ExpandProperty Node))
      {
        $mvObjectType = $exportFlowSet.'mv-object-type'
        $cdObjectType = $exportFlowSet.'cd-object-type'
                     
        foreach($exportFlow in $exportFlowSet.'export-flow')
        {
          $cdAttribute = $exportFlow.'cd-attribute'
          [bool]$allowNulls = $false
          if ([bool]::TryParse($exportFlow.'suppress-deletions', [ref]$allowNulls))
          {
            $allowNulls = -not $allowNulls
          }

          [string]$initialFlowOnly = $null
          [string]$isExistenceTest = $null
                      
   
          if ($exportFlow.'direct-mapping' -ne $null)
          {
            ###
            ### Handle src-attribute that are intrinsic (<src-attribute intrinsic="true">object-id</src-attribute>)
            ###
            if ($exportFlow.'direct-mapping'.'src-attribute'.intrinsic)
            {
              $srcAttribute = "<{0}>" -F $exportFlow.'direct-mapping'.'src-attribute'.'#text'
            }
            else
            {
              $srcAttribute = $exportFlow.'direct-mapping'.'src-attribute'
            }
                             
            $rule = New-Object PSObject
            $rule | Add-Member -MemberType NoteProperty -Name 'RuleType' -Value 'DIRECT'
            $rule | Add-Member -MemberType NoteProperty -Name 'MAName' -Value $maName                
            $rule | Add-Member -MemberType NoteProperty -Name 'MVObjectType' -Value $mvObjectType
            $rule | Add-Member -MemberType NoteProperty -Name 'MVAttribute' -Value $srcAttribute
            $rule | Add-Member -MemberType NoteProperty -Name 'CDObjectType' -Value $cdObjectType
            $rule | Add-Member -MemberType NoteProperty -Name 'CDAttribute' -Value $cdAttribute
            $rule | Add-Member -MemberType NoteProperty -Name 'ScriptContext' -Value $null
            $rule | Add-Member -MemberType NoteProperty -Name 'AllowNulls' -Value $allowNulls
            $rule | Add-Member -MemberType NoteProperty -Name 'InitialFlowOnly' -Value $initialFlowOnly
            $rule | Add-Member -MemberType NoteProperty -Name 'IsExistenceTest' -Value $isExistenceTest

                             
            $rules += $rule
          }
          elseif ($exportFlow.'scripted-mapping' -ne $null)
          {                
            $scriptContext = $exportFlow.'scripted-mapping'.'script-context'                             
            $srcAttributes = @()
                                       
            ###
            ### Handle src-attribute that are intrinsic (<src-attribute intrinsic="true">object-id</src-attribute>)
            ###
            $exportFlow.'scripted-mapping'.'src-attribute' | ForEach-Object {
              if ($_.intrinsic)
              {
                $srcAttributes += "<{0}>" -F $_.'#text'
              }
              elseif ($_) # Do not add empty values.
              {
                $srcAttributes += $_
              }
            }
            # (Commented) Leave as collection
            #if ($srcAttributes.Count -eq 1)
            #{
            #    $srcAttributes = $srcAttributes -as [String]
            #}
                                 
            $rule = New-Object PSObject
            $rule | Add-Member -MemberType NoteProperty -Name 'RuleType' -Value 'SCRIPTED'
            $rule | Add-Member -MemberType NoteProperty -Name 'MAName' -Value $maName
            $rule | Add-Member -MemberType NoteProperty -Name 'MVObjectType' -Value $mvObjectType
            $rule | Add-Member -MemberType NoteProperty -Name 'MVAttribute' -Value $srcAttributes
            $rule | Add-Member -MemberType NoteProperty -Name 'CDObjectType' -Value $cdObjectType
            $rule | Add-Member -MemberType NoteProperty -Name 'CDAttribute' -Value $cdAttribute 
            $rule | Add-Member -MemberType NoteProperty -Name 'ScriptContext' -Value $scriptContext
            $rule | Add-Member -MemberType NoteProperty -Name 'AllowNulls' -Value $allowNulls
            $rule | Add-Member -MemberType NoteProperty -Name 'InitialFlowOnly' -Value $initialFlowOnly
            $rule | Add-Member -MemberType NoteProperty -Name 'IsExistenceTest' -Value $isExistenceTest
                                             
            $rules += $rule                        
          }
          elseif ($exportFlow.'sync-rule-mapping' -ne $null)
          {
            $srcAttribute = $exportFlow.'sync-rule-mapping'.'src-attribute'
            $initialFlowOnly = $exportFlow.'sync-rule-mapping'.'initial-flow-only'
            $isExistenceTest = $exportFlow.'sync-rule-mapping'.'is-existence-test'

            if($exportFlow.'sync-rule-mapping'.'mapping-type' -eq 'direct')
            {
              $rule = New-Object PSObject
              $rule | Add-Member -MemberType NoteProperty -Name 'RuleType' -Value 'OSR-Direct'
              $rule | Add-Member -MemberType NoteProperty -Name 'MAName' -Value $maName
              $rule | Add-Member -MemberType NoteProperty -Name 'MVObjectType' -Value $mvObjectType
              $rule | Add-Member -MemberType NoteProperty -Name 'MVAttribute' -Value $srcAttribute
              $rule | Add-Member -MemberType NoteProperty -Name 'CDObjectType' -Value $cdObjectType
              $rule | Add-Member -MemberType NoteProperty -Name 'CDAttribute' -Value $cdAttribute                                                                                            
              $rule | Add-Member -MemberType NoteProperty -Name 'ScriptContext' -Value $null
              $rule | Add-Member -MemberType NoteProperty -Name 'AllowNulls' -Value $allowNulls
              $rule | Add-Member -MemberType NoteProperty -Name 'InitialFlowOnly' -Value $initialFlowOnly
              $rule | Add-Member -MemberType NoteProperty -Name 'IsExistenceTest' -Value $isExistenceTest
                                                                        
              $rules += $rule             
            }
            elseif ($exportFlow.'sync-rule-mapping'.'mapping-type' -eq 'expression')
            {
              $scriptContext = $exportFlow.'sync-rule-mapping'.'sync-rule-value'.'export-flow'.InnerXml
              $cdAttribute  = $exportFlow.'sync-rule-mapping'.'sync-rule-value'.'export-flow'.dest

              $rule = New-Object PSObject
              $rule | Add-Member -MemberType NoteProperty -Name 'RuleType' -Value 'OSR-Expression'
              $rule | Add-Member -MemberType NoteProperty -Name 'MAName' -Value $maName
              $rule | Add-Member -MemberType NoteProperty -Name 'MVObjectType' -Value $mvObjectType
              $rule | Add-Member -MemberType NoteProperty -Name 'MVAttribute' -Value $srcAttribute
              $rule | Add-Member -MemberType NoteProperty -Name 'CDObjectType' -Value $cdObjectType
              $rule | Add-Member -MemberType NoteProperty -Name 'CDAttribute' -Value $cdAttribute                                                                                            
              $rule | Add-Member -MemberType NoteProperty -Name 'ScriptContext' -Value $scriptContext
              $rule | Add-Member -MemberType NoteProperty -Name 'AllowNulls' -Value $allowNulls
              $rule | Add-Member -MemberType NoteProperty -Name 'InitialFlowOnly' -Value $initialFlowOnly
              $rule | Add-Member -MemberType NoteProperty -Name 'IsExistenceTest' -Value $isExistenceTest
                                                                        
              $rules += $rule             
            }
            elseif ($exportFlow.'sync-rule-mapping'.'mapping-type' -eq 'constant')
            {
              $rule = New-Object PSObject
              $rule | Add-Member -MemberType NoteProperty -Name 'RuleType' -Value 'OSR-Constant'
              $rule | Add-Member -MemberType NoteProperty -Name 'MAName' -Value $maName
              $rule | Add-Member -MemberType NoteProperty -Name 'MVObjectType' -Value $mvObjectType
              $rule | Add-Member -MemberType NoteProperty -Name 'MVAttribute' -Value $null
              $rule | Add-Member -MemberType NoteProperty -Name 'CDObjectType' -Value $cdObjectType
              $rule | Add-Member -MemberType NoteProperty -Name 'CDAttribute' -Value $cdAttribute                                                                                            
              #$rule | Add-Member -MemberType NoteProperty -Name 'ScriptContext' -Value $null
              $rule | Add-Member -MemberType NoteProperty -Name 'AllowNulls' -Value $false
              $rule | Add-Member -MemberType NoteProperty -Name 'InitialFlowOnly' -Value $initialFlowOnly
              $rule | Add-Member -MemberType NoteProperty -Name 'IsExistenceTest' -Value $isExistenceTest   
              $rule | Add-Member -MemberType NoteProperty -Name 'ScriptContext' -Value ($exportFlow.'sync-rule-mapping'.'sync-rule-value')
                                                                        
              $rules += $rule             
            }
            else
            {
              throw "UnsupportedExport Flow type '$($exportFlow.'sync-rule-mapping'.'mapping-type')'"
            }
                               
          }
        }
      }
    }
             
    Write-Output $rules
  }#End
}

<#
    .SYNOPSIS 
    Gets the Joined Rules where the IAF rules are joined to the EAF rules based on the MV Attributes and Object Types

    .DESCRIPTION
    Reads the server configuration from the XML files, and outputs the Joined IAF and EAF Rules as PSObjects

    .OUTPUTS
    PSObjects containing the synchronization server attribute flow rules
   
    .EXAMPLE
    Join-ImportToExportAttributeFlow -ServerConfigurationFolder "E:\sd\IAM\ITAuthorize\Source\Configuration\FimSync\ServerConfiguration"
   
#>
Function Join-ImportToExportAttributeFlow
{
  Param
  (        
    [parameter(Mandatory=$false)]
    [String]
    [ValidateScript({Test-Path $_})]
    $ServerConfigurationFolder
  ) 
  End
  {
    ### Get the Import Attribute Flow Rules
    $IAF = Get-ImportAttributeFlow -ServerConfigurationFolder $ServerConfigurationFolder
             
    ### Get the Export Attribute Flow Rules
    $EAF = Get-ExportAttributeFlow -ServerConfigurationFolder $ServerConfigurationFolder

    ### This is where the rules will be aggregated before we output them
    $e2eFlowRules = @()
    foreach ($iafRule in $IAF)
    {
      ### Look for a corresponding EAF rule    
      $eafMatches = @($EAF | where {$_.'MVAttribute' -contains $iafRule.'MVAttribute' -and $_.'MVObjectType' -eq $iafRule.'MVObjectType'})

      ### There may be multiple EAF rule for each IAF rules
      if ($eafMatches.count -gt 0)
      {
        foreach($eafRule in $eafMatches)
        {                        
          $e2eFlowRuleProperties = @{            
            'IAFRuleType'          = $iafRule.'RuleType'
            'IAFSourceMA'          = $iafRule.'SourceMA'
            'IAFCDObjectType'      = $iafRule.'CDObjectType'
            'IAFCDAttribute'       = $iafRule.'CDAttribute'
            'IAFScriptContext'     = $iafRule.'ScriptContext'
            'IAFPrecedenceType'    = $iafRule.'PrecedenceType'
            'IAFPrecedenceRank'    = $iafRule.'PrecedenceRank'
            'MVObjectType'         = $iafRule.'MVObjectType'
            'MVAttribute'          = $iafRule.'MVAttribute'
            'EAFMVAttribute'       = $eafRule.'MVAttribute'
            'EAFCDAttribute'       = $eafRule.'CDAttribute'
            'EAFTargetMA'          = $eafRule.'MAName'
            'EAFCDObjectType'      = $eafRule.'CDObjectType'
            'EAFRuleType'          = $eafRule.'RuleType'
            'EAFScriptContext'     = $eafRule.'ScriptContext'
            'EAFAllowNulls'        = $eafRule.'AllowNulls'
            'EAFInitialFlowOnly'   = $eafRule.'InitialFlowOnly'
            'EAFIsExistenceTest'   = $eafRule.'IsExistenceTest'

          }
                         
          $e2eFlowRules += New-Object PSObject -Property $e2eFlowRuleProperties 
        }
      }
      ### It is possible there are NO EAF rules for an IAF rule
      ### here we stuff $null into the EAF side to make our output easy to consume for Out-GridView and Compare-Object
      ### otherwise jagged objects seem to confuse things
      ###
      ### In this case the rule may be useless
      ### Or the use of that MV attribute may not be visible here because some rules extension calls it (need to check the source code to confirm)
      else
      {
        $e2eFlowRuleProperties = @{            
          'IAFRuleType'          = $iafRule.'RuleType'
          'IAFSourceMA'          = $iafRule.'SourceMA'
          'IAFCDObjectType'      = $iafRule.'CDObjectType'
          'IAFCDAttribute'       = $iafRule.'CDAttribute'
          'IAFScriptContext'     = $iafRule.'ScriptContext'
          'IAFPrecedenceType'    = $iafRule.'PrecedenceType'
          'IAFPrecedenceRank'    = $iafRule.'PrecedenceRank'
          'MVObjectType'         = $iafRule.'MVObjectType'
          'MVAttribute'          = $iafRule.'MVAttribute'
          'EAFMVAttribute'       = $null
          'EAFCDAttribute'       = $null
          'EAFTargetMA'          = $null
          'EAFCDObjectType'      = $null
          'EAFRuleType'          = $null
          'EAFScriptContext'     = $null
          'EAFAllowNulls'        = $null
          'EAFInitialFlowOnly'   = $null
          'EAFIsExistenceTest'   = $null

        }
                         
        $e2eFlowRules += New-Object PSObject -Property $e2eFlowRuleProperties 
      }
    }

    ### There's one more case in which the MV attribute is blank for an EAF.
    foreach ($eafRule in @($EAF | where {$_.'MVAttribute'.count -eq 0}))
    {
      Write-Verbose $eafRule
      $e2eFlowRuleProperties = @{            
        'IAFRuleType'          = $null
        'IAFSourceMA'          = $null
        'IAFCDObjectType'      = $null
        'IAFCDAttribute'       = $null
        'IAFScriptContext'     = $null
        'IAFPrecedenceType'    = $null
        'IAFPrecedenceRank'    = $null
        'MVObjectType'         = $null
        'MVAttribute'          = $null
        'EAFMVAttribute'       = $eafRule.'MVAttribute'
        'EAFCDAttribute'       = $eafRule.'CDAttribute'
        'EAFTargetMA'          = $eafRule.'MAName'
        'EAFCDObjectType'      = $eafRule.'CDObjectType'
        'EAFRuleType'          = $eafRule.'RuleType'
        'EAFScriptContext'     = $eafRule.'ScriptContext'
        'EAFAllowNulls'        = $eafRule.'AllowNulls' 
        'EAFInitialFlowOnly'   = $eafRule.'InitialFlowOnly' 
        'EAFIsExistenceTest'   = $eafRule.'IsExistenceTest' 
      }
                         
      $e2eFlowRules += New-Object PSObject -Property $e2eFlowRuleProperties 
    }


    $e2eFlowRules | select `
    'IAFSourceMA',`
    'IAFCDObjectType',`
    'IAFCDAttribute',`
    'IAFRuleType',`
    'IAFScriptContext',`
    'IAFPrecedenceType',`
    'IAFPrecedenceRank',`
    'MVObjectType',`
    'MVAttribute',`
    'EAFMVAttribute',`
    'EAFTargetMA',`
    'EAFCDObjectType',`
    'EAFCDAttribute',`
    'EAFRuleType',`
    'EAFScriptContext',`
    'EAFAllowNulls',`
    'EAFInitialFlowOnly',`
    'EAFIsExistenceTest'
  }
}


<#
    .SYNOPSIS 
    Gets the MV Attributes and Object Types

    .DESCRIPTION
    Reads the MV configuration from the XML file, and outputs the MV Attributes and Object Types

    .OUTPUTS
    PSObjects containing the synchronization server MV Attributes and Object Types
   
    .EXAMPLE
    Get-MetaverseSchema .\ServerConfig\001\mv.xml | Out-GridView
   
#>
function Get-MetaverseSchema
{
  param
  (        
    [parameter(Mandatory=$false)]
    [String]                     
    [ValidateScript({(test-path -path $_ -PathType Leaf)})]
    $MVSchemaFile = 'mv.xml'
  ) 
  end
  {
    [xml]$mvXML = get-content $MVSchemaFile
             
    $namespace = @{dsml="http://www.dsml.org/DSML"; 'ms-dsml'="http://www.microsoft.com/MMS/DSML"}
             
    ###
    ### Attribute Types
    ###
    $mvAttributeTypes = select-xml $mvXML -XPath "//dsml:directory-schema/dsml:attribute-type" -Namespace $namespace | select -ExpandProperty Node 
             
    $attributes = @()
    foreach ($mvAttributeType in $mvAttributeTypes)
    {
      switch($mvAttributeType.syntax)
      {
        '1.3.6.1.4.1.1466.115.121.1.12' 
        {
          $syntax = 'Reference';
          break
        }
        '1.3.6.1.4.1.1466.115.121.1.15' 
        {
          $syntax = 'String';
          break
        }
        '1.3.6.1.4.1.1466.115.121.1.5' 
        {
          $syntax = 'Binary';
          break
        }
        '1.3.6.1.4.1.1466.115.121.1.27' 
        {
          $syntax = 'Number';
          break
        }
        '1.3.6.1.4.1.1466.115.121.1.7' 
        {
          $syntax = 'Boolean';
          break
        }
        default 
        {
          $syntax = "Unknown"; 
          break
        }
      }
             
      $attribute = New-Object PSObject
      $attribute | Add-Member -MemberType NoteProperty -Name 'AttributeName' -Value $mvAttributeType.name
      $attribute | Add-Member -MemberType NoteProperty -Name 'Syntax' -Value $syntax
      $attribute | Add-Member -MemberType NoteProperty -Name 'Indexable' -Value ($mvAttributeType.'indexable' -eq 'true')
      $attribute | Add-Member -MemberType NoteProperty -Name 'Indexed' -Value ($mvAttributeType.indexed -ne $null)
      $attribute | Add-Member -MemberType NoteProperty -Name 'MultiValued' -Value ($mvAttributeType.'single-value' -ne 'true' -or $mvAttributeType.'single-value' -eq $null)
      $attributes += $attribute
    }
             
    ###
    ### Bindings
    ###
    $mvObjectTypes = select-xml $mvXML -XPath "//dsml:directory-schema/dsml:class" -Namespace $namespace | select -ExpandProperty node
             
    ### Loop through the Source Class items, then output an attribute with the object type
    foreach ($mvObjectType in $mvObjectTypes)
    {
      Write-Verbose ("`nProcessing Bindings for {0}" -F $mvObjectType.name)                   
      $bindings = $mvObjectType | select -expandproperty attribute | select -ExpandProperty ref
      foreach($binding in $bindings)
      {
        #$attributes | Where-Object {$_.Name -eq $binding.Replace("#",'')} | foreach {$_.ObjectType = $mvObjectType.name; Write-Output $_}
        $attributes | 
        Where-Object {$_.AttributeName -eq $binding.Replace("#",'')} |
        Select-Object -Property @{Name="ObjectType";Expression={$mvObjectType.name}},* |
        Write-Output
      }
    }
  }
} 
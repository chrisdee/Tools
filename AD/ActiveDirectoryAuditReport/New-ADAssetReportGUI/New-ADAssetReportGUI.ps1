#region Source: Startup.pfs
#----------------------------------------------
#region Import Assemblies
#----------------------------------------------
[void][Reflection.Assembly]::Load("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
[void][Reflection.Assembly]::Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
[void][Reflection.Assembly]::Load("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
[void][Reflection.Assembly]::Load("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
[void][Reflection.Assembly]::Load("System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
[void][Reflection.Assembly]::Load("System.Xml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
[void][Reflection.Assembly]::Load("System.DirectoryServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
#endregion Import Assemblies

function Main {
    Param ([String]$Commandline)
    if((Call-ExampleOutputForm_pff) -eq "OK")
    {}
    $global:ExitCode = 0
}
#endregion Source: Startup.pfs

#region Source: Globals.ps1
	# Global Variables
	$OutputToGrid = $false
	$FunctionName = 'YourFunction'
	$FunctionPath = '.\New-ADAssetReport.ps1'
	$FunctionIsExternal = $true
	
	$FunctionParamTypes = @{}
	$FunctionParamTypes.Add("combobox_ReportFormat","combobox")
	$FunctionParamTypes.Add("combobox_ReportType","combobox")
	$FunctionParamTypes.Add("checkbox_ExportAllUsers","checkbox")
	$FunctionParamTypes.Add("checkbox_ExportPrivilegedUsers","checkbox")
	$FunctionParamTypes.Add("checkbox_ExportGraphvizDefinitionFiles","checkbox")
	$FunctionParamTypes.Add("checkbox_SaveData","checkbox")
	$FunctionParamTypes.Add("checkbox_LoadData","checkbox")
	$FunctionParamTypes.Add("textbox_DataFile","string")
	$FunctionParamTypes.Add("checkbox_Verbose","checkbox")
	
	$MandatoryParams = @{}
	$MandatoryParams.Add("ReportFormat",$False)
	$MandatoryParams.Add("ReportType",$False)
	$MandatoryParams.Add("ExportAllUsers",$False)
	$MandatoryParams.Add("ExportPrivilegedUsers",$False)
	$MandatoryParams.Add("ExportGraphvizDefinitionFiles",$False)
	$MandatoryParams.Add("SaveData",$False)
	$MandatoryParams.Add("LoadData",$False)
	$MandatoryParams.Add("DataFile",$False)
	$MandatoryParams.Add("Verbose",$False)
	
	# Global functions
	Function Convert-SplatToString ($Splat)
	{
	    BEGIN
	    {
	        Function Escape-PowershellString ([string]$InputString)
	        {
	            $replacements = @{
	                '!' = '`!' 
	                '"' = '`"'
	                '$' = '`$'
	                '%' = '`%'
	                '*' = '`*'
	                "'" = "`'"
	                ' ' = '` '
	                '#' = '`#'
	                '@' = '`@'
	                '.' = '`.'
	                '=' = '`='
	                ',' = '`,'
	            }
	
	            # Join all (escaped) keys from the hashtable into one regular expression.
	            [regex]$r = @($replacements.Keys | foreach { [regex]::Escape( $_ ) }) -join '|'
	
	            $matchEval = { param( [Text.RegularExpressions.Match]$matchInfo )
	              # Return replacement value for each matched value.
	              $matchedValue = $matchInfo.Groups[0].Value
	              $replacements[$matchedValue]
	            }
	
	            $InputString | Foreach { $r.Replace( $_, $matchEval ) }
	        }
	    }
	    PROCESS
	    {
	    }
	    END
	    {
	        $ResultSplat = ''
	        Foreach ($SplatName in $Splat.Keys)
	        {
	            switch ((($Splat[$SplatName]).GetType()).Name) {
	            	'Boolean' {
	            		if ($Splat[$SplatName] -eq $true)
	                    {
	                        $SplatVal = '$true'
	                    }
	                    else
	                    {
	                        $SplatVal = '$false'
	                    }
	            		break
	            	}
	            	'String' {
	            		$SplatVal = '"' + $(Escape-PowershellString $Splat[$SplatName]) + '"'
	                    break
	            	}
	            	default {
	                    $SplatVal = $Splat[$SplatName]
	            		break
	            	}
	            }
	            $ResultSplat = $ResultSplat + '-' + $SplatName + ':' + $SplatVal + ' '
	        }
	        $ResultSplat
	    }
	}
#endregion Source: Globals.ps1

#region Source: ExampleOutputForm.pff
function Call-ExampleOutputForm_pff
{
	#----------------------------------------------
	#region Import the Assemblies
	#----------------------------------------------
	[void][reflection.assembly]::Load("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	[void][reflection.assembly]::Load("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Xml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.DirectoryServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	#endregion Import Assemblies

	#----------------------------------------------
	#region Generated Form Objects
	#----------------------------------------------
	[System.Windows.Forms.Application]::EnableVisualStyles()
	$YourFunction_Form = New-Object 'System.Windows.Forms.Form'
	$formpanel = New-Object 'System.Windows.Forms.Panel'
	$label_ReportFormat = New-Object 'System.Windows.Forms.Label'
	$combobox_ReportFormat = New-Object 'System.Windows.Forms.ComboBox'
	$label_ReportType = New-Object 'System.Windows.Forms.Label'
	$combobox_ReportType = New-Object 'System.Windows.Forms.ComboBox'
	$checkbox_ExportAllUsers = New-Object 'System.Windows.Forms.CheckBox'
	$checkbox_ExportPrivilegedUsers = New-Object 'System.Windows.Forms.CheckBox'
	$checkbox_ExportGraphvizDefinitionFiles = New-Object 'System.Windows.Forms.CheckBox'
	$checkbox_SaveData = New-Object 'System.Windows.Forms.CheckBox'
	$checkbox_LoadData = New-Object 'System.Windows.Forms.CheckBox'
	$label_DataFile = New-Object 'System.Windows.Forms.Label'
	$textbox_DataFile = New-Object 'System.Windows.Forms.TextBox'
	$checkbox_Verbose = New-Object 'System.Windows.Forms.CheckBox'
	$textbox_CommentBasedHelp = New-Object 'System.Windows.Forms.TextBox'
	$buttonExecute = New-Object 'System.Windows.Forms.Button'
	$tooltip = New-Object 'System.Windows.Forms.ToolTip'
	$InitialFormWindowState = New-Object 'System.Windows.Forms.FormWindowState'
	#endregion Generated Form Objects

	#----------------------------------------------
	# User Generated Script
	#----------------------------------------------
	$OnLoadFormEvent={
	    $combobox_ReportFormat.Text = 'HTML'
	    $combobox_ReportType.Text = 'ForestAndDomain'
	}
	
	$buttonExecute_Click={
	    # Create our function splat
	    $ValidParamSplat = $true
	    $FunctionCallSplat = @{}
	    Foreach ($funcparam in $FunctionParamTypes.keys)
	    {
	        switch ($FunctionParamTypes[$funcparam]) {
	            'switch' {
	                $ParamName = $funcparam -replace 'switch_',''
	                $ControlString = '$' + $funcparam
	                $ControlName = [Scriptblock]::Create($ControlString)
	                $FunctionCallSplat.[string]$ParamName = $(Invoke-Command  $ControlName).Checked
	            }
	            'checkbox' {
	                $ParamName = $funcparam -replace 'checkbox_',''
	                $ControlString = '$' + $funcparam
	                $ControlName = [Scriptblock]::Create($ControlString)
	                $FunctionCallSplat.[string]$ParamName = $(Invoke-Command  $ControlName).Checked
	            }
	            'bool' {
	                $ParamName = $funcparam -replace 'bool_',''
	                $ControlString = '$' + $funcparam
	                $ControlName = [Scriptblock]::Create($ControlString)
	                $FunctionCallSplat.[string]$ParamName = $(Invoke-Command  $ControlName).Checked
	            }
	            'int' {
	                $ParamName = $funcparam -replace 'dial_',''
	                $ControlString = '$' + $funcparam
	                $ControlName = [Scriptblock]::Create($ControlString)
	                $FunctionCallSplat.[string]$ParamName = $(Invoke-Command $ControlName).Value
	            }
	            'string' {
	                $ParamName = $funcparam -replace 'textbox_',''
	                $ControlString = '$' + $funcparam
	                $ControlName = [Scriptblock]::Create($ControlString)
	                $FunctionCallSplat.[string]$ParamName = $(Invoke-Command  $ControlName).Text
	            }
	            'combobox' {
	                $ParamName = $funcparam -replace 'combobox_',''
	                $ControlString = '$' + $funcparam
	                $ControlName = [Scriptblock]::Create($ControlString)
	                $ComboValue = $(Invoke-Command  $ControlName).Text
	                if ($ComboValue -eq '')
	                {
	                    $ValidParamSplat = $false
	                    #[void][reflection.assembly]::Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	                    [void][System.Windows.Forms.MessageBox]::Show("Missing Mandatory Parameter: $ParamName","Error!")
	                }
	                else
	                {
	                    $FunctionCallSplat.[string]$ParamName = $ComboValue
	                }
	            }
	        }         
	    }
	
	    # Validate all mandatory parameters are present
	    if ($ValidParamSplat)
	    {
	        foreach ($MParam in $MandatoryParams.keys)
	        {
	            if ($MandatoryParams[$MParam])
	            {
	                If (! $FunctionCallSplat.ContainsKey($MParam))
	                {
	                    $ValidParamSplat = $false
	                    #[void][reflection.assembly]::Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	                    [void][System.Windows.Forms.MessageBox]::Show("Missing Mandatory Parameter: $MParam","Error!")
	                }
	            }
	        }
	    }
	    
	    if ($ValidParamSplat)
	    {
	        if ($FunctionIsExternal)
	        {
	            $FunctionCallSplatString = Convert-SplatToString $FunctionCallSplat
				#$FunctionCommand = $FunctionPath + " @FunctionCallSplat"
				$FunctionCommand = "Start-Process powershell.exe -ArgumentList '-NoExit -c " + $FunctionPath + " $FunctionCallSplatString" + "'"
	        }
	        else
	        {
	            $FunctionCommand = $FunctionName + " @FunctionCallSplat"
	        }
	        
	        if ($OutputToGrid)
	        {
	            $FunctionCommand = $FunctionCommand + ' | Out-GridViewForm'
	        }
	
	        $FunctionToCall = [Scriptblock]::Create($FunctionCommand)
	        Invoke-Command $FunctionToCall
	    }
	}
	#region Control Helper Functions
	function Load-ListBox 
	{
	<#
	    .SYNOPSIS
	        This functions helps you load items into a ListBox or CheckedListBox.
	
	    .DESCRIPTION
	        Use this function to dynamically load items into the ListBox control.
	
	    .PARAMETER  ListBox
	        The ListBox control you want to add items to.
	
	    .PARAMETER  Items
	        The object or objects you wish to load into the ListBox's Items collection.
	
	    .PARAMETER  DisplayMember
	        Indicates the property to display for the items in this control.
	    
	    .PARAMETER  Append
	        Adds the item(s) to the ListBox without clearing the Items collection.
	    
	    .EXAMPLE
	        Load-ListBox $ListBox1 "Red", "White", "Blue"
	    
	    .EXAMPLE
	        Load-ListBox $listBox1 "Red" -Append
	        Load-ListBox $listBox1 "White" -Append
	        Load-ListBox $listBox1 "Blue" -Append
	    
	    .EXAMPLE
	        Load-ListBox $listBox1 (Get-Process) "ProcessName"
	#>
	    Param (
	        [ValidateNotNull()]
	        [Parameter(Mandatory=$true)]
	        [System.Windows.Forms.ListBox]$ListBox,
	        [ValidateNotNull()]
	        [Parameter(Mandatory=$true)]
	        $Items,
	        [Parameter(Mandatory=$false)]
	        [string]$DisplayMember,
	        [switch]$Append
	    )
	    
	    if(-not $Append)
	    {
	        $listBox.Items.Clear()    
	    }
	    
	    if($Items -is [System.Windows.Forms.ListBox+ObjectCollection])
	    {
	        $listBox.Items.AddRange($Items)
	    }
	    elseif ($Items -is [Array])
	    {
	        $listBox.BeginUpdate()
	        foreach($obj in $Items)
	        {
	            $listBox.Items.Add($obj)
	        }
	        $listBox.EndUpdate()
	    }
	    else
	    {
	        $listBox.Items.Add($Items)    
	    }
	
	    $listBox.DisplayMember = $DisplayMember    
	}
	
	function Load-ComboBox 
	{
	<#
	    .SYNOPSIS
	        This functions helps you load items into a ComboBox.
	
	    .DESCRIPTION
	        Use this function to dynamically load items into the ComboBox control.
	
	    .PARAMETER  ComboBox
	        The ComboBox control you want to add items to.
	
	    .PARAMETER  Items
	        The object or objects you wish to load into the ComboBox's Items collection.
	
	    .PARAMETER  DisplayMember
	        Indicates the property to display for the items in this control.
	    
	    .PARAMETER  Append
	        Adds the item(s) to the ComboBox without clearing the Items collection.
	    
	    .EXAMPLE
	        Load-ComboBox $combobox1 "Red", "White", "Blue"
	    
	    .EXAMPLE
	        Load-ComboBox $combobox1 "Red" -Append
	        Load-ComboBox $combobox1 "White" -Append
	        Load-ComboBox $combobox1 "Blue" -Append
	    
	    .EXAMPLE
	        Load-ComboBox $combobox1 (Get-Process) "ProcessName"
	#>
	    Param (
	        [ValidateNotNull()]
	        [Parameter(Mandatory=$true)]
	        [System.Windows.Forms.ComboBox]$ComboBox,
	        [ValidateNotNull()]
	        [Parameter(Mandatory=$true)]
	        $Items,
	        [Parameter(Mandatory=$false)]
	        [string]$DisplayMember,
	        [switch]$Append
	    )
	    
	    if(-not $Append)
	    {
	        $ComboBox.Items.Clear()    
	    }
	    
	    if($Items -is [Object[]])
	    {
	        $ComboBox.Items.AddRange($Items)
	    }
	    elseif ($Items -is [Array])
	    {
	        $ComboBox.BeginUpdate()
	        foreach($obj in $Items)
	        {
	            $ComboBox.Items.Add($obj)    
	        }
	        $ComboBox.EndUpdate()
	    }
	    else
	    {
	        $ComboBox.Items.Add($Items)    
	    }
	
	    $ComboBox.DisplayMember = $DisplayMember    
	}
	#endregion
	$combobox_ReportType_TextChanged={
		switch ($combobox_ReportType.Text) {
	    	'Forest' {
	    		$checkbox_ExportAllUsers.Checked = $false
	            $checkbox_ExportAllUsers.Enabled = $false
	            $checkbox_ExportPrivilegedUsers.Checked = $false
	            $checkbox_ExportPrivilegedUsers.Enabled = $false
	            $checkbox_ExportGraphvizDefinitionFiles.Enabled = $true
	    	}
	    	'Domain' {
	    		$checkbox_ExportAllUsers.Enabled = $true
	            $checkbox_ExportPrivilegedUsers.Enabled = $true
	            $checkbox_ExportGraphvizDefinitionFiles.Enabled = $false
	            $checkbox_ExportGraphvizDefinitionFiles.Checked = $false
	    	}
	    	'ForestAndDomain' {
	    		$checkbox_ExportAllUsers.Enabled = $true
	            $checkbox_ExportPrivilegedUsers.Enabled = $true
	            $checkbox_ExportGraphvizDefinitionFiles.Enabled = $true
	    	}
	    	default {
	    		#<code>
	    	}
	    } 
	}
	
	$checkbox_SaveData_CheckedChanged={
		if ($checkbox_SaveData.Checked)
	    {
	        $checkbox_LoadData.Checked = $false
	        $checkbox_LoadData.Enabled = $false
		}
	    else
	    {
	        $checkbox_LoadData.Enabled = $true
		}
	}
	
	$checkbox_LoadData_CheckedChanged={
		if ($checkbox_LoadData.Checked)
	    {
	        $checkbox_SaveData.Checked = $false
	        $checkbox_SaveData.Enabled = $false
		}
	    else
	    {
	        $checkbox_SaveData.Enabled = $true
		}
	}
		# --End User Generated Script--
	#----------------------------------------------
	#region Generated Events
	#----------------------------------------------
	
	$Form_StateCorrection_Load=
	{
		#Correct the initial state of the form to prevent the .Net maximized form issue
		$YourFunction_Form.WindowState = $InitialFormWindowState
	}
	
	$Form_StoreValues_Closing=
	{
		#Store the control values
		$script:ExampleOutputForm_combobox_ReportFormat = $combobox_ReportFormat.Text
		$script:ExampleOutputForm_combobox_ReportType = $combobox_ReportType.Text
		$script:ExampleOutputForm_checkbox_ExportAllUsers = $checkbox_ExportAllUsers.Checked
		$script:ExampleOutputForm_checkbox_ExportPrivilegedUsers = $checkbox_ExportPrivilegedUsers.Checked
		$script:ExampleOutputForm_checkbox_ExportGraphvizDefinitionFiles = $checkbox_ExportGraphvizDefinitionFiles.Checked
		$script:ExampleOutputForm_checkbox_SaveData = $checkbox_SaveData.Checked
		$script:ExampleOutputForm_checkbox_LoadData = $checkbox_LoadData.Checked
		$script:ExampleOutputForm_textbox_DataFile = $textbox_DataFile.Text
		$script:ExampleOutputForm_checkbox_Verbose = $checkbox_Verbose.Checked
		$script:ExampleOutputForm_textbox_CommentBasedHelp = $textbox_CommentBasedHelp.Text
	}

	
	$Form_Cleanup_FormClosed=
	{
		#Remove all event handlers from the controls
		try
		{
			$combobox_ReportType.remove_TextChanged($combobox_ReportType_TextChanged)
			$checkbox_SaveData.remove_CheckedChanged($checkbox_SaveData_CheckedChanged)
			$checkbox_LoadData.remove_CheckedChanged($checkbox_LoadData_CheckedChanged)
			$buttonExecute.remove_Click($buttonExecute_Click)
			$YourFunction_Form.remove_Load($OnLoadFormEvent)
			$YourFunction_Form.remove_Load($Form_StateCorrection_Load)
			$YourFunction_Form.remove_Closing($Form_StoreValues_Closing)
			$YourFunction_Form.remove_FormClosed($Form_Cleanup_FormClosed)
		}
		catch [Exception]
		{ }
	}
	#endregion Generated Events

	#----------------------------------------------
	#region Generated Form Code
	#----------------------------------------------
	#
	# YourFunction_Form
	#
	$YourFunction_Form.Controls.Add($formpanel)
	$YourFunction_Form.Controls.Add($textbox_CommentBasedHelp)
	$YourFunction_Form.Controls.Add($buttonExecute)
	$YourFunction_Form.AcceptButton = $buttonExecute
	$YourFunction_Form.AutoScroll = $True
	$YourFunction_Form.AutoSizeMode = 'GrowAndShrink'
	$YourFunction_Form.ClientSize = '756, 362'
	$YourFunction_Form.FormBorderStyle = 'FixedDialog'
	$YourFunction_Form.MaximizeBox = $False
	$YourFunction_Form.MinimizeBox = $False
	$YourFunction_Form.Name = "YourFunction_Form"
	$YourFunction_Form.StartPosition = 'CenterScreen'
	$YourFunction_Form.Text = "AD Asset Report GUI"
	$YourFunction_Form.add_Load($OnLoadFormEvent)
	#
	# formpanel
	#
	$formpanel.Controls.Add($label_ReportFormat)
	$formpanel.Controls.Add($combobox_ReportFormat)
	$formpanel.Controls.Add($label_ReportType)
	$formpanel.Controls.Add($combobox_ReportType)
	$formpanel.Controls.Add($checkbox_ExportAllUsers)
	$formpanel.Controls.Add($checkbox_ExportPrivilegedUsers)
	$formpanel.Controls.Add($checkbox_ExportGraphvizDefinitionFiles)
	$formpanel.Controls.Add($checkbox_SaveData)
	$formpanel.Controls.Add($checkbox_LoadData)
	$formpanel.Controls.Add($label_DataFile)
	$formpanel.Controls.Add($textbox_DataFile)
	$formpanel.Controls.Add($checkbox_Verbose)
	$formpanel.Anchor = 'Top, Bottom, Left'
	$formpanel.AutoScroll = $True
	$formpanel.AutoSizeMode = 'GrowAndShrink'
	$formpanel.BorderStyle = 'Fixed3D'
	$formpanel.Location = '12, 12'
	$formpanel.Name = "formpanel"
	$formpanel.Size = '397, 319'
	$formpanel.TabIndex = 0
	#
	# label_ReportFormat
	#
	$label_ReportFormat.Anchor = 'Top, Left, Right'
	$label_ReportFormat.Font = "Microsoft Sans Serif, 7.8pt, style=Bold, Underline"
	$label_ReportFormat.Location = '3, 10'
	$label_ReportFormat.Name = "label_ReportFormat"
	$label_ReportFormat.Size = '385, 20'
	$label_ReportFormat.TabIndex = 100
	$label_ReportFormat.Text = "Report Format"
	$label_ReportFormat.TextAlign = 'MiddleLeft'
	#
	# combobox_ReportFormat
	#
	$combobox_ReportFormat.Anchor = 'Top, Left, Right'
	[void]$combobox_ReportFormat.AutoCompleteCustomSource.Add("HTML")
	[void]$combobox_ReportFormat.AutoCompleteCustomSource.Add("Excel")
	[void]$combobox_ReportFormat.AutoCompleteCustomSource.Add("Custom")
	$combobox_ReportFormat.DropDownStyle = 'DropDownList'
	$combobox_ReportFormat.FormattingEnabled = $True
	[void]$combobox_ReportFormat.Items.Add("HTML")
	[void]$combobox_ReportFormat.Items.Add("Excel")
	$combobox_ReportFormat.Location = '3, 35'
	$combobox_ReportFormat.Name = "combobox_ReportFormat"
	$combobox_ReportFormat.Size = '385, 21'
	$combobox_ReportFormat.TabIndex = 1
	$tooltip.SetToolTip($combobox_ReportFormat, "Format of report(s) to generate. Defaults to HTML.")
	#
	# label_ReportType
	#
	$label_ReportType.Anchor = 'Top, Left, Right'
	$label_ReportType.Font = "Microsoft Sans Serif, 7.8pt, style=Bold, Underline"
	$label_ReportType.Location = '3, 60'
	$label_ReportType.Name = "label_ReportType"
	$label_ReportType.Size = '385, 20'
	$label_ReportType.TabIndex = 102
	$label_ReportType.Text = "Report Type"
	$label_ReportType.TextAlign = 'MiddleLeft'
	#
	# combobox_ReportType
	#
	$combobox_ReportType.Anchor = 'Top, Left, Right'
	[void]$combobox_ReportType.AutoCompleteCustomSource.Add("Forest")
	[void]$combobox_ReportType.AutoCompleteCustomSource.Add("Domain")
	[void]$combobox_ReportType.AutoCompleteCustomSource.Add("ForestAndDomain")
	[void]$combobox_ReportType.AutoCompleteCustomSource.Add("Custom")
	$combobox_ReportType.DropDownStyle = 'DropDownList'
	$combobox_ReportType.FormattingEnabled = $True
	[void]$combobox_ReportType.Items.Add("Forest")
	[void]$combobox_ReportType.Items.Add("Domain")
	[void]$combobox_ReportType.Items.Add("ForestAndDomain")
	$combobox_ReportType.Location = '3, 85'
	$combobox_ReportType.Name = "combobox_ReportType"
	$combobox_ReportType.Size = '385, 21'
	$combobox_ReportType.TabIndex = 3
	$tooltip.SetToolTip($combobox_ReportType, "Types of report(s) to generate. Defaults to ForestAndDomain.")
	$combobox_ReportType.add_TextChanged($combobox_ReportType_TextChanged)
	#
	# checkbox_ExportAllUsers
	#
	$checkbox_ExportAllUsers.Anchor = 'Top, Left, Right'
	$checkbox_ExportAllUsers.Location = '3, 110'
	$checkbox_ExportAllUsers.Name = "checkbox_ExportAllUsers"
	$checkbox_ExportAllUsers.Size = '385, 20'
	$checkbox_ExportAllUsers.TabIndex = 4
	$checkbox_ExportAllUsers.Text = "Export All Users"
	$tooltip.SetToolTip($checkbox_ExportAllUsers, "CSV Export of all users.(Only applies to Domain account report)")
	$checkbox_ExportAllUsers.UseVisualStyleBackColor = $True
	#
	# checkbox_ExportPrivilegedUsers
	#
	$checkbox_ExportPrivilegedUsers.Anchor = 'Top, Left, Right'
	$checkbox_ExportPrivilegedUsers.Location = '3, 135'
	$checkbox_ExportPrivilegedUsers.Name = "checkbox_ExportPrivilegedUsers"
	$checkbox_ExportPrivilegedUsers.Size = '385, 20'
	$checkbox_ExportPrivilegedUsers.TabIndex = 5
	$checkbox_ExportPrivilegedUsers.Text = "Export Privileged Users"
	$tooltip.SetToolTip($checkbox_ExportPrivilegedUsers, "CSV Export of all priviledged users. (Only applies to Domain account report)")
	$checkbox_ExportPrivilegedUsers.UseVisualStyleBackColor = $True
	#
	# checkbox_ExportGraphvizDefinitionFiles
	#
	$checkbox_ExportGraphvizDefinitionFiles.Anchor = 'Top, Left, Right'
	$checkbox_ExportGraphvizDefinitionFiles.Location = '3, 160'
	$checkbox_ExportGraphvizDefinitionFiles.Name = "checkbox_ExportGraphvizDefinitionFiles"
	$checkbox_ExportGraphvizDefinitionFiles.Size = '385, 20'
	$checkbox_ExportGraphvizDefinitionFiles.TabIndex = 6
	$checkbox_ExportGraphvizDefinitionFiles.Text = "Export Graphviz Definition Files"
	$tooltip.SetToolTip($checkbox_ExportGraphvizDefinitionFiles, "Export graphviz definition files for diagram generation.(Only applies to Forest report)")
	$checkbox_ExportGraphvizDefinitionFiles.UseVisualStyleBackColor = $True
	#
	# checkbox_SaveData
	#
	$checkbox_SaveData.Anchor = 'Top, Left, Right'
	$checkbox_SaveData.Location = '3, 185'
	$checkbox_SaveData.Name = "checkbox_SaveData"
	$checkbox_SaveData.Size = '385, 20'
	$checkbox_SaveData.TabIndex = 7
	$checkbox_SaveData.Text = "Save Data"
	$tooltip.SetToolTip($checkbox_SaveData, "Save all gathered data.")
	$checkbox_SaveData.UseVisualStyleBackColor = $True
	$checkbox_SaveData.add_CheckedChanged($checkbox_SaveData_CheckedChanged)
	#
	# checkbox_LoadData
	#
	$checkbox_LoadData.Anchor = 'Top, Left, Right'
	$checkbox_LoadData.Location = '3, 210'
	$checkbox_LoadData.Name = "checkbox_LoadData"
	$checkbox_LoadData.Size = '385, 20'
	$checkbox_LoadData.TabIndex = 8
	$checkbox_LoadData.Text = "Load Data"
	$tooltip.SetToolTip($checkbox_LoadData, "Load previously saved data.")
	$checkbox_LoadData.UseVisualStyleBackColor = $True
	$checkbox_LoadData.add_CheckedChanged($checkbox_LoadData_CheckedChanged)
	#
	# label_DataFile
	#
	$label_DataFile.Anchor = 'Top, Left, Right'
	$label_DataFile.Font = "Microsoft Sans Serif, 7.8pt, style=Bold, Underline"
	$label_DataFile.Location = '3, 235'
	$label_DataFile.Name = "label_DataFile"
	$label_DataFile.Size = '385, 20'
	$label_DataFile.TabIndex = 109
	$label_DataFile.Text = "DataFile"
	$label_DataFile.TextAlign = 'MiddleLeft'
	#
	# textbox_DataFile
	#
	$textbox_DataFile.Anchor = 'Top, Left, Right'
	$textbox_DataFile.Location = '3, 260'
	$textbox_DataFile.Name = "textbox_DataFile"
	$textbox_DataFile.Size = '385, 20'
	$textbox_DataFile.TabIndex = 10
	$textbox_DataFile.Text = "SaveData.xml"
	$tooltip.SetToolTip($textbox_DataFile, "Data file used when saving or loading data.")
	#
	# checkbox_Verbose
	#
	$checkbox_Verbose.Anchor = 'Top, Left, Right'
	$checkbox_Verbose.Checked = $True
	$checkbox_Verbose.CheckState = 'Checked'
	$checkbox_Verbose.Location = '3, 286'
	$checkbox_Verbose.Name = "checkbox_Verbose"
	$checkbox_Verbose.Size = '385, 20'
	$checkbox_Verbose.TabIndex = 12
	$checkbox_Verbose.Text = "Verbose"
	$tooltip.SetToolTip($checkbox_Verbose, "Return verbose output.")
	$checkbox_Verbose.UseVisualStyleBackColor = $True
	#
	# textbox_CommentBasedHelp
	#
	$textbox_CommentBasedHelp.Anchor = 'Top, Bottom, Right'
	$textbox_CommentBasedHelp.Location = '415, 12'
	$textbox_CommentBasedHelp.Multiline = $True
	$textbox_CommentBasedHelp.Name = "textbox_CommentBasedHelp"
	$textbox_CommentBasedHelp.ScrollBars = 'Both'
	$textbox_CommentBasedHelp.Size = '329, 319'
	$textbox_CommentBasedHelp.TabIndex = 0
	$textbox_CommentBasedHelp.Text = "Creates HTML reports of an active directory forest and its domains.

Author: Zachary Loeber

THIS CODE IS MADE AVAILABLE AS IS, WITHOUT WARRANTY OF ANY KIND. THE ENTIRE 
RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS CODE REMAINS WITH THE USER.

Version 1.7 - 02/13/2014

http://www.the-little-things.net "
	#
	# buttonExecute
	#
	$buttonExecute.Anchor = 'Bottom, Right'
	$buttonExecute.Location = '669, 336'
	$buttonExecute.Name = "buttonExecute"
	$buttonExecute.Size = '75, 23'
	$buttonExecute.TabIndex = 13
	$buttonExecute.Text = "Execute"
	$buttonExecute.UseVisualStyleBackColor = $True
	$buttonExecute.add_Click($buttonExecute_Click)
	#
	# tooltip
	#
	#endregion Generated Form Code

	#----------------------------------------------

	#Save the initial state of the form
	$InitialFormWindowState = $YourFunction_Form.WindowState
	#Init the OnLoad event to correct the initial state of the form
	$YourFunction_Form.add_Load($Form_StateCorrection_Load)
	#Clean up the control events
	$YourFunction_Form.add_FormClosed($Form_Cleanup_FormClosed)
	#Store the control values when form is closing
	$YourFunction_Form.add_Closing($Form_StoreValues_Closing)
	#Show the Form
	return $YourFunction_Form.ShowDialog()

}
#endregion Source: ExampleOutputForm.pff

#Start the application
Main ($CommandLine)
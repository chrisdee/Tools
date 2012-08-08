#========================================================================
# Date: 10/2/2011 8:34 PM
# Author: Rich Prescott
# Blog: blog.richprescott.com
# Twitter: twitter.com/Rich_Prescott
# Link: http://blog.richprescott.com/2011/10/arposh-new-user-creation-anuc.html
#========================================================================
#----------------------------------------------
#region Application Functions
#----------------------------------------------

function OnApplicationLoad {
$CreateXML = @"
<?xml version="1.0" standalone="no"?>
<OPTIONS Product="Arposh New User Creation" Version="1.1">
 <Settings>
  <sAMAccountName Generate="True">
   <Style Format="FirstName.LastName" Enabled="True" />
   <Style Format="FirstInitialLastName" Enabled="False" />
   <Style Format="LastNameFirstInitial" Enabled="False" />
  </sAMAccountName>
  <UPN Generate="True">
   <Style Format="FirstName.LastName" Enabled="True" />
   <Style Format="FirstInitialLastName" Enabled="False" />
   <Style Format="LastNameFirstInitial" Enabled="False" />
  </UPN>
  <DisplayName Generate="True">
   <Style Format="FirstName LastName" Enabled="False" />
   <Style Format="LastName, FirstName" Enabled="True" />
  </DisplayName>
  <AccountStatus Enabled="True" />
  <Password ChangeAtLogon="True" />
 </Settings>
 <Default>
  <Domain>RU.lab</Domain>
  <Path>OU=MyOU,DC=ru,DC=lab</Path>
  <FirstName></FirstName>
  <LastName></LastName>
  <Office></Office>
  <Title></Title>
  <Description>Full-Time Employee</Description>
  <Department>IT</Department>
  <Company>Arposh</Company>
  <Phone>212-555-1000</Phone>
  <Site>NY</Site>
  <StreetAddress>100 Main Street</StreetAddress>
  <City>New York</City>
  <State>NY</State>
  <PostalCode>10001</PostalCode>
  <Password>P@ssw0rd</Password>
 </Default>
 <Locations>
  <Location Site="NY">
   <StreetAddress>1 Main Street</StreetAddress>
   <City>New York</City>
   <State>NY</State>
   <PostalCode>10001</PostalCode>
  </Location>
  <Location Site="NJ">
   <StreetAddress>2 Main Street</StreetAddress>
   <City>Edison</City>
   <State>NJ</State>
   <PostalCode>22222</PostalCode>
  </Location>
  <Location Site="Custom">
   <StreetAddress></StreetAddress>
   <City></City>
   <State></State>
   <PostalCode></PostalCode>
  </Location>
 </Locations>
 <Domains>
  <Domain Name="RU.lab">
   <Path>OU=MyOU,DC=ru,DC=lab</Path>
   <Path>CN=Users,DC=ru,DC=lab</Path>
  </Domain>
  <Domain Name="RP.lab">
   <Path>OU=RPUsers1,DC=rp,DC=lab</Path>
   <Path>OU=RPUsers2,DC=rp,DC=lab</Path>
   <Path>OU=RPUsers3,DC=rp,DC=lab</Path>
  </Domain>
 </Domains>
 <Descriptions>
 <Description>Full-Time Employee</Description>
  <Description>Part-Time Employee</Description>
  <Description>Consultant</Description>
  <Description>Intern</Description>
  <Description>Service Account</Description>
  <Description>Temp</Description>
  <Description>Freelancer</Description>
 </Descriptions>
 <Departments>
  <Department>Finance</Department>
  <Department>IT</Department>
  <Department>Marketing</Department>
  <Department>Sales</Department>
  <Department>Executive</Department>
  <Department>Human Resources</Department>
  <Department>Security</Department>
 </Departments>
</OPTIONS>
"@
	
	Import-Module ActiveDirectory
	[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms") | Out-Null
	$XMLFile = "ANUC.Options.xml"
	$Script:ParentFolder = Split-Path (Get-Variable MyInvocation -scope 1 -ValueOnly).MyCommand.Definition
	$XMLFile = Join-Path $ParentFolder $XMLFile
	
	$XMLMsg = "Could not detect XML Options file ($XMLFile).  Do you want to create a template?"
	if(!(Test-Path $XMLFile)){
 	   if([System.Windows.Forms.MessageBox]::Show($XMLMsg,"Warning",[System.Windows.Forms.MessageBoxButtons]::YesNo) -eq "Yes"){
    	    $CreateXML | Out-File $XMLFile
			[XML]$Script:XML = Get-Content $XMLFile
	 	}
	}
	else{[XML]$Script:XML = Get-Content $XMLFile}
	return $true #return true for success or false for failure
}

function OnApplicationExit {
	Remove-Module ActiveDirectory	
	$script:ExitCode = 0 #Set the exit code for the Packager
}

Function Set-sAMAccountName {
    $GivenName = $txtFirstName.text
    $SurName = $txtLastName.text
    Switch($XML.Options.Settings.sAMAccountName.Style | Where{$_.Enabled -eq $True} | Select -ExpandProperty Format)
        {
        "FirstName.LastName"    {"{0}.{1}" -f $GivenName,$Surname}
        "FirstInitialLastName"  {"{0}{1}" -f ($GivenName)[0],$SurName}
        "LastNameFirstInitial"  {"{0}{1}" -f $SurName,($GivenName)[0]}
        Default                 {"{0}.{1}" -f $GivenName,$Surname}
        }
    }

Function Set-UPN {
    $GivenName = $txtFirstName.text
    $SurName = $txtLastName.text
    $Domain = $cboDomain.Text
    Switch($XML.Options.Settings.UPN.Style | Where{$_.Enabled -eq $True} | Select -ExpandProperty Format)
        {
        "FirstName.LastName"    {"{0}.{1}@{2}" -f $GivenName,$Surname,$Domain}
        "FirstInitialLastName"  {"{0}{1}@{2}" -f ($GivenName)[0],$SurName,$Domain}
        "LastNameFirstInitial"  {"{0}{1}@{2}" -f $SurName,($GivenName)[0],$Domain}
        Default                 {"{0}.{1}@{2}" -f $GivenName,$Surname,$Domain}
        }
    }

Function Set-DisplayName {
    $GivenName = $txtFirstName.text
    $SurName = $txtLastName.text
    Switch($XML.Options.Settings.DisplayName.Style | Where{$_.Enabled -eq $True} | Select -ExpandProperty Format)
        {
        "FirstName LastName"    {"{0} {1}" -f $GivenName,$Surname}
        "LastName, FirstName"   {"{0}, {1}" -f $SurName, $GivenName}
        Default                 {"{0} {1}" -f $GivenName,$Surname}
        }
    }
#endregion Application Functions

#----------------------------------------------
# Generated Form Function
#----------------------------------------------
function Call-ANUC_pff {

	#----------------------------------------------
	#region Import the Assemblies
	#----------------------------------------------
	[void][reflection.assembly]::Load("System.DirectoryServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	[void][reflection.assembly]::Load("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Xml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	[void][reflection.assembly]::Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	[void][reflection.assembly]::Load("System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	#endregion Import Assemblies

	#----------------------------------------------
	#region Generated Form Objects
	#----------------------------------------------
	[System.Windows.Forms.Application]::EnableVisualStyles()
	$formMain = New-Object System.Windows.Forms.Form
	$btnSubmitAll = New-Object System.Windows.Forms.Button
	$btnLast = New-Object System.Windows.Forms.Button
	$btnNext = New-Object System.Windows.Forms.Button
	$btnPrev = New-Object System.Windows.Forms.Button
	$btnFirst = New-Object System.Windows.Forms.Button
	$btnImportCSV = New-Object System.Windows.Forms.Button
	$lvCSV = New-Object System.Windows.Forms.ListView
	$txtUPN = New-Object System.Windows.Forms.TextBox
	$txtsAM = New-Object System.Windows.Forms.TextBox
	$txtDN = New-Object System.Windows.Forms.TextBox
	$cboDepartment = New-Object System.Windows.Forms.ComboBox
	$labelUserPrincipalName = New-Object System.Windows.Forms.Label
	$labelSamAccountName = New-Object System.Windows.Forms.Label
	$labelDisplayName = New-Object System.Windows.Forms.Label
	$SB = New-Object System.Windows.Forms.StatusBar
	$cboSite = New-Object System.Windows.Forms.ComboBox
	$labelSite = New-Object System.Windows.Forms.Label
	$cboDescription = New-Object System.Windows.Forms.ComboBox
	$txtPassword = New-Object System.Windows.Forms.TextBox
	$labelPassword = New-Object System.Windows.Forms.Label
	$cboDomain = New-Object System.Windows.Forms.ComboBox
	$labelCurrentDomain = New-Object System.Windows.Forms.Label
	$txtPostalCode = New-Object System.Windows.Forms.TextBox
	$txtState = New-Object System.Windows.Forms.TextBox
	$txtCity = New-Object System.Windows.Forms.TextBox
	$txtStreetAddress = New-Object System.Windows.Forms.TextBox
	$txtOffice = New-Object System.Windows.Forms.TextBox
	$txtCompany = New-Object System.Windows.Forms.TextBox
	$txtTitle = New-Object System.Windows.Forms.TextBox
	$txtOfficePhone = New-Object System.Windows.Forms.TextBox
	$txtLastName = New-Object System.Windows.Forms.TextBox
	$cboPath = New-Object System.Windows.Forms.ComboBox
	$labelOU = New-Object System.Windows.Forms.Label
	$txtFirstName = New-Object System.Windows.Forms.TextBox
	$labelPostalCode = New-Object System.Windows.Forms.Label
	$labelState = New-Object System.Windows.Forms.Label
	$labelCity = New-Object System.Windows.Forms.Label
	$labelStreetAddress = New-Object System.Windows.Forms.Label
	$labelOffice = New-Object System.Windows.Forms.Label
	$labelCompany = New-Object System.Windows.Forms.Label
	$labelDepartment = New-Object System.Windows.Forms.Label
	$labelTitle = New-Object System.Windows.Forms.Label
	$btnSubmit = New-Object System.Windows.Forms.Button
	$labelDescription = New-Object System.Windows.Forms.Label
	$labelOfficePhone = New-Object System.Windows.Forms.Label
	$labelLastName = New-Object System.Windows.Forms.Label
	$labelFirstName = New-Object System.Windows.Forms.Label
	$menustrip1 = New-Object System.Windows.Forms.MenuStrip
	$fileToolStripMenuItem = New-Object System.Windows.Forms.ToolStripMenuItem
	$formMode = New-Object System.Windows.Forms.ToolStripMenuItem
	$CSVTemplate = New-Object System.Windows.Forms.SaveFileDialog
	$OFDImportCSV = New-Object System.Windows.Forms.OpenFileDialog
	$CreateCSVTemplate = New-Object System.Windows.Forms.ToolStripMenuItem
	$MenuExit = New-Object System.Windows.Forms.ToolStripMenuItem
	$InitialFormWindowState = New-Object System.Windows.Forms.FormWindowState
	#endregion Generated Form Objects

	#----------------------------------------------
	# User Generated Script
	#----------------------------------------------
	
	
	
	$formMain_Load={
		
		$formMain.Text = $formMain.Text + " " + $XML.Options.Version
		
		Write-Verbose "Adding domains to combo box"
		$XML.Options.Domains.Domain | %{$cboDomain.Items.Add($_.Name)}
		
		Write-Verbose "Adding OUs to combo box"
	    $XML.Options.Domains.Domain | ?{$_.Name -match $cboDomain.Text} | Select -ExpandProperty Path | %{$cboPath.Items.Add($_)}
		
		Write-Verbose "Adding descriptions to combo box"
		$XML.Options.Descriptions.Description | %{$cboDescription.Items.Add($_)}
		
		Write-Verbose "Adding sites to combo box"
		$XML.Options.Locations.Location | %{$cboSite.Items.Add($_.Site)}
		
		Write-Verbose "Adding departments to combo box"
		$XML.Options.Departments.Department | %{$cboDepartment.Items.Add($_)}
		
		Write-Verbose "Setting default fields"
		$cboDomain.SelectedItem = $XML.Options.Default.Domain
	    $cboPath.SelectedItem = $XML.Options.Default.Path
		$txtFirstName.Text = $XML.Options.Default.FirstName
		$txtLastName.Text = $XML.Options.Default.LastName
		$txtOffice.Text = $XML.Options.Default.Office
		$txtTitle.Text = $XML.Options.Default.Title
		$cboDescription.SelectedItem = $XML.Options.Default.Description
		$cboDepartment.SelectedItem = $XML.Options.Default.Department
		$txtCompany.Text = $XML.Options.Default.Company
		$txtOfficePhone.Text = $XML.Options.Default.Phone
		$cboSite.SelectedItem = $XML.Options.Default.Site
		$txtStreetAddress.Text = $XML.Options.Default.StreetAddress
		$txtCity.Text = $XML.Options.Default.City
		$txtState.Text = $XML.Options.Default.State
		$txtPostalCode.Text = $XML.Options.Default.PostalCode
		$txtPassword.Text = $XML.Options.Default.Password
		
		Write-Verbose "Creating CSV Headers"
		$Headers = @('ID','Domain','Path','FirstName','LastName','Office','Title','Description','Department','Company','Phone','StreetAddress','City','State','PostalCode','Password','sAMAccountName','userPrincipalName','DisplayName')
		$Headers| %{[Void]$lvCSV.Columns.Add($_)}
	}
	
	$btnSubmit_Click={
		
		$Domain=$cboDomain.Text
		$Path=$cboPath.Text
		$GivenName = $txtFirstName.Text
		$Surname = $txtLastName.Text
		$OfficePhone = $txtOfficePhone.Text
		$Description = $cboDescription.Text
		$Title = $txtTitle.Text
		$Department = $cboDepartment.Text
		$Company = $txtCompany.Text
		$Office = $txtOffice.Text
		$StreetAddress = $txtStreetAddress.Text
		$City = $txtCity.Text
		$State = $txtState.Text
		$PostalCode = $txtPostalCode.Text
	
		if($XML.Options.Settings.Password.ChangeAtLogon -eq "True"){$ChangePasswordAtLogon = $True}
        else{$ChangePasswordAtLogon = $false}
		
        if($XML.Options.Settings.AccountStatus.Enabled -eq "True"){$Enabled = $True}
        else{$Enabled = $false}
	
		$Name="$GivenName $Surname"
		
        if($XML.Options.Settings.sAMAccountName.Generate -eq $True){$sAMAccountName = Set-sAMAccountName}
		else{$sAMAccountName = $txtsAM.Text}

        if($XML.Options.Settings.uPN.Generate -eq $True){$userPrincipalName = Set-UPN}
        else{$userPrincipalName = $txtuPN.Text}
		
        if($XML.Options.Settings.DisplayName.Generate -eq $True){$DisplayName = Set-DisplayName}
        else{$DisplayName = $txtDN.Text}

		$AccountPassword = $txtPassword.text | ConvertTo-SecureString -AsPlainText -Force
	
		$User = @{
		    Name = $Name
		    GivenName = $GivenName
		    Surname = $Surname
		    Path = $Path
		    samAccountName = $samAccountName
		    userPrincipalName = $userPrincipalName
		    DisplayName = $DisplayName
		    AccountPassword = $AccountPassword
		    ChangePasswordAtLogon = $ChangePasswordAtLogon
		    Enabled = $Enabled
		    OfficePhone = $OfficePhone
		    Description = $Description
		    Title = $Title
		    Department = $Department
		    Company = $Company
		    Office = $Office
		    StreetAddress = $StreetAddress
		    City = $City
		    State = $State
		    PostalCode = $PostalCode
		    }
		$SB.Text = "Creating new user $sAMAccountName"
        $ADError = $Null
		New-ADUser @User -ErrorVariable ADError
        if ($ADerror){$SB.Text = "[$sAMAccountName] $ADError"}
        else{$SB.Text = "$sAMAccountName created successfully."}
	}
	
	$cboDomain_SelectedIndexChanged={
		$cboPath.Items.Clear()
		Write-Verbose "Adding OUs to combo box"
	    $XML.Options.Domains.Domain | ?{$_.Name -match $cboDomain.Text} | Select -ExpandProperty Path | %{$cboPath.Items.Add($_)}	
		Write-Verbose "Creating required account fields"
		
        if ($XML.Options.Settings.DisplayName.Generate) {$txtDN.Text = Set-DisplayName}
        if ($XML.Options.Settings.sAMAccountName.Generate) {$txtsAM.Text = Set-sAMAccountName}
        if ($XML.Options.Settings.UPN.Generate) {$txtUPN.Text = Set-UPN}	
	}
	
	$cboSite_SelectedIndexChanged={
		Write-Verbose "Updating site fields with address information"
	    $Site = $XML.Options.Locations.Location | ?{$_.Site -match $cboSite.Text}
		$txtStreetAddress.Text = $Site.StreetAddress
		$txtCity.Text = $Site.City
		$txtState.Text = $Site.State
		$txtPostalCode.Text = $Site.PostalCode
	}
	
	$txtName_TextChanged={
		Write-Verbose "Creating required account fields"
        
        if ($XML.Options.Settings.DisplayName.Generate -eq $True) {$txtDN.Text = Set-DisplayName}
        if ($XML.Options.Settings.sAMAccountName.Generate -eq $True) {$txtsAM.Text = (Set-sAMAccountName)}
        if ($XML.Options.Settings.UPN.Generate -eq $True) {$txtUPN.Text = Set-UPN}
	}
	
	$createTemplateToolStripMenuItem_Click={
		$CSVTemplate.ShowDialog()
	}
	
	$CSVTemplate_FileOk=[System.ComponentModel.CancelEventHandler]{
		"" |
		Select Domain,Path,FirstName,LastName,Office,Title,Description,Department,Company,Phone,StreetAddress,City,State,PostalCode,Password,sAMAccountName,userPrincipalName,DisplayName |
		Export-CSV $CSVTemplate.FileName -NoTypeInformation	
	}
	
	$formMode_Click={
		if($formMode.Text -eq 'CSV Mode'){
			$formMode.Text = "Single-User Mode"
			Get-Variable | ?{$_.Name -match "txt"} | %{Try{$_.Value.Anchor = 'Top,Left'}catch{}}
			Get-Variable | ?{$_.Name -match "cbo"} | %{Try{$_.Value.Anchor = 'Top,Left'}catch{}}
			Get-Variable | ?{$_.Name -match "btn"} | %{Try{$_.Value.Anchor = 'Top,Left'}catch{}}
			$formMain.Size = '1484,635'
			$btnFirst.Visible = $True
			$btnPrev.Visible = $True
			$btnNext.Visible = $True
			$btnLast.Visible = $True
			$btnImportCSV.Visible = $True
			$btnSubmitAll.Visible = $True
			$lvCSV.Visible = $True
			$cboDomain.Width = '175'
		    $cboPath.Width = '249'
			$txtFirstName.Width = '175'
			$txtLastName.Width = '175'
			$txtOffice.Width = '175'
			$txtTitle.Width = '175'
			$cboDescription.Width = '175'
			$cboDepartment.Width = '175'
			$txtCompany.Width = '175'
			$txtOfficePhone.Width = '175'
			$cboSite.Width = '175'
			$txtStreetAddress.Width = '175'
			$txtCity.Width = '175'
			$txtState.Width = '175'
			$txtPostalCode.Width = '175'
			$txtPassword.Width = '175'
			$txtDN.Width = '175'
			$txtsAM.Width = '175'
			$txtUPN.Width = '175'
			}
		else{
			$formMode.Text = "CSV Mode"
			$formMain.Size = '320,635'
			Get-Variable | ?{$_.Name -match "txt"} | %{Try{$_.Value.Anchor = 'Top,Left,Right'}catch{}}
			Get-Variable | ?{$_.Name -match "cbo"} | %{Try{$_.Value.Anchor = 'Top,Left,Right'}catch{}}
			Get-Variable | ?{$_.Name -match "btn"} | %{Try{$_.Value.Anchor = 'Top,Left,Right'}catch{}}
			$btnFirst.Visible = $False
			$btnPrev.Visible = $False
			$btnNext.Visible = $False
			$btnLast.Visible = $False
			$btnImportCSV.Visible = $False
			$btnSubmitAll.Visible = $False
			$lvCSV.Visible = $False
			}
	}
	
	$btnImportCSV_Click={
		$OFDImportCSV.ShowDialog()
		$CSV = Import-Csv $OFDImportCSV.FileName
		$i = 0
		ForEach ($Entry in $CSV){
			$User = New-Object System.Windows.Forms.ListViewItem($i)
			ForEach ($Col in ($lvCSV.Columns | ?{$_.Text -ne "ID"})){
                $Field = $Col.Text
                $SubItem = "$($Entry.$Field)"
                if($Field -eq 'FirstName'){$GivenName = $SubItem}
                if($Field -eq 'LastName'){$Surname = $SubItem}
                if($Field -eq 'Domain'){$Domain = $SubItem}
                if($Field -eq 'sAMAccountName' -AND $SubItem -eq ""){$SubItem = Set-sAMAccountName}
                if($Field -eq 'userPrincipalName' -AND $SubItem -eq ""){$SubItem = Set-UPN}
                if($Field -eq 'DisplayName' -AND $SubItem -eq ""){$SubItem = Set-DisplayName}
                $User.SubItems.Add($SubItem)
                }
			$lvCSV.Items.Add($User)
			$i++
		}
	}
	
	$lvCSV_SelectedIndexChanged={
		try{$cboDomain.SelectedItem = $lvCSV.SelectedItems[0].SubItems[1].Text}catch{}
		try{$cboPath.SelectedItem = $lvCSV.SelectedItems[0].SubItems[2].Text}catch{}
		try{$txtFirstName.Text = $lvCSV.SelectedItems[0].SubItems[3].Text}catch{}
		try{$txtLastName.Text = $lvCSV.SelectedItems[0].SubItems[4].Text}catch{}
		try{$txtOffice.Text = $lvCSV.SelectedItems[0].SubItems[5].Text}catch{}
		try{$txtTitle.Text = $lvCSV.SelectedItems[0].SubItems[6].Text}catch{}
		try{$cboDescription.SelectedItem = $lvCSV.SelectedItems[0].SubItems[7].Text}catch{}
		try{$cboDepartment.SelectedItem = $lvCSV.SelectedItems[0].SubItems[8].Text}catch{}
		try{$txtCompany.Text = $lvCSV.SelectedItems[0].SubItems[9].Text}catch{}
		try{$txtOfficePhone.Text = $lvCSV.SelectedItems[0].SubItems[10].Text}catch{}
		try{$txtStreetAddress.Text = $lvCSV.SelectedItems[0].SubItems[11].Text}catch{}
		try{$txtCity.Text = $lvCSV.SelectedItems[0].SubItems[12].Text}catch{}
		try{$txtState.Text = $lvCSV.SelectedItems[0].SubItems[13].Text}catch{}
		try{$txtPostalCode.Text = $lvCSV.SelectedItems[0].SubItems[14].Text}catch{}
		try{$txtPassword.Text = $lvCSV.SelectedItems[0].SubItems[15].Text}catch{}
        try{$txtsAM.Text = $lvCSV.SelectedItems[0].SubItems[16].Text}catch{}
        try{$txtuPN.Text = $lvCSV.SelectedItems[0].SubItems[17].Text}catch{}
        try{$txtDN.Text = $lvCSV.SelectedItems[0].SubItems[18].Text}catch{}
	}
	
	$btnFirst_Click={
		$lvCSV.Items | %{$_.Selected = $False}
		$lvCSV.Items[0].Selected = $True	
	}
	
	$btnLast_Click={
		$LastRow = ($lvCSV.Items).Count - 1
		$lvCSV.Items | %{$_.Selected = $False}
		$lvCSV.Items[$LastRow].Selected = $True	
	}
	
	$btnNext_Click={
		$LastRow = ($lvCSV.Items).Count - 1
		[Int]$Index = $lvCSV.SelectedItems[0].Index
		if($LastRow -gt $Index){
			$lvCSV.Items | %{$_.Selected = $False}
			$lvCSV.Items[$Index+1].Selected = $True	
		}
	}
	
	$btnPrev_Click={
		[Int]$Index = $lvCSV.SelectedItems[0].Index
		if($Index -gt 0){
			$lvCSV.Items | %{$_.Selected = $False}
			$lvCSV.Items[$Index-1].Selected = $True	
		}
	}
	
	$MenuExit_Click={
		$formMain.Close()
	}
	
	$btnSubmitAll_Click={
		$lvCSV.Items | %{
			
			$Domain = $_.Subitems[1].Text
			$Path = $_.Subitems[2].Text
			$GivenName = $_.Subitems[3].Text
			$Surname = $_.Subitems[4].Text
			$OfficePhone = $_.Subitems[5].Text
			$Description = $_.Subitems[6].Text
			$Title = $_.Subitems[7].Text
			$Department = $_.Subitems[8].Text
			$Company = $_.Subitems[9].Text
			$Office = $_.Subitems[10].Text
			$StreetAddress = $_.Subitems[11].Text
			$City = $_.Subitems[12].Text
			$State = $_.Subitems[13].Text
			$PostalCode = $_.Subitems[14].Text
	
			$Name = "$GivenName $Surname"

		    if($XML.Options.Settings.Password.ChangeAtLogon -eq "True"){$ChangePasswordAtLogon = $True}
            else{$ChangePasswordAtLogon = $false}
		
            if($XML.Options.Settings.AccountStatus.Enabled -eq "True"){$Enabled = $True}
            else{$Enabled = $false}
	
		    if($_.Subitems[16].Text -eq $null){$sAMAccountName = Set-sAMAccountName}
		    else{$sAMAccountName = $_.Subitems[16].Text}

            if($_.Subitems[17].Text -eq $null){$userPrincipalName = Set-UPN}
            else{$userPrincipalName = $_.Subitems[17].Text}
		
            if($_.Subitems[18].Text -eq $null){$DisplayName = Set-DisplayName}
            else{$DisplayName = $_.Subitems[18].Text}

			$AccountPassword = $_.Subitems[15].Text | ConvertTo-SecureString -AsPlainText -Force
	
			$User = @{
			    Name = $Name
			    GivenName = $GivenName
			    Surname = $Surname
			    Path = $Path
			    samAccountName = $samAccountName
			    userPrincipalName = $userPrincipalName
			    DisplayName = $DisplayName
			    AccountPassword = $AccountPassword
			    ChangePasswordAtLogon = $ChangePasswordAtLogon
			    Enabled = $Enabled
			    OfficePhone = $OfficePhone
			    Description = $Description
			    Title = $Title
			    Department = $Department
			    Company = $Company
			    Office = $Office
			    StreetAddress = $StreetAddress
			    City = $City
			    State = $State
			    PostalCode = $PostalCode
			    }
			$SB.Text = "Creating new user $sAMAccountName"
            $ADError = $Null
			New-ADUser @User -ErrorVariable ADError
            if ($ADerror){$SB.Text = "[$sAMAccountName] $ADError"}
            else{$SB.Text = "$sAMAccountName created successfully."}
		}
	}
	
	
	# --End User Generated Script--
	#----------------------------------------------
	#region Generated Events
	#----------------------------------------------
	
	$Form_StateCorrection_Load=
	{
		#Correct the initial state of the form to prevent the .Net maximized form issue
		$formMain.WindowState = $InitialFormWindowState
	}
	
	$Form_Cleanup_FormClosed=
	{
		#Remove all event handlers from the controls
		try
		{
			$btnSubmitAll.remove_Click($btnSubmitAll_Click)
			$btnLast.remove_Click($btnLast_Click)
			$btnNext.remove_Click($btnNext_Click)
			$btnPrev.remove_Click($btnPrev_Click)
			$btnFirst.remove_Click($btnFirst_Click)
			$btnImportCSV.remove_Click($btnImportCSV_Click)
			$lvCSV.remove_SelectedIndexChanged($lvCSV_SelectedIndexChanged)
			$cboSite.remove_SelectedIndexChanged($cboSite_SelectedIndexChanged)
			$cboDomain.remove_SelectedIndexChanged($cboDomain_SelectedIndexChanged)
			$txtLastName.remove_TextChanged($txtName_TextChanged)
			$txtFirstName.remove_TextChanged($txtName_TextChanged)
			$btnSubmit.remove_Click($btnSubmit_Click)
			$formMain.remove_Load($formMain_Load)
			$formMode.remove_Click($formMode_Click)
			$CSVTemplate.remove_FileOk($CSVTemplate_FileOk)
			$CreateCSVTemplate.remove_Click($createTemplateToolStripMenuItem_Click)
			$MenuExit.remove_Click($MenuExit_Click)
			$formMain.remove_Load($Form_StateCorrection_Load)
			$formMain.remove_FormClosed($Form_Cleanup_FormClosed)
		}
		catch [Exception]
		{ }
	}
	#endregion Generated Events

	#----------------------------------------------
	#region Generated Form Code
	#----------------------------------------------
	#
	# formMain
	#
	$formMain.Controls.Add($btnSubmitAll)
	$formMain.Controls.Add($btnLast)
	$formMain.Controls.Add($btnNext)
	$formMain.Controls.Add($btnPrev)
	$formMain.Controls.Add($btnFirst)
	$formMain.Controls.Add($btnImportCSV)
	$formMain.Controls.Add($lvCSV)
	$formMain.Controls.Add($txtUPN)
	$formMain.Controls.Add($txtsAM)
	$formMain.Controls.Add($txtDN)
	$formMain.Controls.Add($cboDepartment)
	$formMain.Controls.Add($labelUserPrincipalName)
	$formMain.Controls.Add($labelSamAccountName)
	$formMain.Controls.Add($labelDisplayName)
	$formMain.Controls.Add($SB)
	$formMain.Controls.Add($cboSite)
	$formMain.Controls.Add($labelSite)
	$formMain.Controls.Add($cboDescription)
	$formMain.Controls.Add($txtPassword)
	$formMain.Controls.Add($labelPassword)
	$formMain.Controls.Add($cboDomain)
	$formMain.Controls.Add($labelCurrentDomain)
	$formMain.Controls.Add($txtPostalCode)
	$formMain.Controls.Add($txtState)
	$formMain.Controls.Add($txtCity)
	$formMain.Controls.Add($txtStreetAddress)
	$formMain.Controls.Add($txtOffice)
	$formMain.Controls.Add($txtCompany)
	$formMain.Controls.Add($txtTitle)
	$formMain.Controls.Add($txtOfficePhone)
	$formMain.Controls.Add($txtLastName)
	$formMain.Controls.Add($cboPath)
	$formMain.Controls.Add($labelOU)
	$formMain.Controls.Add($txtFirstName)
	$formMain.Controls.Add($labelPostalCode)
	$formMain.Controls.Add($labelState)
	$formMain.Controls.Add($labelCity)
	$formMain.Controls.Add($labelStreetAddress)
	$formMain.Controls.Add($labelOffice)
	$formMain.Controls.Add($labelCompany)
	$formMain.Controls.Add($labelDepartment)
	$formMain.Controls.Add($labelTitle)
	$formMain.Controls.Add($btnSubmit)
	$formMain.Controls.Add($labelDescription)
	$formMain.Controls.Add($labelOfficePhone)
	$formMain.Controls.Add($labelLastName)
	$formMain.Controls.Add($labelFirstName)
	$formMain.Controls.Add($menustrip1)
	$formMain.AcceptButton = $btnSubmit
	$formMain.ClientSize = '304, 597'
	$System_Windows_Forms_MenuStrip_1 = New-Object System.Windows.Forms.MenuStrip
	$System_Windows_Forms_MenuStrip_1.Location = '0, 0'
	$System_Windows_Forms_MenuStrip_1.Name = ""
	$System_Windows_Forms_MenuStrip_1.Size = '271, 24'
	$System_Windows_Forms_MenuStrip_1.TabIndex = 1
	$System_Windows_Forms_MenuStrip_1.Visible = $False
	$formMain.MainMenuStrip = $System_Windows_Forms_MenuStrip_1
	$formMain.Name = "formMain"
	$formMain.ShowIcon = $False
	$formMain.StartPosition = 'CenterScreen'
	$formMain.Text = "Arposh New User Creation"
	$formMain.add_Load($formMain_Load)
	#
	# btnSubmitAll
	#
	$btnSubmitAll.Location = '503, 0'
	$btnSubmitAll.Name = "btnSubmitAll"
	$btnSubmitAll.Size = '75, 25'
	$btnSubmitAll.TabIndex = 59
	$btnSubmitAll.Text = "Submit All"
	$btnSubmitAll.UseVisualStyleBackColor = $True
	$btnSubmitAll.Visible = $False
	$btnSubmitAll.add_Click($btnSubmitAll_Click)
	#
	# btnLast
	#
	$btnLast.Location = '472, 0'
	$btnLast.Name = "btnLast"
	$btnLast.Size = '30, 25'
	$btnLast.TabIndex = 58
	$btnLast.Text = ">>"
	$btnLast.UseVisualStyleBackColor = $True
	$btnLast.Visible = $False
	$btnLast.add_Click($btnLast_Click)
	#
	# btnNext
	#
	$btnNext.Location = '441, 0'
	$btnNext.Name = "btnNext"
	$btnNext.Size = '30, 25'
	$btnNext.TabIndex = 57
	$btnNext.Text = ">"
	$btnNext.UseVisualStyleBackColor = $True
	$btnNext.Visible = $False
	$btnNext.add_Click($btnNext_Click)
	#
	# btnPrev
	#
	$btnPrev.Location = '410, 0'
	$btnPrev.Name = "btnPrev"
	$btnPrev.Size = '30, 25'
	$btnPrev.TabIndex = 56
	$btnPrev.Text = "<"
	$btnPrev.UseVisualStyleBackColor = $True
	$btnPrev.Visible = $False
	$btnPrev.add_Click($btnPrev_Click)
	#
	# btnFirst
	#
	$btnFirst.Location = '379, 0'
	$btnFirst.Name = "btnFirst"
	$btnFirst.Size = '30, 25'
	$btnFirst.TabIndex = 55
	$btnFirst.Text = "<<"
	$btnFirst.UseVisualStyleBackColor = $True
	$btnFirst.Visible = $False
	$btnFirst.add_Click($btnFirst_Click)
	#
	# btnImportCSV
	#
	$btnImportCSV.Location = '303, 0'
	$btnImportCSV.Name = "btnImportCSV"
	$btnImportCSV.Size = '75, 25'
	$btnImportCSV.TabIndex = 54
	$btnImportCSV.Text = "Import CSV"
	$btnImportCSV.UseVisualStyleBackColor = $True
	$btnImportCSV.Visible = $False
	$btnImportCSV.add_Click($btnImportCSV_Click)
	#
	# lvCSV
	#
	$lvCSV.FullRowSelect = $True
	$lvCSV.GridLines = $True
	$lvCSV.Location = '305, 35'
	$lvCSV.Name = "lvCSV"
	$lvCSV.Size = '1150, 535'
	$lvCSV.TabIndex = 53
	$lvCSV.UseCompatibleStateImageBehavior = $False
	$lvCSV.View = 'Details'
	$lvCSV.Visible = $False
	$lvCSV.add_SelectedIndexChanged($lvCSV_SelectedIndexChanged)
	#
	# txtUPN
	#
	$txtUPN.Anchor = 'Top, Left, Right'
	$txtUPN.Location = '118, 505'
	$txtUPN.Name = "txtUPN"
	$txtUPN.Size = '173, 20'
	$txtUPN.TabIndex = 51
	#
	# txtsAM
	#
	$txtsAM.Anchor = 'Top, Left, Right'
	$txtsAM.Location = '118, 480'
	$txtsAM.Name = "txtsAM"
	$txtsAM.Size = '173, 20'
	$txtsAM.TabIndex = 50
	#
	# txtDN
	#
	$txtDN.Anchor = 'Top, Left, Right'
	$txtDN.Location = '118, 455'
	$txtDN.Name = "txtDN"
	$txtDN.Size = '173, 20'
	$txtDN.TabIndex = 49
	#
	# cboDepartment
	#
	$cboDepartment.Anchor = 'Top, Left, Right'
	$cboDepartment.FormattingEnabled = $True
	$cboDepartment.Location = '118, 235'
	$cboDepartment.Name = "cboDepartment"
	$cboDepartment.Size = '173, 21'
	$cboDepartment.TabIndex = 8
	#
	# labelUserPrincipalName
	#
	$labelUserPrincipalName.Location = '10, 505'
	$labelUserPrincipalName.Name = "labelUserPrincipalName"
	$labelUserPrincipalName.Size = '100, 23'
	$labelUserPrincipalName.TabIndex = 48
	$labelUserPrincipalName.Text = "userPrincipalName"
	$labelUserPrincipalName.TextAlign = 'MiddleLeft'
	#
	# labelSamAccountName
	#
	$labelSamAccountName.Location = '10, 480'
	$labelSamAccountName.Name = "labelSamAccountName"
	$labelSamAccountName.Size = '100, 23'
	$labelSamAccountName.TabIndex = 47
	$labelSamAccountName.Text = "samAccountName"
	$labelSamAccountName.TextAlign = 'MiddleLeft'
	#
	# labelDisplayName
	#
	$labelDisplayName.Location = '10, 455'
	$labelDisplayName.Name = "labelDisplayName"
	$labelDisplayName.Size = '100, 23'
	$labelDisplayName.TabIndex = 46
	$labelDisplayName.Text = "Display Name"
	$labelDisplayName.TextAlign = 'MiddleLeft'
	#
	# SB
	#
	$SB.Location = '0, 575'
	$SB.Name = "SB"
	$SB.Size = '304, 22'
	$SB.TabIndex = 45
	$SB.Text = "Ready"
	#
	# cboSite
	#
	$cboSite.Anchor = 'Top, Left, Right'
	$cboSite.FormattingEnabled = $True
	$cboSite.Location = '118, 320'
	$cboSite.Name = "cboSite"
	$cboSite.Size = '173, 21'
	$cboSite.TabIndex = 11
	$cboSite.add_SelectedIndexChanged($cboSite_SelectedIndexChanged)
	#
	# labelSite
	#
	$labelSite.Location = '10, 320'
	$labelSite.Name = "labelSite"
	$labelSite.Size = '100, 23'
	$labelSite.TabIndex = 44
	$labelSite.Text = "Site"
	$labelSite.TextAlign = 'MiddleLeft'
	#
	# cboDescription
	#
	$cboDescription.Anchor = 'Top, Left, Right'
	$cboDescription.FormattingEnabled = $True
	$cboDescription.Location = '118, 210'
	$cboDescription.Name = "cboDescription"
	$cboDescription.Size = '173, 21'
	$cboDescription.TabIndex = 7
	#
	# txtPassword
	#
	$txtPassword.Anchor = 'Top, Left, Right'
	$txtPassword.Location = '118, 547'
	$txtPassword.Name = "txtPassword"
	$txtPassword.Size = '173, 20'
	$txtPassword.TabIndex = 16
	$txtPassword.UseSystemPasswordChar = $True
	#
	# labelPassword
	#
	$labelPassword.Location = '10, 545'
	$labelPassword.Name = "labelPassword"
	$labelPassword.Size = '100, 23'
	$labelPassword.TabIndex = 41
	$labelPassword.Text = "Password"
	$labelPassword.TextAlign = 'MiddleLeft'
	#
	# cboDomain
	#
	$cboDomain.Anchor = 'Top, Left, Right'
	$cboDomain.FormattingEnabled = $True
	$cboDomain.Location = '118, 35'
	$cboDomain.Name = "cboDomain"
	$cboDomain.Size = '173, 21'
	$cboDomain.TabIndex = 1
	$cboDomain.add_SelectedIndexChanged($cboDomain_SelectedIndexChanged)
	#
	# labelCurrentDomain
	#
	$labelCurrentDomain.Location = '10, 35'
	$labelCurrentDomain.Name = "labelCurrentDomain"
	$labelCurrentDomain.Size = '100, 23'
	$labelCurrentDomain.TabIndex = 39
	$labelCurrentDomain.Text = "Current Domain"
	$labelCurrentDomain.TextAlign = 'MiddleLeft'
	#
	# txtPostalCode
	#
	$txtPostalCode.Anchor = 'Top, Left, Right'
	$txtPostalCode.Location = '118, 420'
	$txtPostalCode.Name = "txtPostalCode"
	$txtPostalCode.Size = '173, 20'
	$txtPostalCode.TabIndex = 15
	#
	# txtState
	#
	$txtState.Anchor = 'Top, Left, Right'
	$txtState.Location = '118, 395'
	$txtState.Name = "txtState"
	$txtState.Size = '173, 20'
	$txtState.TabIndex = 14
	#
	# txtCity
	#
	$txtCity.Anchor = 'Top, Left, Right'
	$txtCity.Location = '118, 370'
	$txtCity.Name = "txtCity"
	$txtCity.Size = '173, 20'
	$txtCity.TabIndex = 13
	#
	# txtStreetAddress
	#
	$txtStreetAddress.Anchor = 'Top, Left, Right'
	$txtStreetAddress.Location = '118, 345'
	$txtStreetAddress.Name = "txtStreetAddress"
	$txtStreetAddress.Size = '173, 20'
	$txtStreetAddress.TabIndex = 12
	#
	# txtOffice
	#
	$txtOffice.Anchor = 'Top, Left, Right'
	$txtOffice.Location = '118, 160'
	$txtOffice.Name = "txtOffice"
	$txtOffice.Size = '173, 20'
	$txtOffice.TabIndex = 5
	#
	# txtCompany
	#
	$txtCompany.Anchor = 'Top, Left, Right'
	$txtCompany.Location = '118, 260'
	$txtCompany.Name = "txtCompany"
	$txtCompany.Size = '173, 20'
	$txtCompany.TabIndex = 9
	#
	# txtTitle
	#
	$txtTitle.Anchor = 'Top, Left, Right'
	$txtTitle.Location = '118, 185'
	$txtTitle.Name = "txtTitle"
	$txtTitle.Size = '173, 20'
	$txtTitle.TabIndex = 6
	#
	# txtOfficePhone
	#
	$txtOfficePhone.Anchor = 'Top, Left, Right'
	$txtOfficePhone.Location = '118, 285'
	$txtOfficePhone.Name = "txtOfficePhone"
	$txtOfficePhone.Size = '173, 20'
	$txtOfficePhone.TabIndex = 10
	#
	# txtLastName
	#
	$txtLastName.Anchor = 'Top, Left, Right'
	$txtLastName.Location = '118, 135'
	$txtLastName.Name = "txtLastName"
	$txtLastName.Size = '173, 20'
	$txtLastName.TabIndex = 4
	$txtLastName.add_TextChanged($txtName_TextChanged)
	#
	# cboPath
	#
	$cboPath.Anchor = 'Top, Left, Right'
	$cboPath.FormattingEnabled = $True
	$cboPath.Location = '45, 65'
	$cboPath.Name = "cboPath"
	$cboPath.Size = '247, 21'
	$cboPath.TabIndex = 2
	#
	# labelOU
	#
	$labelOU.Location = '10, 65'
	$labelOU.Name = "labelOU"
	$labelOU.Size = '36, 23'
	$labelOU.TabIndex = 26
	$labelOU.Text = "OU"
	$labelOU.TextAlign = 'MiddleLeft'
	#
	# txtFirstName
	#
	$txtFirstName.Anchor = 'Top, Left, Right'
	$txtFirstName.Location = '118, 110'
	$txtFirstName.Name = "txtFirstName"
	$txtFirstName.Size = '173, 20'
	$txtFirstName.TabIndex = 3
	$txtFirstName.add_TextChanged($txtName_TextChanged)
	#
	# labelPostalCode
	#
	$labelPostalCode.Location = '10, 420'
	$labelPostalCode.Name = "labelPostalCode"
	$labelPostalCode.Size = '100, 23'
	$labelPostalCode.TabIndex = 24
	$labelPostalCode.Text = "Postal Code"
	$labelPostalCode.TextAlign = 'MiddleLeft'
	#
	# labelState
	#
	$labelState.Location = '10, 395'
	$labelState.Name = "labelState"
	$labelState.Size = '100, 23'
	$labelState.TabIndex = 23
	$labelState.Text = "State"
	$labelState.TextAlign = 'MiddleLeft'
	#
	# labelCity
	#
	$labelCity.Location = '10, 370'
	$labelCity.Name = "labelCity"
	$labelCity.Size = '100, 23'
	$labelCity.TabIndex = 22
	$labelCity.Text = "City"
	$labelCity.TextAlign = 'MiddleLeft'
	#
	# labelStreetAddress
	#
	$labelStreetAddress.Location = '10, 345'
	$labelStreetAddress.Name = "labelStreetAddress"
	$labelStreetAddress.Size = '100, 23'
	$labelStreetAddress.TabIndex = 21
	$labelStreetAddress.Text = "Street Address"
	$labelStreetAddress.TextAlign = 'MiddleLeft'
	#
	# labelOffice
	#
	$labelOffice.Location = '10, 160'
	$labelOffice.Name = "labelOffice"
	$labelOffice.Size = '100, 23'
	$labelOffice.TabIndex = 20
	$labelOffice.Text = "Office"
	$labelOffice.TextAlign = 'MiddleLeft'
	#
	# labelCompany
	#
	$labelCompany.Location = '10, 260'
	$labelCompany.Name = "labelCompany"
	$labelCompany.Size = '100, 23'
	$labelCompany.TabIndex = 19
	$labelCompany.Text = "Company"
	$labelCompany.TextAlign = 'MiddleLeft'
	#
	# labelDepartment
	#
	$labelDepartment.Location = '10, 235'
	$labelDepartment.Name = "labelDepartment"
	$labelDepartment.Size = '100, 23'
	$labelDepartment.TabIndex = 18
	$labelDepartment.Text = "Department"
	$labelDepartment.TextAlign = 'MiddleLeft'
	#
	# labelTitle
	#
	$labelTitle.Location = '10, 185'
	$labelTitle.Name = "labelTitle"
	$labelTitle.Size = '100, 23'
	$labelTitle.TabIndex = 17
	$labelTitle.Text = "Title"
	$labelTitle.TextAlign = 'MiddleLeft'
	#
	# btnSubmit
	#
	$btnSubmit.Location = '216, 0'
	$btnSubmit.Name = "btnSubmit"
	$btnSubmit.Size = '75, 25'
	$btnSubmit.TabIndex = 17
	$btnSubmit.Text = "Submit"
	$btnSubmit.UseVisualStyleBackColor = $True
	$btnSubmit.add_Click($btnSubmit_Click)
	#
	# labelDescription
	#
	$labelDescription.Location = '10, 210'
	$labelDescription.Name = "labelDescription"
	$labelDescription.Size = '100, 23'
	$labelDescription.TabIndex = 15
	$labelDescription.Text = "Description"
	$labelDescription.TextAlign = 'MiddleLeft'
	#
	# labelOfficePhone
	#
	$labelOfficePhone.Location = '10, 285'
	$labelOfficePhone.Name = "labelOfficePhone"
	$labelOfficePhone.Size = '100, 23'
	$labelOfficePhone.TabIndex = 14
	$labelOfficePhone.Text = "Office Phone"
	$labelOfficePhone.TextAlign = 'MiddleLeft'
	#
	# labelLastName
	#
	$labelLastName.Location = '10, 135'
	$labelLastName.Name = "labelLastName"
	$labelLastName.Size = '100, 23'
	$labelLastName.TabIndex = 13
	$labelLastName.Text = "Last Name"
	$labelLastName.TextAlign = 'MiddleLeft'
	#
	# labelFirstName
	#
	$labelFirstName.Location = '10, 110'
	$labelFirstName.Name = "labelFirstName"
	$labelFirstName.Size = '100, 23'
	$labelFirstName.TabIndex = 12
	$labelFirstName.Text = "First Name"
	$labelFirstName.TextAlign = 'MiddleLeft'
	#
	# menustrip1
	#
	[void]$menustrip1.Items.Add($fileToolStripMenuItem)
	$menustrip1.Location = '0, 0'
	$menustrip1.Name = "menustrip1"
	$menustrip1.Size = '304, 24'
	$menustrip1.TabIndex = 52
	$menustrip1.Text = "menustrip1"
	#
	# fileToolStripMenuItem
	#
	[void]$fileToolStripMenuItem.DropDownItems.Add($formMode)
	[void]$fileToolStripMenuItem.DropDownItems.Add($CreateCSVTemplate)
	[void]$fileToolStripMenuItem.DropDownItems.Add($MenuExit)
	$fileToolStripMenuItem.Name = "fileToolStripMenuItem"
	$fileToolStripMenuItem.Size = '37, 20'
	$fileToolStripMenuItem.Text = "File"
	#
	# formMode
	#
	$formMode.Name = "formMode"
	$formMode.Size = '185, 22'
	$formMode.Text = "CSV Mode"
	$formMode.add_Click($formMode_Click)
	#
	# CSVTemplate
	#
	$CSVTemplate.CheckPathExists = $False
	$CSVTemplate.DefaultExt = "csv"
	$CSVTemplate.FileName = "ANUCusers.csv"
	$CSVTemplate.Filter = "CSV Files|*.csv|All Files|*.*"
	$CSVTemplate.ShowHelp = $True
	$CSVTemplate.Title = "Create CSV Template For ANUC"
	$CSVTemplate.add_FileOk($CSVTemplate_FileOk)
	#
	# OFDImportCSV
	#
	$OFDImportCSV.FileName = "C:\ANUC\AnucUsers.csv"
	$OFDImportCSV.ShowHelp = $True
	#
	# CreateCSVTemplate
	#
	$CreateCSVTemplate.Name = "CreateCSVTemplate"
	$CreateCSVTemplate.Size = '185, 22'
	$CreateCSVTemplate.Text = "Create CSV Template"
	$CreateCSVTemplate.add_Click($createTemplateToolStripMenuItem_Click)
	#
	# MenuExit
	#
	$MenuExit.Name = "MenuExit"
	$MenuExit.Size = '185, 22'
	$MenuExit.Text = "Exit"
	$MenuExit.add_Click($MenuExit_Click)
	#endregion Generated Form Code

	#----------------------------------------------

	#Save the initial state of the form
	$InitialFormWindowState = $formMain.WindowState
	#Init the OnLoad event to correct the initial state of the form
	$formMain.add_Load($Form_StateCorrection_Load)
	#Clean up the control events
	$formMain.add_FormClosed($Form_Cleanup_FormClosed)
	#Show the Form
	return $formMain.ShowDialog()

} #End Function

#Call OnApplicationLoad to initialize
if((OnApplicationLoad) -eq $true)
{
	#Call the form
	Call-ANUC_pff | Out-Null
	#Perform cleanup
	OnApplicationExit
}

param()
cls
cd C:\Users\clinth\Documents\~MyDocs\~Projects\PAL_PS\PALWizard\bin\Debug

Function Test-Property 
{
    #// Function provided by Jeffrey Snover
    #// Tests if a property is a memory of an object.
	param ([Parameter(Position=0,Mandatory=1)]$InputObject,[Parameter(Position=1,Mandatory=1)]$Name)
	[Bool](Get-Member -InputObject $InputObject -Name $Name -MemberType *Property)
}

Function RemoveCounterComputer
{
    param($sCounterPath)
    
	#'\\IDCWEB1\Processor(_Total)\% Processor Time"
	[string] $sString = ""
	#// Remove the double backslash if exists
	If ($sCounterPath.substring(0,2) -eq "\\")
	{		
		$sComputer = $sCounterPath.substring(2)
		$iLocThirdBackSlash = $sComputer.IndexOf("\")
		$sString = $sComputer.substring($iLocThirdBackSlash)
	}
	Else
	{
		$sString = $sCounterPath
	}		
		Return $sString	
}

Function RemoveCounterNameAndComputerName
{
    param($sCounterPath)
    
    If ($sCounterPath.substring(0,2) -eq "\\")
    {
    	$sCounterObject = RemoveCounterComputer $sCounterPath
    }
    Else
    {
        $sCounterObject = $sCounterPath
    }
	# \Paging File(\??\C:\pagefile.sys)\% Usage Peak
	# \(MSSQL|SQLServer).*:Memory Manager\Total Server Memory (KB)
	$aCounterObject = $sCounterObject.split("\")
	$iLenOfCounterName = $aCounterObject[$aCounterObject.GetUpperBound(0)].length
	$sCounterObject = $sCounterObject.substring(0,$sCounterObject.length - $iLenOfCounterName)
	$sCounterObject = $sCounterObject.Trim("\")
    Return $sCounterObject 	    
}

Function ReadThresholdFileIntoMemory
{
	param($sThresholdFilePath)
	
	[xml] (Get-Content $sThresholdFilePath)	
}

Function RemoveCounterComputer
{
    param($sCounterPath)
    
	#'\\IDCWEB1\Processor(_Total)\% Processor Time"
	[string] $sString = ""
	#// Remove the double backslash if exists
	If ($sCounterPath.substring(0,2) -eq "\\")
	{		
		$sComputer = $sCounterPath.substring(2)
		$iLocThirdBackSlash = $sComputer.IndexOf("\")
		$sString = $sComputer.substring($iLocThirdBackSlash)
	}
	Else
	{
		$sString = $sCounterPath
	}		
		Return $sString	
}

Function RemoveCounterNameAndComputerName
{
    param($sCounterPath)
    
    If ($sCounterPath.substring(0,2) -eq "\\")
    {
    	$sCounterObject = RemoveCounterComputer $sCounterPath
    }
    Else
    {
        $sCounterObject = $sCounterPath
    }
	# \Paging File(\??\C:\pagefile.sys)\% Usage Peak
	# \(MSSQL|SQLServer).*:Memory Manager\Total Server Memory (KB)
	$aCounterObject = $sCounterObject.split("\")
	$iLenOfCounterName = $aCounterObject[$aCounterObject.GetUpperBound(0)].length
	$sCounterObject = $sCounterObject.substring(0,$sCounterObject.length - $iLenOfCounterName)
	$sCounterObject = $sCounterObject.Trim("\")
    Return $sCounterObject 	    
}

Function GetCounterComputer
{
    param($sCounterPath)
    
	#'\\IDCWEB1\Processor(_Total)\% Processor Time"
	[string] $sComputer = ""
	
	If ($sCounterPath.substring(0,2) -ne "\\")
	{
		Return ""
	}
	$sComputer = $sCounterPath.substring(2)
	$iLocThirdBackSlash = $sComputer.IndexOf("\")
	$sComputer = $sComputer.substring(0,$iLocThirdBackSlash)
	Return $sComputer
}

Function GetCounterObject
{
    param($sCounterPath)
    
	$sCounterObject = RemoveCounterNameAndComputerName $sCounterPath
	
	#// "Paging File(\??\C:\pagefile.sys)"
	$Char = $sCounterObject.Substring(0,1)
	If ($Char -eq "`\")
	{
		$sCounterObject = $sCounterObject.SubString(1)
	}	
	
	$Char = $sCounterObject.Substring($sCounterObject.Length-1,1)	
	If ($Char -ne "`)")
	{
		Return $sCounterObject
	}	
	$iLocOfCounterInstance = 0
	For ($a=$sCounterObject.Length-1;$a -gt 0;$a = $a - 1)
	{			
		$Char = $sCounterObject.Substring($a,1)
		If ($Char -eq "`)")
		{
			$iRightParenCount = $iRightParenCount + 1
		}
		If ($Char -eq "`(")
		{
			$iRightParenCount = $iRightParenCount - 1
		}
		$iLocOfCounterInstance = $a
		If ($iRightParenCount -eq 0){break}
	}
	Return $sCounterObject.Substring(0,$iLocOfCounterInstance)
}

Function GetCounterInstance
{
    param($sCounterPath)
    
	$sCounterObject = RemoveCounterNameAndComputerName $sCounterPath	
	#// "Paging File(\??\C:\pagefile.sys)"
	$Char = $sCounterObject.Substring(0,1)	
	If ($Char -eq "`\")
	{
		$sCounterObject = $sCounterObject.SubString(1)
	}
	$Char = $sCounterObject.Substring($sCounterObject.Length-1,1)	
	If ($Char -ne "`)")
	{
		Return ""
	}	
	$iLocOfCounterInstance = 0
	For ($a=$sCounterObject.Length-1;$a -gt 0;$a = $a - 1)
	{			
		$Char = $sCounterObject.Substring($a,1)
		If ($Char -eq "`)")
		{
			$iRightParenCount = $iRightParenCount + 1
		}
		If ($Char -eq "`(")
		{
			$iRightParenCount = $iRightParenCount - 1
		}
		$iLocOfCounterInstance = $a
		If ($iRightParenCount -eq 0){break}
	}
	$iLenOfInstance = $sCounterObject.Length - $iLocOfCounterInstance - 2
	Return $sCounterObject.Substring($iLocOfCounterInstance+1, $iLenOfInstance)
}

Function GetCounterName
{
    param($sCounterPath)
    
	$aCounterPath = $sCounterPath.Split("\")
	Return $aCounterPath[$aCounterPath.GetUpperBound(0)]
}

Function Get-GUID()
{
	Return "{" + [System.GUID]::NewGUID() + "}"
}

Function ConvertCounterExpressionToVarName($sCounterExpression)
{
	$sCounterObject = GetCounterObject $sCounterExpression
	$sCounterName = GetCounterName $sCounterExpression
	$sCounterInstance = GetCounterInstance $sCounterExpression	
	If ($sCounterInstance -ne "*")
	{
		$sResult = $sCounterObject + $sCounterName + $sCounterInstance
	}
	Else
	{
		$sResult = $sCounterObject + $sCounterName + "ALL"
	}
	$sResult = $sResult -replace "/", ""
	$sResult = $sResult -replace "\.", ""
	$sResult = $sResult -replace "%", "Percent"
	$sResult = $sResult -replace " ", ""
	$sResult = $sResult -replace "\.", ""
	$sResult = $sResult -replace ":", ""
	$sResult = $sResult -replace "\(", ""
	$sResult = $sResult -replace "\)", ""
	$sResult = $sResult -replace "-", ""
	$sResult
}

Function TranslateTo-Language
{
    param($Text,$FromLanguageCode,$ToLanguageCode)
    #$null = [Reflection.Assembly]::LoadWithPartialName("System.Web")
    
    #// Split up the text into sentences
    $aSentences = @($Text.Split('.'))
    
    For ($s = 0;$s -lt $aSentences.Count;$s++)
    {
        $sSentence = $aSentences[$s]
        $null = [Reflection.Assembly]::LoadWithPartialName("System.Web")
        $UrlEncodedText = [System.Web.HttpUtility]::UrlEncode($sSentence)   
        $AppID = '9D6C0A30281B418BF0761043B8EB16B10EC8E2EA'
        
        #[string] $requestString = "http://api.bing.net/xml.aspx?AppId=$AppID&Query=$Text&Sources=Translation&Version=2.2&Translation.SourceLanguage=$FromLanguageCode&Translation.TargetLanguage=$ToLanguageCode" 
        [string] $requestString = "http://api.bing.net/xml.aspx?AppId=$AppID&Query=$UrlEncodedText&Sources=Translation&Version=2.2&Translation.SourceLanguage=$FromLanguageCode&Translation.TargetLanguage=$ToLanguageCode" 

        # Create and initialize the request. 
        [System.Net.HttpWebRequest] $request = [System.Net.HttpWebRequest]::Create($requestString)

        # Send the request; display the response. 
        [System.Net.HttpWebResponse] $response = [System.Net.HttpWebResponse] $request.GetResponse()

        [System.Xml.XmlDocument] $xmlDocument = New-Object System.Xml.XmlDocument
        $xmlDocument.Load($response.GetResponseStream())
        $xmlRoot = $xmlDocument.DocumentElement
        $TranslatedText = $xmlRoot.Translation.Results.TranslationResult.TranslatedTerm
        If ($TranslatedText -eq $null)
        {
            #Start-Sleep -Seconds 60
            $aSentences[$s] = $sSentence
            #$TranslatedText = TranslateTo-Language -Text $Text -FromLanguageCode $FromLanguageCode -ToLanguageCode $ToLanguageCode
        }
        Else
        {
            $aSentences[$s] = $TranslatedText
        }    
    }

    $sFullText = $aSentences[0]
    For ($i = 1;$i -lt $aSentences.Count;$i++)
    {
        $sFullText = "$sFullText" + '. ' + "$($aSentences[$i])"
    }
    $sFullText
}

$htLanguages = @{'ar'='Arabic';'bg'='Bulgarian';'ca'='Catalan';'zh-CHS'='Chinese (Simplified)';'zh-CHT'='Chinese (Traditional)';'cs'='Czech';'da'='Danish';'nl'='Dutch';'et'='Estonian';'fi'='Finnish';'fr'='French';'de'='German';'el'='Greek';'ht'='Haitian Creole';'he'='Hebrew';'hi'='Hindi';'hu'='Hungarian';'id'='Indonesian';'it'='Italian';'ja'='Japanese';'ko'='Korean';'lv'='Latvian';'lt'='Lithuanian';'no'='Norwegian';'pl'='Polish';'pt'='Portuguese';'ro'='Romanian';'ru'='Russian';'sk'='Slovak';'sl'='Slovenian';'es'='Spanish';'sv'='Swedish';'th'='Thai';'tr'='Turkish';'uk'='Ukrainian';'vi'='Vietnamese'}

ForEach ($sLanguageCode in $htLanguages.Keys)
{
    Write-Host $sLanguageCode
    If ((Test-Path -Path $(".\$sLanguageCode")) -eq $False)
    {
        $oLanguageDirectory = New-Item -Path $(".\$sLanguageCode") -Type Directory
    }
    Else
    {
        $oLanguageDirectory = Get-Item -Path $(".\$sLanguageCode")
    }

    $oXmlFiles = Get-ChildItem -Path * -Include *.xml
    ForEach ($oXmlFile in $oXmlFiles)
    {
        Write-Host "`t$($oXmlFile.BaseName)"
        $sNewFilePath = "$($oXmlFile.BaseName)" -Replace '-en',''
        $sNewFilePath = "$oLanguageDirectory\$sNewFilePath-$sLanguageCode.xml"

        [xml] $XmlPalOneThresholdFile = ReadThresholdFileIntoMemory -sThresholdFilePath $oXmlFile
        $XmlPalTwoThresholdFile = New-Object System.Xml.XmlDocument
        
        $NAME = TranslateTo-Language -Text "$($XmlPalOneThresholdFile.PAL.NAME)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
        $DESCRIPTION = TranslateTo-Language -Text "$($XmlPalOneThresholdFile.PAL.DESCRIPTION)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
        $CONTENTOWNERS = "$($XmlPalOneThresholdFile.PAL.CONTENTOWNERS)"
        $FEEDBACKEMAILADDRESS = "$($XmlPalOneThresholdFile.PAL.FEEDBACKEMAILADDRESS)"
        $VERSION = "$($XmlPalOneThresholdFile.PAL.VERSION)"
        $LANGUAGE = TranslateTo-Language -Text "$($htLanguages[$sLanguageCode])" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
        $PALVERSION = '2.0'
        
        $XmlNewPal = $XmlPalTwoThresholdFile.CreateElement('PAL')
        $XmlNewPal.SetAttribute('NAME',"$NAME")
        $XmlNewPal.SetAttribute('DESCRIPTION',"$DESCRIPTION")
        $XmlNewPal.SetAttribute('CONTENTOWNERS',"$CONTENTOWNERS")
        $XmlNewPal.SetAttribute('FEEDBACKEMAILADDRESS',"$FEEDBACKEMAILADDRESS")
        $XmlNewPal.SetAttribute('VERSION',"$VERSION")
        $XmlNewPal.SetAttribute('LANGUAGE',"$LANGUAGE")
        $XmlNewPal.SetAttribute('LANGUAGECODE',"$sLanguageCode")
        $XmlNewPal.SetAttribute('PALVERSION','2.0')
        [void] $XmlPalTwoThresholdFile.AppendChild($XmlNewPal)
        
        #// INHERITANCE
        ForEach ($XmlInheritance in $XmlPalOneThresholdFile.SelectNodes('//INHERITANCE'))
        {
            $sReplacement = "-$sLanguageCode.xml"
            $sTemp = "$($XmlInheritance.FILEPATH)".Replace('.xml',$sReplacement)
            $sInheritanceFilePath = ".\$sLanguageCode\$sTemp"
            $XmlNewInheritanceNode = $XmlPalTwoThresholdFile.CreateElement("INHERITANCE")
            $XmlNewInheritanceNode.SetAttribute('FILEPATH', "$sInheritanceFilePath")
    		[void] $XmlPalTwoThresholdFile.PAL.AppendChild($XmlNewInheritanceNode)
        }

        #// Analysis
        ForEach ($XmlPalOneAnalysisInstance in $XmlPalOneThresholdFile.SelectNodes('//ANALYSIS'))
        {
        	Write-Host "`t`t$($XmlPalOneAnalysisInstance.NAME)"
        	$sAnalysisName = TranslateTo-Language -Text "$($XmlPalOneAnalysisInstance.NAME)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
            $sAnalysisCategory = TranslateTo-Language -Text "$($XmlPalOneAnalysisInstance.CATEGORY)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode

        	#// ANALYSIS Attributes
            $XmlNewAnalysisNode = $XmlPalTwoThresholdFile.CreateElement("ANALYSIS")
            $XmlNewAnalysisNode.SetAttribute('NAME', "$sAnalysisName")
            $XmlNewAnalysisNode.SetAttribute('ENABLED', "$($XmlPalOneAnalysisInstance.ENABLED)")
            $XmlNewAnalysisNode.SetAttribute('CATEGORY', "$sAnalysisCategory")
        	$XmlNewAnalysisNode.SetAttribute('PRIMARYDATASOURCE', "$($XmlPalOneAnalysisInstance.PRIMARYDATASOURCE)")
            $XmlNewAnalysisNode.SetAttribute('ID', "$($XmlPalOneAnalysisInstance.ID)")
        	$XmlNewAnalysisNode.SetAttribute('FROMALLCOUNTERSTATS', "$($XmlPalOneAnalysisInstance.FROMALLCOUNTERSTATS)")
        		
        	If ($($XmlPalOneAnalysisInstance.DESCRIPTION) -is [System.Xml.XmlElement])
        	{
                $sAnalysisDescription = TranslateTo-Language -Text "$($XmlPalOneAnalysisInstance.DESCRIPTION.get_innertext())" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
        		$XmlNewDescriptionNode = $XmlPalTwoThresholdFile.CreateElement('DESCRIPTION')
        		$XmlNewCDataNode = $XmlPalTwoThresholdFile.CreateCDataSection($sAnalysisDescription)
        		#$XmlNewCDataNode.AppendData("$($XmlPalOneAnalysisInstance.DESCRIPTION.get_innertext())")
        		[void] $XmlNewDescriptionNode.AppendChild($XmlNewCDataNode)
        		[void] $XmlNewAnalysisNode.AppendChild($XmlNewDescriptionNode)
        	}
            
            #// QUESTION
            ForEach ($XmlQuestion in $XmlPalOneAnalysisInstance.SelectNodes('./QUESTION'))
            {
        		$XmlNewQuestionNode = $XmlPalTwoThresholdFile.CreateElement('QUESTION')	
        		$XmlNewQuestionNode.SetAttribute('DATATYPE', "$($XmlQuestion.DATATYPE)")
        		$XmlNewQuestionNode.SetAttribute('DEFAULTVALUE', "$($XmlQuestion.DEFAULTVALUE)")
        		$XmlNewQuestionNode.SetAttribute('QUESTIONVARNAME', "$($XmlQuestion.QUESTIONVARNAME)")
                
                $sQuestion = TranslateTo-Language -Text "$($XmlQuestion.get_innertext())" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
                $XmlNewCDataNode = $XmlPalTwoThresholdFile.CreateCDataSection($sQuestion)
        		[void] $XmlNewQuestionNode.AppendChild($XmlNewCDataNode)
        		[void] $XmlNewAnalysisNode.AppendChild($XmlNewQuestionNode)                
            }
        	
            #// DATASOURCE
        	ForEach ($XmlDataSource in $XmlPalOneAnalysisInstance.SelectNodes('./DATASOURCE'))
        	{
        		$VarName = ConvertCounterExpressionToVarName $XmlDataSource.NAME
        		$XmlNewDataSourceNode = $XmlPalTwoThresholdFile.CreateElement('DATASOURCE')	
        		$XmlNewDataSourceNode.SetAttribute('TYPE', "$($XmlDataSource.TYPE)")
        		$XmlNewDataSourceNode.SetAttribute('NAME', "$($XmlDataSource.NAME)")
        		$XmlNewDataSourceNode.SetAttribute('COLLECTIONVARNAME', "$($XmlDataSource.COLLECTIONVARNAME)")
        		$XmlNewDataSourceNode.SetAttribute('EXPRESSIONPATH', "$($XmlDataSource.EXPRESSIONPATH)")
                                
                If ($(Test-property -InputObject $XmlDataSource -Name 'ISCOUNTEROBJECTREGULAREXPRESSION') -eq $True)
                {
                    $XmlNewDataSourceNode.SetAttribute('ISCOUNTEROBJECTREGULAREXPRESSION', "$($XmlDataSource.ISCOUNTEROBJECTREGULAREXPRESSION)")
                }
                If ($(Test-property -InputObject $XmlDataSource -Name 'ISCOUNTERNAMEREGULAREXPRESSION') -eq $True)
                {
                    $XmlNewDataSourceNode.SetAttribute('ISCOUNTERNAMEREGULAREXPRESSION', "$($XmlDataSource.ISCOUNTERNAMEREGULAREXPRESSION)")
                }
                If ($(Test-property -InputObject $XmlDataSource -Name 'ISCOUNTERINSTANCEREGULAREXPRESSION') -eq $True)
                {
                    $XmlNewDataSourceNode.SetAttribute('ISCOUNTERINSTANCEREGULAREXPRESSION', "$($XmlDataSource.ISCOUNTERINSTANCEREGULAREXPRESSION)")
                }
                If ($(Test-property -InputObject $XmlDataSource -Name 'REGULAREXPRESSIONCOUNTERPATH') -eq $True)
                {
                    $XmlNewDataSourceNode.SetAttribute('REGULAREXPRESSIONCOUNTERPATH', "$($XmlDataSource.REGULAREXPRESSIONCOUNTERPATH)")
                }
        		$XmlNewDataSourceNode.SetAttribute('DATATYPE', "$($XmlDataSource.DATATYPE)")
                
                #// Code for generated data source
                ForEach ($XmlCode in $XmlDataSource.SelectNodes('./CODE'))
                {
                    $sDataSourceCode = "$($XmlCode.get_innertext())"
            		$XmlNewCodeNode = $XmlPalTwoThresholdFile.CreateElement('CODE')
            		$XmlNewCDataNode = $XmlPalTwoThresholdFile.CreateCDataSection("$sDataSourceCode")
            		#$XmlNewCDataNode.AppendData("$($XmlPalOneAnalysisInstance.DESCRIPTION.get_innertext())")
            		[void] $XmlNewCodeNode.AppendChild($XmlNewCDataNode)
            		[void] $XmlNewDataSourceNode.AppendChild($XmlNewCodeNode)                
                }
                
                #// DataSource Exclude
                ForEach ($XmlExclude in $XmlDataSource.SelectNodes('./EXCLUDE'))
                {
            		$XmlNewExcludeNode = $XmlPalTwoThresholdFile.CreateElement('EXCLUDE')
                    $XmlNewExcludeNode.SetAttribute('INSTANCE', "$($XmlExclude.INSTANCE)")
            		[void] $XmlNewDataSourceNode.AppendChild($XmlNewExcludeNode)                
                }                
                
        		[void] $XmlNewAnalysisNode.AppendChild($XmlNewDataSourceNode)
        	}
        	
            #// THRESHOLD
        	ForEach ($XmlThreshold in $XmlPalOneAnalysisInstance.SelectNodes('./THRESHOLD'))
        	{
                $sThresholdName = TranslateTo-Language -Text "$($XmlThreshold.NAME)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
        		$XmlNewThresholdNode = $XmlPalTwoThresholdFile.CreateElement('THRESHOLD')
                
        		$XmlNewThresholdNode.SetAttribute("NAME", "$sThresholdName")
        		$XmlNewThresholdNode.SetAttribute("CONDITION", "$($XmlThreshold.CONDITION)")
        		$XmlNewThresholdNode.SetAttribute("COLOR", "$($XmlThreshold.COLOR)")
        		$XmlNewThresholdNode.SetAttribute("PRIORITY", "$($XmlThreshold.PRIORITY)")
                
                ForEach ($XmlCode in $XmlThreshold.SelectNodes('./CODE'))
                {
                    $sThresholdCode = "$($XmlCode.get_innertext())"
            		$XmlNewCodeNode = $XmlPalTwoThresholdFile.CreateElement('CODE')
            		$XmlNewCDataNode = $XmlPalTwoThresholdFile.CreateCDataSection("$sThresholdCode")
            		[void] $XmlNewCodeNode.AppendChild($XmlNewCDataNode)
            		[void] $XmlNewThresholdNode.AppendChild($XmlNewCodeNode)
                }
                [void] $XmlNewAnalysisNode.AppendChild($XmlNewThresholdNode)
            }            
            
            #// CHART
        	ForEach ($XmlChart in $XmlPalOneAnalysisInstance.SelectNodes('./CHART'))
        	{
                $sChartTitle = TranslateTo-Language -Text "$($XmlChart.CHARTTITLE)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
        		$XmlNewChartNode = $XmlPalTwoThresholdFile.CreateElement('CHART')
        		$XmlNewChartNode.SetAttribute("CHARTTITLE", "$sChartTitle")
        		$XmlNewChartNode.SetAttribute("ISTHRESHOLDSADDED", "$($XmlChart.ISTHRESHOLDSADDED)")
        		$XmlNewChartNode.SetAttribute("DATASOURCE", "$($XmlChart.DATASOURCE)")
        		$XmlNewChartNode.SetAttribute("CHARTLABELS", "$($XmlChart.CHARTLABELS)")        		
                
                ForEach ($XmlSeries in $XmlChart.SelectNodes('./SERIES'))
                {
                    $sSeriesName = TranslateTo-Language -Text "$($XmlSeries.NAME)" -FromLanguageCode 'en' -ToLanguageCode $sLanguageCode
            		$XmlNewSeriesNode = $XmlPalTwoThresholdFile.CreateElement('SERIES')
            		$XmlNewSeriesNode.SetAttribute("NAME", "$sSeriesName")

                    ForEach ($XmlCode in $XmlSeries.SelectNodes('./CODE'))
                    {
                        $sSeriesCode = "$($XmlCode.get_innertext())"
                		$XmlNewCodeNode = $XmlPalTwoThresholdFile.CreateElement('CODE')
                		$XmlNewCDataNode = $XmlPalTwoThresholdFile.CreateCDataSection("$sSeriesCode")
                		[void] $XmlNewCodeNode.AppendChild($XmlNewCDataNode)
                		[void] $XmlNewSeriesNode.AppendChild($XmlNewCodeNode)
                    }
            		[void] $XmlNewChartNode.AppendChild($XmlNewSeriesNode)
                }
                [void] $XmlNewAnalysisNode.AppendChild($XmlNewChartNode)
            }
        		
            [void] $XmlPalTwoThresholdFile.PAL.AppendChild($XmlNewAnalysisNode)
        	$XmlPalTwoThresholdFile.Save($sNewFilePath)
        }
    }
}

Write-Host 'Done!'
trap { continue }
0..100 | % {
	(Get-Variable -Scope $_ myinvocation).value.positionmessage -replace "`n"
}
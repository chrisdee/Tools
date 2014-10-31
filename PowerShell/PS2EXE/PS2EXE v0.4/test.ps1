#Demo file for use with PS2EXE (http://ps2exe.codeplex.com), part of PS2EXE v0.3.0.0

write-host "PS2EXE v0.3.0.0 by Ingo Karstein" -f Cyan
write-host ""
write-host "See " -f Yellow
write-host "   http://ps2exe.codeplex.com" -f Green
write-host "   http://blog.karstein-consulting.com" -f Green
Write-Host ""

$intptrSize = [System.IntPtr]::Size

if( $intptrSize -eq 4 ) {
	Write-Host "This is a 32 bit environment" -ForegroundColor DarkCyan
} else {
	Write-Host "This is a 64 bit environment" -ForegroundColor DarkCyan
}
write-host ""

if( $PSVersionTable.PSVersion.Major -eq 3 ) {
    write-host "This is PowerShell 3.0" -fore DarkCyan
} else {
    if( $PSVersionTable.PSVersion.Major -eq 2 ) {
        write-host "This is PowerShell 2.0" -fore DarkCyan
    } else {
        write-host "This is a unknown PowerShell version." -fore DarkCyan
    }

}

write-host ""

write-host "Thread Appartment State is $([System.Threading.Thread]::CurrentThread.GetApartmentState())"

write-host ""

Get-Credential "test1"


Read-Host "Press ENTER to exit..."
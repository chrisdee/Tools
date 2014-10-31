call "callPS2EXE.bat" "test.ps1" "test.exe" 

call "callPS2EXE.bat" "test.ps1" "test_x64.exe" -x64

call "callPS2EXE.bat" "test.ps1" "test_x86.exe" -x86

call "callPS2EXE.bat" "test.ps1" "test_20_STA.exe" -sta -runtime20

call "callPS2EXE.bat" "test.ps1" "test_30_MTA.exe" -mta -runtime30

call "callPS2EXE.bat" "test.ps1" "test_30_NOCONSOLE.exe" -noconsole -runtime30

call "callPS2EXE.bat" "test.ps1" "test_20_NOCONSOLE.exe" -noconsole -runtime20

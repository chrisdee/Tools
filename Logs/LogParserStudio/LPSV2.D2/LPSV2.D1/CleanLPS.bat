REM Removes all LPS user settings from current and/or previous versions including the library if the library is stored in AppData

CD %AppData%
CD ..

RD "Roaming\ExLPT" /S /Q
RD "Local\ExLPT" /S /Q

Pause
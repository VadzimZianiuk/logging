@echo off
set logPath= ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log
LOGPARSER "Select substr(text, 24, 5) AS Level, Count(1) AS Count from %logPath% GROUP BY Level" -i:TEXTLINE -o:NAT
pause;
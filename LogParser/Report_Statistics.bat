@echo off
set logPath= ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs
LOGPARSER "Select substr(text, 24, 5) AS Level, Count(1) AS Count from %logPath%\*.log GROUP BY Level" -i:TEXTLINE -o:NAT
pause;
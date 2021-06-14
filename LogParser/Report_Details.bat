@echo off
set logPath= ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log
set reportPath=.\Report.csv
set where=Level LIKE 'Warning' OR Level LIKE 'Error' OR Level LIKE 'Fatal'  
LOGPARSER "Select substr(text, 0, 19) AS Date, substr(text, 24, 5) AS Level, substr(text, 32) AS Text INTO %reportPath% FROM %logPath% WHERE %where%" -i:TEXTLINE -o:csv
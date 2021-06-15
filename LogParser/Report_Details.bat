@echo off
setlocal enabledelayedexpansion
set logPath= ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log
set reportPath=.\Report.csv

set "levels[0]=DEBUG" 
set "levels[1]=INFO "
set "levels[2]=WARN "
set "levels[3]=ERROR"
set "levels[4]=FATAL"
for /L %%i in (0 1 4) do call echo   %%i. %%levels[%%i]%%
:start
set /P answer=Choose a level. You can select multiple if you separate with a space:
set "whereSection="
for %%a in (%answer%) do (
	if defined levels[%%a] (
		set whereSection=!whereSection! OR Level LIKE '!levels[%%a]!'
		set "levels[%%a]="
	)
)
if "%whereSection%"=="" goto start

LOGPARSER "Select substr(text, 0, 19) AS Date, substr(text, 24, 5) AS Level, substr(text, 32) AS Text INTO %reportPath% FROM %logPath% WHERE %whereSection:~4%" -i:TEXTLINE -o:csv

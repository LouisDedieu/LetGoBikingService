@echo off

NET SESSION >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo This script requires elevated privileges. Please run as an administrator.
    pause
    exit
)

start "Proxy Server" .\ProxyCacheSOAP\ProxyCacheSOAP\bin\Debug\ProxyCacheSOAP.exe
start "Routing Server" .\LetGoBikingService\LetGoBikingService\bin\Debug\LetGoBikingService.exe

cd LetGoBikingClient
call mvn clean install
call mvn compile
call mvn exec:java



echo Génération terminée.
pause
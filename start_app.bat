@echo off

echo Vérification des privilèges administratifs...
NET SESSION >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo Ce script nécessite des privilèges élevés. Veuillez l'exécuter en tant qu'administrateur.
    pause
    exit
)

echo Vérification de l'installation de Maven...
call mvn -v >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo Maven n'est pas installé ou n'est pas dans le PATH.
    pause
    exit
)

echo Démarrage du serveur proxy...
start "Proxy Server" .\ProxyCacheSOAP\ProxyCacheSOAP\bin\Debug\ProxyCacheSOAP.exe
echo Démarrage du serveur de routage...
start "Routing Server" .\LetGoBikingService\LetGoBikingService\bin\Debug\LetGoBikingService.exe

cd LetGoBikingClient
echo Exécution de Maven clean install...
cmd /c mvn clean install
IF %ERRORLEVEL% NEQ 0 (
    echo Échec de la commande Maven: clean install
    pause
    exit
)

echo Exécution de Maven compile...
cmd /c mvn compile
IF %ERRORLEVEL% NEQ 0 (
    echo Échec de la commande Maven: compile
    pause
    exit
)

echo Exécution de Maven exec:java...
cmd /c mvn exec:java
IF %ERRORLEVEL% NEQ 0 (
    echo Échec de la commande Maven: exec:java
    pause
    exit
)

echo Génération terminée.
pause

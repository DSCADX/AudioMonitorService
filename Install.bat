@echo off
setlocal enabledelayedexpansion

set SERVICE_NAME=AudioMonitorService
set DISPLAY_NAME=Audio Monitor Service
set DESCRIPTION=Monitorea los dispositivos de audio del sistema.

set SERVICE_PATH=%~dp0%SERVICE_NAME%.exe
set SERVICE_PATH="%SERVICE_PATH:"=%"

if not exist %SERVICE_PATH% (
    echo [ERROR] Executable file not found in: %SERVICE_PATH%
    pause
    exit /b 1
)

sc query %SERVICE_NAME% >nul 2>&1
if %errorlevel%==0 (
    echo [INFO] Service "%SERVICE_NAME%" already exist.
    echo [INFO] Trying to update it...    
    sc stop %SERVICE_NAME% >nul 2>&1
    sc delete %SERVICE_NAME% >nul 2>&1
    timeout /t 2 >nul
)

echo [INFO] Installing service "%SERVICE_NAME%"...
sc create %SERVICE_NAME% binPath= %SERVICE_PATH% start= auto DisplayName= "%DISPLAY_NAME%" >nul 2>&1

if not %errorlevel%==0 (
    echo [ERROR] Service creation failed.
    echo verify if you have administrator permissions.
    pause
    exit /b 1
)

sc description %SERVICE_NAME% "%DESCRIPTION%" >nul 2>&1

echo [INFO] Starting service...
sc start %SERVICE_NAME% >nul 2>&1

if not %errorlevel%==0 (
    echo [ERROR] The service could not be started..
    pause
    exit /b 1
)

echo Service "%DISPLAY_NAME%" installed and running.

pause
exit /b 0
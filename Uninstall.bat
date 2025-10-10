@echo off
setlocal

set SERVICE_NAME=AudioMonitorService

sc query %SERVICE_NAME% >nul 2>&1
if not %errorlevel%==0 (
    echo [INFO] Service "%SERVICE_NAME%" does not exist.
    pause
    exit /b 0
)

echo [INFO] Stoping service...
sc stop %SERVICE_NAME% >nul 2>&1
timeout /t 2 >nul

echo [INFO] Deleting service...
sc delete %SERVICE_NAME% >nul 2>&1

if not %errorlevel%==0 (
    echo [ERROR] The service could not be removed.
    echo verify if you have administrator permissions.
    pause
    exit /b 1
)

echo Service deleted successfully.

pause
exit /b 0

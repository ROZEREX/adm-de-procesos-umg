@echo off
setlocal
title Desinstalar - Administrador de Procesos

set "APP=Administrador de Procesos"
set "DESTINO=%LOCALAPPDATA%\ProcesosManager"

echo.
choice /c SN /n /m "  Desinstalar %APP%? (S/N): "
if errorlevel 2 exit /b 0

taskkill /im ProcesosManager.exe /f >nul 2>nul
if exist "%DESTINO%" rmdir /s /q "%DESTINO%"
powershell -NoProfile -Command ^
  "Remove-Item -ErrorAction SilentlyContinue (Join-Path ([Environment]::GetFolderPath('Desktop')) '%APP%.lnk'), (Join-Path ([Environment]::GetFolderPath('Programs')) '%APP%.lnk')"

echo.
echo  Desinstalado. (El .NET SDK no se elimina.)
echo.
pause

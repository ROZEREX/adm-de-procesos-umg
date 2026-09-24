@echo off
setlocal
title Instalador - Administrador de Procesos
cd /d "%~dp0"

set "APP=Administrador de Procesos"
set "DESTINO=%LOCALAPPDATA%\ProcesosManager"
set "RID=win-x64"
if /i "%PROCESSOR_ARCHITECTURE%"=="ARM64" set "RID=win-arm64"

echo.
echo  ==================================================
echo    Instalando %APP%
echo  ==================================================
echo.

rem --- 1. Verificar .NET 10 SDK -------------------------------------------
set "PATH=%ProgramFiles%\dotnet;%LOCALAPPDATA%\Microsoft\dotnet;%PATH%"
dotnet --list-sdks 2>nul | findstr /b "10." >nul
if not errorlevel 1 goto :compilar

echo  [1/3] No se encontro .NET 10 SDK. Instalando...
where winget >nul 2>nul
if errorlevel 1 goto :sinwinget
winget install --id Microsoft.DotNet.SDK.10 -e --silent --accept-package-agreements --accept-source-agreements
goto :verificar

:sinwinget
powershell -NoProfile -ExecutionPolicy Bypass -Command ^
  "$s = Join-Path $env:TEMP 'dotnet-install.ps1';" ^
  "Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile $s -UseBasicParsing;" ^
  "& $s -Channel 10.0 -InstallDir (Join-Path $env:LOCALAPPDATA 'Microsoft\dotnet')"

:verificar

dotnet --list-sdks 2>nul | findstr /b "10." >nul
if errorlevel 1 (
    echo.
    echo  [ERROR] No se pudo instalar .NET 10 SDK.
    echo  Descarguelo manualmente desde https://dotnet.microsoft.com/download/dotnet/10.0
    echo  y vuelva a ejecutar este instalador.
    goto :fin
)

:compilar
rem --- 2. Compilar en un .exe unico -----------------------------------------
echo  [2/3] Compilando la aplicacion (puede tardar un minuto)...
dotnet publish "ProcesosManager\ProcesosManager.csproj" -c Release -r %RID% --self-contained true -p:PublishSingleFile=true -o "%DESTINO%" --nologo -v q
if errorlevel 1 (
    echo.
    echo  [ERROR] Fallo la compilacion. Revise los mensajes de arriba.
    goto :fin
)

rem --- 3. Accesos directos --------------------------------------------------
echo  [3/3] Creando accesos directos...
powershell -NoProfile -ExecutionPolicy Bypass -Command ^
  "$w = New-Object -ComObject WScript.Shell;" ^
  "foreach ($d in @([Environment]::GetFolderPath('Desktop'), [Environment]::GetFolderPath('Programs'))) {" ^
  "  $l = $w.CreateShortcut((Join-Path $d '%APP%.lnk'));" ^
  "  $l.TargetPath = '%DESTINO%\ProcesosManager.exe';" ^
  "  $l.WorkingDirectory = '%DESTINO%';" ^
  "  $l.Save() }"

echo.
echo  ==================================================
echo    Instalacion completada
echo  ==================================================
echo   Carpeta: %DESTINO%
echo   Acceso directo en el Escritorio y en el menu Inicio.
echo.
choice /c SN /n /m "  Abrir la aplicacion ahora? (S/N): "
if errorlevel 2 goto :fin
start "" /d "%DESTINO%" "%DESTINO%\ProcesosManager.exe"
exit /b 0

:fin
echo.
pause

@echo off

SET prjDir=%CD%\PogoVersionMonitor
SET binDir=%prjDir%\bin

:: Download .NET 5.0 installer
echo "Downloading .NET 5.0 installer..."
powershell -Command "iwr -outf ~/Desktop/dotnet-install.ps1 https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.ps1"

:: Make installer executable

:: Install .NET 5.0 SDK
echo "Launching .NET installer..."
powershell -ExecutionPolicy RemoteSigned -File dotnet-install.ps1 -Version 5.0.404

:: Delete .NET 5.0 installer
echo "Deleting .NET installer..."
del dotnet-install.ps1

:: Clone repository
echo "Cloning repository..."
git clone https://github.com/versx/PogoVersionMonitor

:: Change directory into cloned repository
echo "Changing directory..."
cd %prjDir%

:: Build PogoVersionMonitor.dll
echo "Building PogoVersionMonitor..."
dotnet build

:: Copy example config
echo "Copying example files..."
xcopy /s /e %binDir%\config.example.json %binDir%\config.json

echo "Copying static folder..."
xcopy /s /e %prjDir%\static\* %binDir%\static\

echo "Changing directory to build folder..."
cd %binDir%
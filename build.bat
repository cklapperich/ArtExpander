@echo off
setlocal enabledelayedexpansion

:: Store the script's directory
set "SCRIPT_DIR=%~dp0"

:: Define paths relative to script directory
set "SOURCE_DLL=%SCRIPT_DIR%bin\Debug\netstandard2.1\ArtExpander.dll"
set "BEPINEX_PLUGINS=%SCRIPT_DIR%BepInEx\plugins\ArtExpander\ArtExpander.dll"
set "ZIP_NAME=ArtExpander.zip"
set "MOD_FOLDER=%SCRIPT_DIR%BepInEx"
set "DESTINATION_PATH=%SCRIPT_DIR%ArtExpander.zip"

:: Echo paths for verification
echo Current directory: %CD%
echo Script directory: %SCRIPT_DIR%
echo Source DLL path: %SOURCE_DLL%
echo BepInEx plugins path: %BEPINEX_PLUGINS%   


:: Build the project
dotnet build
if errorlevel 1 (
    echo Build failed!
    exit /b %errorlevel%
)

:: Verify source file exists
if not exist "%SOURCE_DLL%" (
    echo ERROR: Source DLL not found at: %SOURCE_DLL%
    exit /b 1
)

:: Copy DLL with verbose output
echo Copying from: %SOURCE_DLL%
echo Copying to: %BEPINEX_PLUGINS%
copy /Y "%SOURCE_DLL%" "%BEPINEX_PLUGINS%"
if errorlevel 1 (
    echo ERROR: Copy failed!
    exit /b %errorlevel%
)

:: Create zip (requires PowerShell)
echo Creating zip file: %DESTINATION_PATH%
powershell -Command "Compress-Archive -Force -Path '%MOD_FOLDER%' -DestinationPath '%DESTINATION_PATH%'"
if errorlevel 1 (
    echo ERROR: Zip creation failed!
    exit /b %errorlevel%
)

echo Build complete!
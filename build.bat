@echo off
setlocal enabledelayedexpansion

:: Store the script's directory
set "SCRIPT_DIR=%~dp0"

:: Define paths relative to script directory
set "SOURCE_DLL=%SCRIPT_DIR%bin\Debug\netstandard2.1\ArtExpander.dll"
set "BEPINEX_PLUGINS=%SCRIPT_DIR%BepInEx\plugins\ArtExpander\ArtExpander.dll"
set "STEAM_PLUGINS=C:\Program Files (x86)\Steam\steamapps\common\TCG Card Shop Simulator\BepInEx\plugins\ArtExpander\ArtExpander.dll"
set "ZIP_NAME=ArtExpander.zip"
set "MOD_FOLDER=%SCRIPT_DIR%BepInEx"
set "DESTINATION_PATH=%SCRIPT_DIR%ArtExpander.zip"

:: Echo paths for verification
echo [INFO] Current directory: %CD%
echo [INFO] Script directory: %SCRIPT_DIR%
echo [INFO] Source DLL path: %SOURCE_DLL%
echo [INFO] BepInEx plugins path: %BEPINEX_PLUGINS%
echo [INFO] Steam plugins path: %STEAM_PLUGINS%

:: Build the project
echo [BUILD] Building project...
dotnet build
if errorlevel 1 (
    echo [ERROR] Build failed!
    exit /b %errorlevel%
)

:: Verify source file exists
if not exist "%SOURCE_DLL%" (
    echo [ERROR] Source DLL not found at: %SOURCE_DLL%
    exit /b 1
)

:: Create directories if they don't exist
echo [INFO] Creating directories...
for %%G in ("%BEPINEX_PLUGINS%" "%STEAM_PLUGINS%") do (
    if not exist "%%~dpG" (
        mkdir "%%~dpG"
        if errorlevel 1 (
            echo [ERROR] Failed to create directory: %%~dpG
            exit /b 1
        )
    )
)

:: Copy DLL to development folder
echo [INFO] Copying to development folder...
copy /Y "%SOURCE_DLL%" "%BEPINEX_PLUGINS%" > nul
if errorlevel 1 (
    echo [ERROR] Development folder copy failed!
    echo [DEBUG] Source: %SOURCE_DLL%
    echo [DEBUG] Destination: %BEPINEX_PLUGINS%
    exit /b %errorlevel%
)

:: Check Steam directory exists and is accessible
echo [INFO] Checking Steam directory...
if not exist "C:\Program Files (x86)\Steam\steamapps\common\TCG Card Shop Simulator" (
    echo [WARNING] Steam game directory not found. Is the game installed?
    choice /M "Do you want to continue without copying to Steam directory"
    if errorlevel 2 exit /b 1
) else (
    :: Copy DLL to Steam folder with enhanced error checking
    echo [INFO] Copying to Steam folder...
    copy /Y "%SOURCE_DLL%" "%STEAM_PLUGINS%" > nul
    if errorlevel 1 (
        echo [ERROR] Steam folder copy failed! Checking permissions...
        :: Test write permissions
        echo. 2> "%STEAM_PLUGINS%\.test" > nul
        if errorlevel 1 (
            echo [ERROR] No write permission to Steam directory. Try running as administrator.
            choice /M "Do you want to continue without copying to Steam directory"
            if errorlevel 2 exit /b 1
        ) else (
            del "%STEAM_PLUGINS%\.test"
            echo [ERROR] Unknown error during copy to Steam directory.
            exit /b 1
        )
    )
)

:: Create zip (requires PowerShell)
echo [INFO] Creating zip file...
if exist "%DESTINATION_PATH%" del "%DESTINATION_PATH%"
powershell -NoProfile -Command "Compress-Archive -Force -Path '%MOD_FOLDER%' -DestinationPath '%DESTINATION_PATH%'"
if errorlevel 1 (
    echo [ERROR] Zip creation failed!
    echo [DEBUG] Source: %MOD_FOLDER%
    echo [DEBUG] Destination: %DESTINATION_PATH%
    exit /b %errorlevel%
)

echo [SUCCESS] Build complete!
#!/bin/bash

# Store the script's directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

<<<<<<< HEAD
# Define paths relative to script directory
SOURCE_DLL="$SCRIPT_DIR/bin/Debug/netstandard2.1/ArtExpander.dll"
BEPINEX_PLUGINS="$SCRIPT_DIR/BepInEx/plugins/ArtExpander/ArtExpander.dll"
=======
# Get current user dynamically
CURRENT_USER="$(whoami)"

# Define paths relative to script directory
SOURCE_DLL="$SCRIPT_DIR/bin/Debug/netstandard2.1/ArtExpander.dll"
BEPINEX_PLUGINS="$SCRIPT_DIR/BepInEx/plugins/ArtExpander/ArtExpander.dll"
STEAM_PLUGINS="/home/$CURRENT_USER/.steam/debian-installation/steamapps/common/TCG Card Shop Simulator/BepInEx/plugins/ArtExpander/ArtExpander.dll"
>>>>>>> foilmask
ZIP_NAME="ArtExpander.zip"
MOD_FOLDER="$SCRIPT_DIR/BepInEx"
DESTINATION_PATH="$SCRIPT_DIR/ArtExpander.zip"

# Echo paths for verification
echo "[INFO] Current directory: $(pwd)"
echo "[INFO] Script directory: $SCRIPT_DIR"
<<<<<<< HEAD
echo "[INFO] Source DLL path: $SOURCE_DLL"
echo "[INFO] BepInEx plugins path: $BEPINEX_PLUGINS"
=======
echo "[INFO] Current user: $CURRENT_USER"
echo "[INFO] Source DLL path: $SOURCE_DLL"
echo "[INFO] BepInEx plugins path: $BEPINEX_PLUGINS"
echo "[INFO] Steam plugins path: $STEAM_PLUGINS"
>>>>>>> foilmask

# Build the project
echo "[BUILD] Building project..."
dotnet build
if [ $? -ne 0 ]; then
    echo "[ERROR] Build failed!"
    exit 1
fi

# Verify source file exists
if [ ! -f "$SOURCE_DLL" ]; then
    echo "[ERROR] Source DLL not found at: $SOURCE_DLL"
    exit 1
fi

# Create directories if they don't exist
echo "[INFO] Creating directories..."
mkdir -p "$(dirname "$BEPINEX_PLUGINS")"
if [ $? -ne 0 ]; then
    echo "[ERROR] Failed to create directory: $(dirname "$BEPINEX_PLUGINS")"
    exit 1
fi

<<<<<<< HEAD
=======
mkdir -p "$(dirname "$STEAM_PLUGINS")"
if [ $? -ne 0 ]; then
    echo "[ERROR] Failed to create directory: $(dirname "$STEAM_PLUGINS")"
    exit 1
fi

>>>>>>> foilmask
# Copy DLL to development folder
echo "[INFO] Copying to development folder..."
cp "$SOURCE_DLL" "$BEPINEX_PLUGINS"
if [ $? -ne 0 ]; then
    echo "[ERROR] Development folder copy failed!"
    echo "[DEBUG] Source: $SOURCE_DLL"
    echo "[DEBUG] Destination: $BEPINEX_PLUGINS"
    exit 1
fi

<<<<<<< HEAD
=======
# Check Steam directory exists and is accessible
echo "[INFO] Checking Steam directory..."
STEAM_GAME_DIR="/home/$CURRENT_USER/.steam/debian-installation/steamapps/common/TCG Card Shop Simulator"
if [ ! -d "$STEAM_GAME_DIR" ]; then
    echo "[WARNING] Steam game directory not found. Is the game installed?"
    read -p "Do you want to continue without copying to Steam directory? (y/N): " choice
    case "$choice" in 
        y|Y ) echo "[INFO] Continuing without Steam copy...";;
        * ) exit 1;;
    esac
else
    # Copy DLL to Steam folder with enhanced error checking
    echo "[INFO] Copying to Steam folder..."
    cp "$SOURCE_DLL" "$STEAM_PLUGINS"
    if [ $? -ne 0 ]; then
        echo "[ERROR] Steam folder copy failed! Checking permissions..."
        # Test write permissions
        if ! touch "$(dirname "$STEAM_PLUGINS")/.test" 2>/dev/null; then
            echo "[ERROR] No write permission to Steam directory. Try running with sudo or check permissions."
            read -p "Do you want to continue without copying to Steam directory? (y/N): " choice
            case "$choice" in 
                y|Y ) echo "[INFO] Continuing without Steam copy...";;
                * ) exit 1;;
            esac
        else
            rm -f "$(dirname "$STEAM_PLUGINS")/.test"
            echo "[ERROR] Unknown error during copy to Steam directory."
            exit 1
        fi
    fi
fi

>>>>>>> foilmask
# Create zip
echo "[INFO] Creating zip file..."
if [ -f "$DESTINATION_PATH" ]; then
    rm "$DESTINATION_PATH"
fi

<<<<<<< HEAD
# Use zip if available, otherwise try tar
if command -v zip &> /dev/null; then
    cd "$SCRIPT_DIR"
    zip -r "$ZIP_NAME" BepInEx/
    if [ $? -ne 0 ]; then
        echo "[ERROR] Zip creation failed!"
        exit 1
    fi
elif command -v tar &> /dev/null; then
    echo "[INFO] zip not found, using tar instead..."
    cd "$SCRIPT_DIR"
    tar -czf "ArtExpander.tar.gz" BepInEx/
    if [ $? -ne 0 ]; then
        echo "[ERROR] Tar creation failed!"
        exit 1
    fi
    echo "[INFO] Created ArtExpander.tar.gz instead of zip"
else
    echo "[WARNING] Neither zip nor tar found, skipping archive creation"
    echo "[INFO] DLL copied successfully to: $BEPINEX_PLUGINS"
fi

echo "[SUCCESS] Build complete!"
echo "[INFO] Built files:"
echo "  - DLL: $BEPINEX_PLUGINS"
if [ -f "$DESTINATION_PATH" ]; then
    echo "  - Archive: $DESTINATION_PATH"
elif [ -f "$SCRIPT_DIR/ArtExpander.tar.gz" ]; then
    echo "  - Archive: $SCRIPT_DIR/ArtExpander.tar.gz"
fi
=======
if command -v zip >/dev/null 2>&1; then
    cd "$SCRIPT_DIR"
    zip -r "$ZIP_NAME" "BepInEx/"
    if [ $? -ne 0 ]; then
        echo "[ERROR] Zip creation failed!"
        echo "[DEBUG] Source: $MOD_FOLDER"
        echo "[DEBUG] Destination: $DESTINATION_PATH"
        exit 1
    fi
else
    echo "[ERROR] 'zip' command not found. Please install zip package."
    exit 1
fi

echo "[SUCCESS] Build complete!"
>>>>>>> foilmask

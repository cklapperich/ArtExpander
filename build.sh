#!/bin/bash

# Store the script's directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Define paths relative to script directory
SOURCE_DLL="$SCRIPT_DIR/bin/Debug/netstandard2.1/ArtExpander.dll"
BEPINEX_PLUGINS="$SCRIPT_DIR/BepInEx/plugins/ArtExpander/ArtExpander.dll"
ZIP_NAME="ArtExpander.zip"
MOD_FOLDER="$SCRIPT_DIR/BepInEx"
DESTINATION_PATH="$SCRIPT_DIR/ArtExpander.zip"

# Echo paths for verification
echo "[INFO] Current directory: $(pwd)"
echo "[INFO] Script directory: $SCRIPT_DIR"
echo "[INFO] Source DLL path: $SOURCE_DLL"
echo "[INFO] BepInEx plugins path: $BEPINEX_PLUGINS"

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

# Copy DLL to development folder
echo "[INFO] Copying to development folder..."
cp "$SOURCE_DLL" "$BEPINEX_PLUGINS"
if [ $? -ne 0 ]; then
    echo "[ERROR] Development folder copy failed!"
    echo "[DEBUG] Source: $SOURCE_DLL"
    echo "[DEBUG] Destination: $BEPINEX_PLUGINS"
    exit 1
fi

# Create zip
echo "[INFO] Creating zip file..."
if [ -f "$DESTINATION_PATH" ]; then
    rm "$DESTINATION_PATH"
fi

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
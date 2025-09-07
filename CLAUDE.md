# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ArtExpander is a BepInEx mod for "Card Shop Simulator" that extends the game's artwork system. It allows custom artwork for different card border types (Base, FirstEdition, Silver, Gold, EX, FullArt) and expansion types, with special support for Destiny cards and Ghost expansions.

## Build Commands

**Build the project:**
```bash
dotnet build
```

**Build and package for distribution:**
```bash
./build.bat
```

The build.bat script:
1. Builds the project using `dotnet build`
2. Copies the DLL to `BepInEx/plugins/ArtExpander/`
3. Creates `ArtExpander.zip` containing the mod files

## Architecture

### Core Components

**Plugin.cs** - Main entry point that:
- Initializes the art cache from either `cardart/` or plugin root directory
- Applies Harmony patches for game integration
- Manages the plugin lifecycle

**ArtCache.cs** - Central artwork management system:
- Caches resolved file paths for (MonsterType, BorderType, ExpansionType) combinations
- Handles fallback logic for missing artwork
- Special handling for Ghost expansion (white/black variants based on isDestiny flag)
- Manages sprite loading and caching

**FileNameToMonsterTypeResolver.cs** - Converts image filenames to game monster types

### Harmony Patches

**CardUISetCardPatch.cs** - Tracks current card data during UI operations using CardDataTracker static class

**GetIconPatch.cs** - Intercepts sprite loading to inject custom artwork

**CardUISetGhostCardUI.cs** - Handles ghost card UI with animation support

### Key Enums and Types

- `EMonsterType` - Game monster types (e.g., PiggyA, PiggyB)
- `ECardBorderType` - Border variants (Base, FirstEdition, Silver, Gold, EX, FullArt)
- `ECardExpansionType` - Expansion types (None for all_expansions, Destiny, Ghost, etc.)
- Special ghost border constants: `GhostWhiteBorder` (-2), `GhostBlackBorder` (-3)

## Development Notes

- Project targets .NET Standard 2.1 with Unity 2021.3.38 references
- Uses BepInEx 5.x framework for Unity game modding
- Harmony library for runtime code patching
- All custom artwork should follow the folder structure documented in the readme file
- The cache system prioritizes specific matches over generic fallbacks
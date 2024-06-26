<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/banner-hc.gif" alt="banner">

<p align="center">
   <img src="https://img.shields.io/badge/Engine-Unity%202022.3.30f1-blueviolet?style=&logo=unity" alt="Engine">
   <img src="https://img.shields.io/badge/Platform-Android%206+%20-darkgreen?style=&logo=android" alt="Platform">
   <img src="https://img.shields.io/badge/Release%20Date-15.06.2024-red" alt="Release Date">
   <img src="https://img.shields.io/badge/Version-1.1.4-blue" alt="Game Version">
   <img src="https://img.shields.io/github/license/Zixxatis/Hundred-Cells" alt="License">
</p>

## Table of Contents
* [About](#About)
* [Features](#features)
	* [Project Systems](#project-systems)
	* [Design Patterns Used](#design-patterns-used)
	* [Optimization for Mobiles](#optimization-for-mobiles)
	* [Custom Editor Scripts](#custom-editor-scripts)
	* [Extensions](#extensions)
- [Screenshots](#Screenshots)
- [License](#License)
- [Contact](#Contact)

## About
Hundred Cells is a mobile puzzle game where players strategically place shapes on a 10x10 grid to clear lines, earning points and unlocking bonuses. Plan ahead to maximize your score and aim for the top of the leaderboards in this challenging game.

Play WebGL version or download .apk for Android from [itch.io](https://zixxatis.itch.io/hundred-cells)
	or download the .apk for Android directly from [Releases](https://github.com/Zixxatis/Hundred-Cells/releases)

### Built With
- [Newtonsoft Json](https://www.newtonsoft.com/json) - advanced JSON serialization and deserialization.
- [Zenject](https://github.com/modesttree/zenject) - a Dependency Injection container for Unity.
- [Dependencies Hunter](https://github.com/AlexeyPerov/Unity-Dependencies-Hunter) - tool for finding asset references in a Unity project.

## Features
### Project Systems
- [Saving System 2.0](https://github.com/Zixxatis/Hundred-Cells/tree/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Saving%20System) -A storage system designed specifically for use with the DI container. Can be used with any class that implements [Data](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Data/Data.cs). Allows reading and writing to a JSON file.
- [Localization System](https://github.com/Zixxatis/Hundred-Cells/tree/main/Assets/Scripts/%5BGlobal%20Scripts%5D/Localization%20System) - A system that allows to localize a TMP text component in Editor and in the runtime.
- [Color Adapter](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/Visual%20Elements/Color%20Adapters/ColorAdapter.cs) - A Graphic extension to match the current color scheme, designed to be used with the DI container.

### Design Patterns Used
- [Template Method](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/In-game%20Objects/Grid%20Modifier/Bonuses/BonusActivator.cs) - Used as base for various [bonuses](https://github.com/Zixxatis/Hundred-Cells/tree/main/Assets/Scripts/In-game%20Objects/Grid%20Modifier/Bonuses) behaviour.
- [Factory](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/In-game%20Objects/Shapes/Shapes%20Factory/ShapesFactory.cs) - Used for initial Shapes creation.
- [Object Pooling](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/In-game%20Objects/Shapes/Shapes%20Factory/ShapesFactory.cs) - Used to hide and reset Shapes instead of instantiating and destroying them.

### Optimization for Mobiles
- Screen Adaptation
	- [Aspect Ratio Fitter](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-AspectRatioFitter.html) Component
	- [Grid Aspect Ratio Maintainer](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/Visual%20Elements/GridAspectRatioMaintainer.cs)

- Build Optimization
	- Sprite optimization - sprite atlases, maintaining POT resolution.
	- UI Elements - canvas separation, maintaining raycast on visual elements.

### Custom Editor Scripts
- [Localization Editor](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Editor/Windows/LocalizationEditor.cs) - An editor window, that helps to maintain the localization file and find any references.
- [Localized Text](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Editor/Inspector%20GUI/TextInputter.cs) Component drawer - Allows to preview localized text in the Editor mode.
- [GameObject Creator](https://github.com/Zixxatis/Hundred-Cells/tree/main/Assets/Editor/GameObject%20Creation) - A simple factory for creating custom objects from the context menu in Edit mode.

### Extensions
- [General Extensions](https://github.com/Zixxatis/Hundred-Cells/tree/main/Assets/Scripts/%5BExtensions%20%26%20Misc%5D/Extensions) - A collection of various extensions for general C# & Unity classes.
- Project-specific extensions
	- [Multidimensional Array Extensions](https://github.com/Zixxatis/Hundred-Cells/blob/main/Assets/Scripts/%5BExtensions%20%26%20Misc%5D/Project%20Specific%20Extensions/MatrixExtensions.cs)

## Screenshots
<p align="center">
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_1.png" alt="screenshot1" width="200"/>
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_2.png" alt="screenshot2" width="200"/>
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_3.png" alt="screenshot3" width="200"/>
<img src="https://github.com/Zixxatis/Hundred-Cells/blob/main/.github/screenshot_4.png" alt="screenshot4" width="200"/>
</p>

## License
This project is open source and available under the [Apache-2.0 License](https://github.com/Zixxatis/Hundred-Cells/blob/main/LICENSE).
## Contact
Created by [@Zixxatis](https://github.com/Zixxatis/) - feel free to contact me!
# ProjectDMG

ProjectDMG is a C# coded emulator of the Nintendo Game Boy wich was originally codenamed Dot Matrix Game (DMG). More information can be found in the the original repository: https://github.com/BluestormDNA/ProjectDMG

## Using the emulator

Use File > Load to open a ROM. The input is mapped as:

* D-Pad UP: **Up** or **W**
* D-Pad Left: **Left** or **A**
* D-Pad Down: **Down** or **S**
* D-Pad Right: **Right** or **D**
* A: **Z** or **J**
* B: **X** or **K**
* Start: **V** or **Enter**
* Select: **C** or **Space**
* Save state: **CTRL+1-3**
* Load state: **CTRL+Shift+1-3**

## Tracking values in memory

The ProjectDMG.API supports subscribing to address ranges to keep track of values. More information can be found at: https://intodot.net/implementing-saving-and-loading-of-states-in-a-c-gb-emulator/

## Creating a plugin

Follow the steps below to create a new plugin and track your own addresses:
* Create a new project
* Create a class that inherits from ProjectDMGPlugin
* Set the following post-build event: 
* * copy /Y "$(TargetDir)$(ProjectName).dll" "$(SolutionDir)PluginsDlls\$(ProjectName).dll"

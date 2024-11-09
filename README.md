## Overview
Port of https://github.com/DigitalRune/DigitalRune for the modern day MonoGame.

So far it works only under Windows.

![image](https://github.com/user-attachments/assets/3452879e-3edc-4a01-bbb6-4c05d450cb25)

## Build Instructions

1. Install mgcb if it's not installed: `dotnet tool install dotnet-mgcb --global`
2. Open `DigitalRune-MonoGame-Windows.sln` in the IDE and build Release version. All projects but `Samples.MonoGame.DirectX` should build succesfully
3. Execute `Build-Content-Release.cmd`
4. Goto `Samples` folder and execute `Build-Content-MonoGame-Windows.cmd`
5. Build the solution in the IDE again and this time `Samples.MonoGame.DirectX` should build succesfully
6. Run `Samples.MonoGame.DirectX`

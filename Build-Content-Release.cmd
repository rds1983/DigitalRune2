@ECHO OFF
SETLOCAL

ECHO ----- Building DigitalRune.Graphics.Content...

:: Change working directory.
cd Source\DigitalRune.Graphics.Content

:: Build content with MonoGame Content Builder tool.
mgcb /@:DigitalRune.Graphics.mgcb || GOTO error

:: ZIP content.
..\..\Tools\Pack\bin\Release\net8.0-windows\Pack.exe --output bin\DigitalRune.zip --recursive --directory bin\Windows *.* || GOTO error

cd ..\..

ECHO.
ECHO SUCCESS - Content build successful.
PAUSE
EXIT

:error
ECHO.
ECHO ERROR - Content build failed.
PAUSE
EXIT /B 1

@echo off

set PROJECT=VSProjectManager_GUI.csproj
set FRAMEWORK=net6.0
set CONFIG=Release

echo === Windows x64 ===
dotnet publish %PROJECT% -c %CONFIG% -f %FRAMEWORK% -r win-x64 ^
--self-contained true ^
/p:PublishSingleFile=true ^
/p:TrimMode=full ^
/p:IncludeNativeLibrariesForSelfExtract=true ^
/p:EnableCompressionInSingleFile=true ^
/p:Optimize=true ^
/p:DebugType=None ^
/p:DebugSymbols=false
  
echo === macOS Intel ===
dotnet publish %PROJECT% -c %CONFIG% -f %FRAMEWORK% -r osx-x64 ^
--self-contained true ^
/p:PublishSingleFile=true ^
/p:TrimMode=full ^
/p:IncludeNativeLibrariesForSelfExtract=true ^
/p:EnableCompressionInSingleFile=true ^
/p:Optimize=true ^
/p:DebugType=None ^
/p:DebugSymbols=false

echo === macOS ARM (M1/M2) ===
dotnet publish %PROJECT% -c %CONFIG% -f %FRAMEWORK% -r osx-arm64 ^
--self-contained true ^
/p:PublishSingleFile=true ^
/p:TrimMode=full ^
/p:IncludeNativeLibrariesForSelfExtract=true ^
/p:EnableCompressionInSingleFile=true ^
/p:Optimize=true ^
/p:DebugType=None ^
/p:DebugSymbols=false

echo === Linux x64 ===
dotnet publish %PROJECT% -c %CONFIG% -f %FRAMEWORK% -r linux-x64 ^
--self-contained true ^
/p:PublishSingleFile=true ^
/p:TrimMode=full ^
/p:IncludeNativeLibrariesForSelfExtract=true ^
/p:EnableCompressionInSingleFile=true ^
/p:Optimize=true ^
/p:DebugType=None ^
/p:DebugSymbols=false

echo Done!
pause

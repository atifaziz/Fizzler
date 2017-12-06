@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
set dotpack=dotnet pack -o ..\..\dist --include-source --include-symbols --no-restore --no-build
set nupack=.nuget\NuGet pack -OutputDirectory dist -NoPackageAnalysis
if not "%~1"=="" (
    set dotpack=%dotpack% --version-suffix %1
    set nupack=%nupack% -Properties VersionSuffix=-%1
)
call build /v:m ^
  && %dotpack% src\Fizzler ^
  && %dotpack% src\Fizzler.Systems.HtmlAgilityPack ^
  && %nupack% Fizzler.Tools.nuspec
goto :EOF

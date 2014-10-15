@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
set nupack=.nuget\NuGet pack -OutputDirectory dist
call build /v:m ^
  && %nupack% -Symbols Fizzler.nuspec ^
  && %nupack% -Symbols Fizzler.Systems.HtmlAgilityPack.nuspec ^
  && %nupack% -NoPackageAnalysis Fizzler.Tools.nuspec
goto :EOF

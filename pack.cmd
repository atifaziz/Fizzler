@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
set dotpack=dotnet pack -o ..\..\dist --include-source --include-symbols
call build /v:m ^
  && %dotpack% src\Fizzler ^
  && %dotpack% src\Fizzler.Systems.HtmlAgilityPack ^
  && .nuget\NuGet pack -OutputDirectory dist -NoPackageAnalysis Fizzler.Tools.nuspec
goto :EOF

@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
if not exist dist md dist
if not %errorlevel%==0 exit /b %errorlevel%
call build /v:m && for %%i in (*.nuspec) do .nuget\NuGet pack %%i -OutputDirectory dist
goto :EOF

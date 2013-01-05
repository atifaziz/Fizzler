@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
set MSBUILDEXE=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
if not exist "%MSBUILDEXE%" (
    echo Microsoft .NET Framework 4.0 does not appear to be installed on this 
    echo machine, which is required to build the solution.
    exit /b 1
)
for %%s in (*.sln) do for %%c in (Debug Release) do if not errorlevel 1 "%MSBUILDEXE%" "%%s" /p:Configuration=%%c /v:m %*
goto :EOF

@echo off
for %%i in (Debug Release) do (
    "%SystemRoot%\Microsoft.NET\Framework\v3.5\msbuild" /p:Configuration=%%i "%~dp0Fizzler.sln"
)

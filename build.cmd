@echo off
setlocal
pushd "%~dp0"
call :main %*
popd
goto :EOF

:main
call msbuild /t:restore ^
  && for %%s in (*.sln) do for %%c in (Debug Release) do if not errorlevel 1 call msbuild "%%s" /p:Configuration=%%c /v:m %*
goto :EOF

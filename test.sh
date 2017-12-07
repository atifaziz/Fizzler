#!/usr/bin/env bash
[[ -e test.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
./build.sh
for c in Debug Release; do
    dotnet test --no-restore --no-build -c $c -f netcoreapp2.0 src/Fizzler.Tests
done
# TODO test net40 builds

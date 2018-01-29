#!/usr/bin/env bash
[[ -e test.sh ]] || { echo >&2 "Please cd into the script location before running it."; exit 1; }
set -e
./build.sh
for c in Debug Release; do
    dotnet test --no-build tests -c $c
done


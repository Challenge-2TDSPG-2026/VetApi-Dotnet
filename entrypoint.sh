#!/bin/bash
set -e
echo "Iniciando VetClinic API..."
exec dotnet VetApi.dll

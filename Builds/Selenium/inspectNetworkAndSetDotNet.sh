
docker network inspect simpitch-tc_backend_network


#!/bin/bash
    set -e
    
    echo "=== Checking .NET versions ==="
    dotnet --version
    dotnet --list-sdks
    
    echo "=== Installing .NET 9.0 SDK ==="
    
    # Sprawdź czy .NET 9.0 jest już zainstalowany
    if dotnet --list-sdks | grep -q "9.0"; then
        echo "✓ .NET 9.0 is already installed"
        exit 0
    fi
    
    # Pobierz i uruchom instalator
    echo "Downloading .NET 9.0 installer..."
    wget -q https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
    chmod +x dotnet-install.sh
    
    # Instaluj do standardowego katalogu
    echo "Installing .NET 9.0..."
    ./dotnet-install.sh --version 9.0 --install-dir /usr/local/dotnet
    
    # Dodaj do PATH (dla tego build stepu)
    export PATH="/usr/local/dotnet:${'$'}PATH"
    
    echo "=== Verification ==="
    /usr/local/dotnet/dotnet --version
    /usr/local/dotnet/dotnet --list-sdks
    
    rm -f dotnet-install.sh
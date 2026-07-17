#!/bin/bash
    set -e
    
    # TeamCity config parameter (can be empty / special value)
    TEST_NAME="%TEST_NAME%"
    
    echo "=== Copying TC appsettings ==="
    cp SimPitchSelenium/appsettings.tc.json SimPitchSelenium/appsettings.json
    
    # If TEST_NAME is empty or equals 'ALL' (case-insensitive), run the whole test project.
    # Otherwise, run only the selected test by name using --filter.
    if [ -z "$TEST_NAME" ] || [ "$TEST_NAME" = "ALL" ] || [ "$TEST_NAME" = "all" ]; then
        echo "=== Running Selenium Tests: ALL ==="
        dotnet test SimPitchSelenium/SimPitchSelenium.csproj \
        --configuration Release \
        --logger "trx;LogFileName=TestResults/TestResults.trx"
    else
        echo "=== Running Selenium Test: $TEST_NAME ==="
        dotnet test SimPitchSelenium/SimPitchSelenium.csproj \
        --configuration Release \
        --filter "Name=$TEST_NAME" \
        --logger "trx;LogFileName=TestResults/TestResults.trx"
    fi
    
    echo "✓ Test completed"
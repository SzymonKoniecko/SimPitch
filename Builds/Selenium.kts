package _Self.buildTypes

import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.XmlReport
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildFeatures.xmlReport
import jetbrains.buildServer.configs.kotlin.buildSteps.script

object RunTests : BuildType({
    name = "RunTests"

    artifactRules = """
        src/Selenium/SimPitchSelenium/bin/Release/net9.0/Reports/**/*
        src/Selenium/SimPitchSelenium/TestResults/**/*
    """.trimIndent()

    params {
        param("TEST_NAME", "ALL")
    }

    vcs {
        root(HttpsGithubComSzymonKonieckoSimPitchSeleniumGitRefsHeadsMain)
    }

    steps {
        script {
            name = "CheckingInstalling"
            id = "CheckingInstalling"
            scriptContent = """
                docker network inspect simpitch-tc_backend_network
                
                
                #!/bin/bash
                    set -e
                    
                    echo "=== Checking .NET versions ==="
                    dotnet --version
                    dotnet --list-sdks
                    
                    echo "=== Installing .NET 9.0 SDK ==="
                    
                    if dotnet --list-sdks | grep -q "9.0"; then
                        echo "✓ .NET 9.0 is already installed"
                        exit 0
                    fi
                    
                    echo "Downloading .NET 9.0 installer..."
                    wget -q https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
                    chmod +x dotnet-install.sh
                    
                    echo "Installing .NET 9.0..."
                    ./dotnet-install.sh --version 9.0 --install-dir /usr/local/dotnet
                    
                    export PATH="/usr/local/dotnet:${'$'}{'${'$'}'}PATH"
                    
                    echo "=== Verification ==="
                    /usr/local/dotnet/dotnet --version
                    /usr/local/dotnet/dotnet --list-sdks
                    
                    rm -f dotnet-install.sh
            """.trimIndent()
        }
        script {
            name = "RunTests"
            id = "RunTests"
            scriptContent = """
                #!/bin/bash
                    set -e
                    
                    # TeamCity config parameter (can be empty / special value)
                    TEST_NAME="%TEST_NAME%"
                    
                    echo "=== Copying TC appsettings ==="
                    cp SimPitchSelenium/appsettings.tc.json SimPitchSelenium/appsettings.json
                    
                    # If TEST_NAME is empty or equals 'ALL' (case-insensitive), run the whole test project.
                    # Otherwise, run only the selected test by name using --filter.
                    if [ -z "${'$'}TEST_NAME" ] || [ "${'$'}TEST_NAME" = "ALL" ] || [ "${'$'}TEST_NAME" = "all" ]; then
                        echo "=== Running Selenium Tests: ALL ==="
                        dotnet test SimPitchSelenium/SimPitchSelenium.csproj \
                        --configuration Release \
                        --logger "trx;LogFileName=TestResults/TestResults.trx" \
                        --logger "teamcity"
                    else
                        echo "=== Running Selenium Test: ${'$'}TEST_NAME ==="
                        dotnet test SimPitchSelenium/SimPitchSelenium.csproj \
                        --configuration Release \
                        --filter "Name=${'$'}TEST_NAME" \
                        --logger "trx;LogFileName=TestResults/TestResults.trx" \
                        --logger "teamcity"
                    fi
                    
                    echo "✓ Test completed"
            """.trimIndent()
        }
    }

    features {
        perfmon {
        }
        xmlReport {
            enabled = false
            reportType = XmlReport.XmlReportType.TRX
            rules = "+:**/TestResults.trx"
        }
    }
})

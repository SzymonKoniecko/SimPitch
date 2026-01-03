package _Self.buildTypes

import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.XmlReport
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildFeatures.xmlReport
import jetbrains.buildServer.configs.kotlin.buildSteps.nunitConsole
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.triggers.vcs

object Selenium : BuildType({
    name = "Selenium"

    artifactRules = """
        src/Selenium/SimPitchSelenium/bin/Release/net9.0/Reports/**/*
        src/Selenium/SimPitchSelenium/TestResults/**/*
    """.trimIndent()

    params {
        text("TEST_NAME", "PrepareSimulation_Should_Create_ALL_Simulation", label = "Choose the test value", display = ParameterDisplay.PROMPT, allowEmpty = true)
    }

    vcs {
        root(HttpsGithubComSzymonKonieckoSimPitchRefsHeadsMain)
    }
    steps {
        script {
            name = "Inspect network"
            id = "Inspect_network"
            scriptContent = "docker network inspect simpitch-tc_backend_network"
        }
        script {
            name = "Install 9.0 DOTNET"
            id = "Install_9_0_DOTNET"
            enabled = false
            scriptContent = """
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
            """.trimIndent()
        }
        script {
            name = "Check if app is running"
            id = "Check_if_app_is_running"
            scriptContent = """
                #!/bin/bash
                set -e
                
                PROJECT_NAME="simpitch-tc"
                RUNNING=${'$'}(docker ps --filter "label=com.docker.compose.project=${'$'}PROJECT_NAME" -q | wc -l)
                
                if [ "${'$'}RUNNING" -gt 0 ]; then
                  echo "✓ Docker Compose stack '${'$'}PROJECT_NAME' is running"
                  exit 0
                else
                  echo "✗ Stack not running. Start it first."
                  exit 1
                fi
            """.trimIndent()
        }
        script {
            name = "Run single test or all"
            id = "Run_test"
            scriptContent = """
                #!/bin/bash
                set -e
                
                # TeamCity config parameter (can be empty / special value)
                TEST_NAME="%TEST_NAME%"
                
                echo "=== Copying TC appsettings ==="
                cp src/Selenium/SimPitchSelenium/appsettings.tc.json src/Selenium/SimPitchSelenium/appsettings.json
                
                # If TEST_NAME is empty or equals 'ALL' (case-insensitive), run the whole test project.
                # Otherwise, run only the selected test by name using --filter.
                if [ -z "${'$'}TEST_NAME" ] || [ "${'$'}TEST_NAME" = "ALL" ] || [ "${'$'}TEST_NAME" = "all" ]; then
                  echo "=== Running Selenium Tests: ALL ==="
                  dotnet test src/Selenium/SimPitchSelenium/SimPitchSelenium.csproj \
                    --configuration Release \
                    --logger "trx;LogFileName=TestResults/TestResults.trx"
                else
                  echo "=== Running Selenium Test: ${'$'}{TEST_NAME} ==="
                  dotnet test src/Selenium/SimPitchSelenium/SimPitchSelenium.csproj \
                    --configuration Release \
                    --filter "Name=${'$'}{TEST_NAME}" \
                    --logger "trx;LogFileName=TestResults/TestResults.trx"
                fi
                
                echo "✓ Test completed"
            """.trimIndent()
        }
        nunitConsole {
            name = "Run NUnit Selenium Tests"
            id = "Run_NUnit_Selenium_Tests"
            enabled = false
            nunitPath = "%teamcity.tool.NUnit.Console.3.21.1%"
            includeTests = "**/SimPitchSelenium.dll"
        }
    }
    triggers {
        vcs {
            enabled = false
        }
    }

    features {
        perfmon {
        }
        xmlReport {
            reportType = XmlReport.XmlReportType.TRX
            rules = "+:**/TestResults.trx"
        }
    }
})

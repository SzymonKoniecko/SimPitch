package _Self.buildTypes

import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.triggers.vcs

object Selenium : BuildType({
    name = "Selenium"

    vcs {
        root(HttpsGithubComSzymonKonieckoSimPitchRefsHeadsMain)
    }

    steps {
        script {
            name = "Install 9.0 DOTNET"
            id = "Install_9_0_DOTNET"
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
            name = "Run test"
            id = "Run_test"
            scriptContent = """
                #!/bin/bash
                set -e
                
                echo "=== Copying TC appsettings ==="
                #cp tests/SimPitchSelenium/appsettings.tc.json tests/SimPitchSelenium/appsettings.json
                
                echo "=== Running Selenium Test: Nav_Should_Each_Element_Work==="
                dotnet test src/Selenium/SimPitchSelenium/SimPitchSelenium.csproj \
                  --configuration Release \
                  --filter "Name=Nav_Should_Each_Element_Work" \
                  --logger "trx;LogFileName=TestResults.trx"
                
                echo "✓ Test completed"
            """.trimIndent()
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
    }
})

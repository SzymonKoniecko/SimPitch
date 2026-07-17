package _Self.buildTypes

import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.triggers.vcs

object Build : BuildType({
    name = "Build"

    params {
        checkbox("env.IS_FORCE_UPDATE", "", display = ParameterDisplay.PROMPT,
                  checked = "true", unchecked = "false")
        text("env.PREDICTED_TESTS_API", "http://host.docker.internal:8888/api/v1/prediction/", display = ParameterDisplay.HIDDEN, allowEmpty = true)
        checkbox("env.PREDICTION_ALLOWED", "", display = ParameterDisplay.PROMPT,
                  checked = "true", unchecked = "false")
    }

    vcs {
        root(HttpsGithubComSzymonKonieckoSimPitchGitRefsHeadsStabilizeSeleniumTests)
    }
    steps {
        script {
            name = "cleanup"
            id = "simpleRunner"
            scriptContent = """
                #!/bin/bash
                # scripts/cleanup.sh - Czyści wszystkie SimPitch buildy i pliki
                
                set -e
                
                # ============================================
                # 2. Usuń pliki .env
                # ============================================
                echo ""
                echo "--- Removing .env files ---"
                
                if [ -f ".env" ]; then
                  echo "Removing .env"
                  rm -f .env
                  echo "✓ .env removed"
                fi
            """.trimIndent()
        }
        script {
            name = "build-project"
            id = "build_project"
            scriptContent = """
                #!/bin/bash
                # scripts/build-project.sh - Buduje wszystkie Docker images z tagami
                
                set -e
                
                BUILD_NUMBER=${'$'}{BUILD_NUMBER:-local}
                
                echo "=== Building SimPitch Docker Images ==="
                echo "Build Number: ${'$'}{BUILD_NUMBER}"
                echo "Current directory: ${'$'}(pwd)"
                
                # ============================================
                # 1. Generowanie .env
                # ============================================
                echo ""
                echo "--- Generating Environment Variables ---"
                
                if [ -f "scripts/generate-env.sh" ]; then
                  chmod +x scripts/generate-env.sh
                  ./scripts/generate-env.sh --seed true
                else
                  echo "WARNING: scripts/generate-env.sh not found"
                fi
                
                # ============================================
                # 2. Konfiguracja Docker BuildKit
                # ============================================
                echo ""
                echo "--- Docker Configuration ---"
                
                export DOCKER_BUILDKIT=0
                export COMPOSE_DOCKER_CLI_BUILD=0
                
                echo "✓ BuildKit disabled"
                
                # ============================================
                # 3. Lista obrazów do zbudowania
                # ============================================
                
                #!/bin/bash
                set -e
                
                echo "Building SimPitch images (APP)"
                
                docker compose -f docker-compose.yml -p simpitch-tc build
                
                echo "Images built:"
                docker images | grep simpitch
            """.trimIndent()
        }
        script {
            name = "Running APP stack"
            id = "Running_APP_stack"
            scriptContent = """
                #!/bin/bash
                set -e
                
                echo "Running APP stack"
                
                
                docker compose \
                  -f docker-compose.app.yml \
                  -p simpitch-tc \
                  up -d
                
                
                echo "Waiting for containers"
                sleep 20
                
                docker ps
            """.trimIndent()
        }
    }
    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
    }
})

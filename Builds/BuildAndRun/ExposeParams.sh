#!/bin/bash
set -euo pipefail

echo "=== Exposing Build&Run context as TeamCity parameters ==="

BUILD_ID="%teamcity.build.id%"
BRANCH="%teamcity.build.branch%"
REVISION="%build.vcs.number%"
BUILD_TYPE_ID="%system.teamcity.buildType.id%"
SERVER_URL="%teamcity.serverUrl%"

if [ -z "${BRANCH}" ]; then
  BRANCH="default"
fi

echo "Build ID      : ${BUILD_ID}"
echo "Build Type ID : ${BUILD_TYPE_ID}"
echo "Branch        : ${BRANCH}"
echo "Revision      : ${REVISION}"

echo "##teamcity[setParameter name='prediction.sourceBuildId' value='${BUILD_ID}']"
echo "##teamcity[setParameter name='prediction.sourceBuildTypeId' value='${BUILD_TYPE_ID}']"
echo "##teamcity[setParameter name='prediction.sourceBranch' value='${BRANCH}']"
echo "##teamcity[setParameter name='prediction.sourceRevision' value='${REVISION}']"
echo "##teamcity[setParameter name='prediction.sourceServerUrl' value='${SERVER_URL}']"

echo "=== Parameters exported ==="
pipeline {

    agent {

        docker {

            image 'mcr.microsoft.com/dotnet/sdk:8.0'

            args '-u root:root'

        }

    }

    options {

        timestamps()

        buildDiscarder(logRotator(numToKeepStr: '10'))

    }

    stages {

        stage('Restore') {

            steps { sh 'dotnet restore' }

        }

        stage('Lint') {

            steps { sh 'dotnet format --verify-no-changes' }

        }

        stage('Build') {

            steps { sh 'dotnet build --no-restore --configuration Release' }

        }

        stage('Test + Coverage') {

            steps {

                sh '''

                    dotnet test --no-build --configuration Release \

                      --collect:"XPlat Code Coverage" \

                      --results-directory ./coverage

                '''

            }

        }

        stage('Coverage Gate') {

            steps {

                sh '''

                    dotnet tool install -g dotnet-reportgenerator-globaltool

                    export PATH="$PATH:/root/.dotnet/tools"

                    reportgenerator \

                      -reports:"./coverage/**/coverage.cobertura.xml" \

                      -targetdir:"./coverage/report" \

                      -reporttypes:"TextSummary"

                    cat ./coverage/report/Summary.txt

                    LINE=$(grep -oP 'Line coverage:\\s*\\K[0-9.]+' ./coverage/report/Summary.txt)

                    THRESHOLD=70

                    awk "BEGIN{exit !($LINE >= $THRESHOLD)}" || { echo "Coverage $LINE% < $THRESHOLD%"; exit 1; }

                '''

            }

        }

        stage('Publish Artifact') {

            steps {

                sh 'dotnet publish Widget.Api/Widget.Api.csproj -c Release -o ./publish'

                archiveArtifacts artifacts: 'publish/**', fingerprint: true

            }

        }

    }

    post {

        always  { echo "Build #${env.BUILD_NUMBER} finished: ${currentBuild.currentResult}" }

        failure { echo 'Pipeline failed — check the stage logs above.' }

    }

}

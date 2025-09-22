pipeline {
    agent { label 'AppServerAgent' }

    environment {
        SOLUTION_PATH = 'C:\\JenkinsAgent\\workspace\\My First Jenkins Job'
        BUILD_CONFIGURATION = 'Release'
        DEPLOY_WEB_PATH = 'C:\\DeployedApps\\OrderWeb'
        DEPLOY_PROCESSOR_PATH = 'C:\\DeployedApps\\OrderProcessor'
        PROCESSOR_SERVICE = 'OrderProcessor'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'master', url: 'https://github.com/Cheekustar56/OrderProcessingSolution.git'
            }
        }

        stage('Stop OrderProcessor Service') {
            steps {
                bat """
                    sc stop ${PROCESSOR_SERVICE} || echo Service not running
                """
            }
        }

        stage('Build & Publish OrderWeb') {
            steps {
                bat "dotnet publish \"${SOLUTION_PATH}\\OrderWeb\\OrderWeb.csproj\" -c ${BUILD_CONFIGURATION} -o \"${DEPLOY_WEB_PATH}\""
            }
        }

        stage('Build & Publish OrderProcessor') {
            steps {
                bat "dotnet publish \"${SOLUTION_PATH}\\OrderProcessor\\OrderProcessor.csproj\" -c ${BUILD_CONFIGURATION} -o \"${DEPLOY_PROCESSOR_PATH}\""
            }
        }

        stage('Start OrderProcessor Service') {
            steps {
                bat "sc start ${PROCESSOR_SERVICE}"
            }
        }
    }

    post {
        success {
            echo 'Build and deployment completed successfully!'
        }
        failure {
            echo 'Build or deployment failed!'
        }
    }
}

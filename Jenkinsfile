pipeline {
    agent { label 'AppServerAgent' }
    
    environment {
        BUILD_CONFIGURATION = 'Release'
        DEPLOY_BASE = 'C:\\DeployedApps'  // Base deployment folder on agent
        PROCESSOR_SERVICE = 'OrderProcessor'
    }

    stages {
        stage('Checkout') {
            steps {
                // Checkout from GitHub
                git url: 'https://github.com/Cheekustar56/order-processing-solution.git', branch: 'master'
            }
        }

        stage('Build OrderWeb') {
            steps {
                // Build OrderWeb project
                bat "dotnet build \"%WORKSPACE%\\OrderWeb\\OrderWeb.csproj\" -c ${BUILD_CONFIGURATION}"
            }
        }

        stage('Build OrderProcessor') {
            steps {
                // Build OrderProcessor project
                bat "dotnet build \"%WORKSPACE%\\OrderProcessor\\OrderProcessor.csproj\" -c ${BUILD_CONFIGURATION}"
            }
        }

        stage('Publish OrderWeb') {
            steps {
                // Publish OrderWeb
                bat "dotnet publish \"%WORKSPACE%\\OrderWeb\\OrderWeb.csproj\" -c ${BUILD_CONFIGURATION} -o \"${DEPLOY_BASE}\\OrderWeb\""
            }
        }

        stage('Publish OrderProcessor') {
            steps {
                // Publish OrderProcessor
                bat "dotnet publish \"%WORKSPACE%\\OrderProcessor\\OrderProcessor.csproj\" -c ${BUILD_CONFIGURATION} -o \"${DEPLOY_BASE}\\OrderProcessor\""
            }
        }

        stage('Restart OrderProcessor Service') {
            steps {
                script {
                    // Stop service if it exists
                    bat """
                    sc stop ${PROCESSOR_SERVICE} || echo Service not running
                    """
                    // Start service
                    bat """
                    sc start ${PROCESSOR_SERVICE}
                    """
                }
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

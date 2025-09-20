pipeline {
    agent { label 'AppServerAgent' }

    environment {
        BUILD_CONFIGURATION = 'Release'
        DEPLOY_PATH = 'C:\\DeployedApps\\OrderWeb'
        PROCESSOR_SERVICE = 'OrderProcessor'
    }

    stages {
        stage('Checkout') {
            steps {
                echo "Cloning repository..."
                git url: 'https://github.com/Cheekustar56/OrderProcessingSolution.git', branch: 'master'
            }
        }

        stage('Build Solution') {
            steps {
                echo "Building solution..."
                bat "dotnet build \"%WORKSPACE%\\OrderProcessingSolution.sln\" -c ${env.BUILD_CONFIGURATION}"
            }
        }

        stage('Publish Web App') {
            steps {
                echo "Publishing OrderWeb..."
                bat """
                dotnet publish "%WORKSPACE%\\OrderWeb\\OrderWeb.csproj" -c ${env.BUILD_CONFIGURATION} -o ${env.DEPLOY_PATH}
                """
            }
        }

        stage('Restart Processor Service') {
            steps {
                echo "Restarting Windows service: ${env.PROCESSOR_SERVICE}"
                bat """
                sc stop ${env.PROCESSOR_SERVICE} || echo Service not running
                sc start ${env.PROCESSOR_SERVICE}
                """
            }
        }
    }

    post {
        success {
            echo "Build and deployment completed successfully!"
        }
        failure {
            echo "Build or deployment failed. Check the logs!"
        }
    }
}

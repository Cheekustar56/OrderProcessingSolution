pipeline {
    agent {
        label 'AppServerAgent' // Your Windows agent label
    }

    environment {
        SOLUTION_PATH = 'D:\\Tech Trainings\\Jenkins\\order_processing_solution'
        BUILD_CONFIGURATION = 'Release'
        DEPLOY_PATH = 'C:\\DeployedApps\\OrderWeb'
        PROCESSOR_SERVICE = 'OrderProcessor' // Name of your Windows service
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/Cheekustar56/OrderProcessingSolution.git'
            }
        }

        stage('Build') {
            steps {
                bat "dotnet build \"${env.SOLUTION_PATH}\\OrderProcessingSolution.sln\" -c ${env.BUILD_CONFIGURATION}"
            }
        }

        stage('Publish Web App') {
            steps {
                bat "dotnet publish \"${env.SOLUTION_PATH}\\OrderWeb\\OrderWeb.csproj\" -c ${env.BUILD_CONFIGURATION} -o \"${env.DEPLOY_PATH}\""
            }
        }

        stage('Restart Windows Service') {
            steps {
                // Stop the processor service if running
                bat """
                if exist \"${env.DEPLOY_PATH}\" (
                    net stop ${env.PROCESSOR_SERVICE} || echo Service not running
                    net start ${env.PROCESSOR_SERVICE}
                )
                """
            }
        }
    }

    post {
        success {
            echo 'Deployment completed successfully!'
        }
        failure {
            echo 'Deployment failed. Check the logs!'
        }
    }
}

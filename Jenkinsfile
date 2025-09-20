pipeline {
    agent { label 'AppServerAgent' }  // Your Windows agent

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

        stage('Build Solution') {
            steps {
                bat "dotnet build ${SOLUTION_PATH}\\OrderProcessingSolution.sln -c ${BUILD_CONFIGURATION}"
            }
        }

        stage('Publish Web App') {
            steps {
                bat """
                    dotnet publish ${SOLUTION_PATH}\\OrderWeb\\OrderWeb.csproj -c ${BUILD_CONFIGURATION} -o ${DEPLOY_PATH}
                """
            }
        }

        stage('Deploy & Restart Service') {
            steps {
                echo "Stopping OrderProcessor service..."
                bat "sc stop ${PROCESSOR_SERVICE} || echo Service not running"
                
                echo "Deploying new version..."
                // Optional: Clean deploy directory first
                bat "xcopy /E /Y ${DEPLOY_PATH}\\* ${DEPLOY_PATH}\\"

                echo "Starting OrderProcessor service..."
                bat "sc start ${PROCESSOR_SERVICE}"
            }
        }
    }

    post {
        success {
            echo 'Deployment completed successfully!'
        }
        failure {
            echo 'Deployment failed. Check the logs.'
        }
    }
}

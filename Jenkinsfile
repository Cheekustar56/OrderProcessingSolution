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

        stage('Restart OrderProcessor Service') {
    steps {
        script {
            def serviceName = "${PROCESSOR_SERVICE}"
            def timeoutSeconds = 120 // wait up to 2 minutes
            def interval = 5 // check every 5 seconds
            def waited = 0

            // Stop service if running
            bat "sc stop ${serviceName} || echo Service not running"

            // Start service
            bat "sc start ${serviceName}"

            // Wait until service status is RUNNING or timeout reached
            while (waited < timeoutSeconds) {
                def status = bat(returnStdout: true, script: "sc query ${serviceName} | findstr STATE").trim()
                if (status.contains("RUNNING")) {
                    echo "${serviceName} is now running."
                    break
                } else {
                    echo "Waiting for ${serviceName} to start..."
                    sleep(interval)
                    waited += interval
                }
            }

            if (waited >= timeoutSeconds) {
                error "Timeout reached while waiting for ${serviceName} to start."
				}
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

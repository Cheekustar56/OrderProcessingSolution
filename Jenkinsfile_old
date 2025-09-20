pipeline {
    agent none
    stages {
        stage('Build') {
            agent { label 'master' }
            steps {
                bat 'dotnet restore'
                bat 'dotnet build OrderProcessingSolution.sln -c Release'
                bat 'dotnet publish OrderWeb -c Release -o C:\Jenkins\artifacts\OrderWeb'
                bat 'dotnet publish OrderProcessor -c Release -o C:\Jenkins\artifacts\OrderProcessor'
            }
        }
        stage('Deploy') {
            agent { label 'appserver' }
            steps {
                bat '''
                xcopy C:\Jenkins\artifacts\OrderWeb C:\inetpub\wwwroot\OrderWeb /E /I /Y
                powershell Restart-WebAppPool -Name "DefaultAppPool"
                xcopy C:\Jenkins\artifacts\OrderProcessor C:\Services\OrderProcessor /E /I /Y
                nssm restart OrderProcessorService
                '''
            }
        }
    }
}

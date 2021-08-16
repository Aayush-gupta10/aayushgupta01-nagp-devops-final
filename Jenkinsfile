pipeline{
    agent any
    environment{
        scannerHome = tool name: 'sonar_scanner_dotnet'
        username = 'aayushgupta01'
        registry = 'aayushgup10/'
    }
    stages{
        stage('Checkout')
        {
            git url = "https://github.com/Aayush-gupta10/aayushgupta01-nagp-devops-final.git"
        }
        stage('nuget restore')
        {
            bat "dotnet restore"
        }
        stage('Start Sonar Qube Analysis')
        {
            withSonarQubeEnv('Test_Sonar')
            {
                bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:aayushgupta01 -d:sonar.cs.opencover.reportsPaths=XUnitTestProject1/coverage.opencover.xml -d:sonar.cs.xunit.reportPaths=XUnitTestProject1/TestResults/testresult.xml"
            }
        }
        stage('Build')
        {
            steps{
                echo "Code build"
                bat "dotnet clean"
                bat "dotnet build -c Release -o DevopsWebApp/app/build"
            }
        }
        stage('Test Cases')
        {
            steps{
                bat "dotnet build test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover -l:trx;LogFileName=testresult.xml"
            }
        }
        stage('Stop Sonar Analysis')
        {
            withSonarQubeEnv('Test_Sonar')
            {
                bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
            }
        }
        stage('Build Docker')
        {
            steps{
                bat "docker build -t i-${username}-$env.BRANCH_NAME ."
                bat "docker tag i-${username}-$env.BRANCH_NAME ${registry}$env.BRANCH_NAME:$BUILD_NUMBER"
                bat "docker tag i-${username}-$env.BRANCH_NAME ${registry}$env.BRANCH_NAME:$latest"
            }
        }
        stage('Push Docker Hub')
        {
            steps{
                withDockerRegistry(credentialsID:'DockerHub',url:'')
                {
                    bat "docker push ${registry}$env.BRANCH_NAME:$BUILD_NUMBER"
                    bat "docker push ${registry}$env.BRANCH_NAME:$latest"
                }
            }
        }
        stage('Delete Contianer if running')
        {
            script{
                env.containerId = bat(script:"docker ps -a -f publish 7400 -q", returnStdout=true).trim().readlines().drop(1).join()
                if(env.containerId == '')
                {
                    echo "No conatiner is running"
                }
                else{
                    bat "docker stop ${enev.containerId}"
                    bat "docker rm ${enev.containerId}"
                }
            }
        }
        stage('Deployment')
        {
            steps{
                parallel(
                    "Docker Deployment":{
                        bat "docker run --name c-${username}-$env.BRANCH_NAME -d -p 7400:80 ${registry}$env.BRANCH_NAME:$latest"
                    },
                    "kubernetes deployment":
                    {
                        bat "kubectl apply -f config.yaml --namespace=aayushgupta01"
                        bat "kubectl apply -f deployment.yaml --namespace=aayushgupta01"
                    }
                )
            }
        }
    }
}
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
            steps{
            git url: "https://github.com/Aayush-gupta10/aayushgupta01-nagp-devops-final.git"
            }
        }
        stage('nuget restore')
        {
            steps{
            bat "dotnet restore"
            }
        }
        stage('Start Sonar Qube Analysis')
        {
            steps{
                withSonarQubeEnv('Test_Sonar')
                {
                    bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:aayushgupta01 -d:sonar.cs.opencover.reportsPaths=XUnitTestProject1/coverage.opencover.xml -d:sonar.cs.xunit.reportPaths=XUnitTestProject1/TestResults/testresult.xml"
                }
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
                bat "dotnet test XUnitTestProject1 /p:CollectCoverage=true /p:CoverletOutputFormat=opencover -l:trx;LogFileName=testresult.xml"
            }
        }
        stage('Stop Sonar Analysis')
        {
            steps{
                withSonarQubeEnv('Test_Sonar')
                {
                    bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                }
            }
        }
        stage('Build Docker')
        {
            steps{
                bat "docker build -t i-${username}-feature ."
                bat "docker tag i-${username}-feature feature:latest"
            }
        }
        stage('Push Docker Hub')
        {
            steps{
                withDockerRegistry(credentialsId:'DockerHub',url:'')
                {
                    bat "docker push ${registry}feature:latest"
                }
            }
        }
        stage('Delete Contianer if running')
        {
            steps{
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
        }
        stage('Deployment')
        {
            steps{
                parallel(
                    "Docker Deployment":{
                        bat "docker run --name c-${username}-feature -d -p 7400:80 feature:latest"
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

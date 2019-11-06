$ErrorActionPreference = 'Stop'

Write-Host "Updating variables..."

$instanceId = Get-EC2InstanceMetadata -Category InstanceId
$instanceTags = Get-EC2Tag -Region "us-east-1" -Filter @{ Name="resource-id"; Values=$instanceId } 
$application = $instanceTags.Where({$_.Key -eq "application"}).Value
$environment = $instanceTags.Where({$_.Key -eq "environment"}).Value

$parameterPath = "/$application/$environment/"

Set-Location -Path "C:\Website"

$currentFolder = $PWD

$configPath = "$currentFolder\Web.config_master"
$configReplacedPath = "$currentFolder\Web.replaced.config_master"
$configContents = Get-Content $configPath

$parameters = Get-SSMParametersByPath -Path $parameterPath -Region us-east-1 | 
	ForEach-Object {
		Write-Host $_.Name
		Write-Host $_.Value
		
		$variableName = "%{" + ($_.Name -Replace $parameterPath, "") + "}"
		Write-Host $variableName
		Write-Host
		
		$configContents = $configContents -replace $variableName, $_.Value
	}
	
Set-Content -Path $configReplacedPath -Value $configContents	

Write-Host "Script finished"
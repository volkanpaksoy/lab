. .\"00. configuration.ps1"

# Create AMI
$imageId = New-EC2Image -InstanceId $instanceId -Name $amiName -Description $amiDescription -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion

Set-Variable -Scope global -Name AMI_ID -Value $imageId
Write-Host "AMI_ID: [" $AMI_ID "]"



. .\"00. configuration.ps1"

$imageId = Get-Variable AMI_ID -valueOnly
Copy-EC2Image -SourceImageId $imageId -SourceRegion $sourceRegion -Name $amiName -ProfileName $targetAccountAwsProfileName -Region $targetRegion


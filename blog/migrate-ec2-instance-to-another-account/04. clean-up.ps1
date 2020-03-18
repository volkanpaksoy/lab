. .\"00. configuration.ps1"

# Create image script writes the AMI ID to a variable. If it doesn't exist get the image id from AWS Management Console
$imageId = Get-Variable AMI_ID -valueOnly

Write-Host "Unregistering image: [" $imageId "]"
Unregister-EC2Image -ImageId $imageId -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion

$imageSnapshots = Get-EC2Snapshot -OwnerId $sourceAccountId -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion
                | Where-Object {$_.Description -like "*$imageId*" }

foreach ($snapshot in $imageSnapshots) {
    Write-Host "Removing snapshot: [" $snapshot.SnapshotId "]"
    Remove-EC2Snapshot -SnapshotId $snapshot.SnapshotId -Force -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion
}

# Delete variable
Remove-Variable AMI_ID -Scope global

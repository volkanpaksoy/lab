. .\"00. configuration.ps1"

$imageId = Get-Variable AMI_ID -valueOnly
Edit-EC2ImageAttribute -ImageId $imageId -Attribute launchPermission -OperationType add -UserId $targetAccountId -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion

$imageSnapshots = Get-EC2Snapshot -OwnerId $sourceAccountId -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion
                | Where-Object {$_.Description -like "*$imageId*" }

foreach ($snapshot in $imageSnapshots) {
    Edit-EC2SnapshotAttribute -SnapshotId $snapshot.SnapshotId -Attribute createVolumePermission -OperationType add -UserId $targetAccountId -ProfileName $sourceAccountAwsProfileName -Region $sourceRegion
}

param($LabInstanceId)
$testURL = "https://lods${LabInstanceId}.azurewebsites.net/evaluate/devicecount"

$LabInstanceId = "7191819"

$testURL = "http://lods$($LabInstanceId).azurewebsites.net/evaluate/createsqltable"



$payload = @{
    sqlConnection="-1";
    storageConnection="-1"
}

$result = Invoke-RestMethod -Method "Post" -Uri $testURL -Body $payload
$result.Message
$result.Passed
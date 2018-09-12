$LabInstanceId = "7223858"

param($LabInstanceId)

$id = $($LabInstanceId)
$rg = "Archlod$($id)"
$saName = "sa$($id)"

$sa = Get-AzureRmStorageAccount -ResourceGroupName $rg -Name $saName -ErrorAction Ignore
$key = (Get-AzureRmStorageAccountKey -ResourceGroupName $rg -Name $saName -ErrorAction Ignore)[0].Value
$context = New-AzureStorageContext -StorageAccountName $saName -StorageAccountKey $key

$container = Get-AzureStorageContainer -Name `$logs -Context $context -ErrorAction Ignore
$container -ne $null

param($LabInstanceId)

$id = $($LabInstanceId)
$rg = "Archlod$($id)"
$waName = "lods$($id)"
$wa = Get-AzureRmWebApp -ResourceGroupName $rg -Name $waName

$result=$false

foreach($cs in $wa.SiteConfig.ConnectionStrings) {
    if($cs.Name -eq "storageConnection") {
        $result = $true
    }
}
return $result

$LabInstanceId = "7202203"

param($LabInstanceId)

$id = $($LabInstanceId)
$rg = "Archlod$($id)"
$laName = "logic$($id)"

$la = Get-AzureRmLogicApp -ResourceGroupName $rg -Name $laName -ErrorAction Ignore

$result = $false
#This is super clugey and will fail if MS changes the structure of a logic app at all.
if($la -ne $null) {
    foreach ($def in $la.Definition) {
        foreach($lvl2 in $def) {
            if($lvl2.Type -ne "String") {
                foreach($lvl3 in $lvl2) {
                    if($lvl3.Name -eq "Insert_Row") {
                        $result = $true
                    }
                }
            }
        }
    }
}

$result
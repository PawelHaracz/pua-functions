# Input bindings are passed in via param block.
# https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference-powershell?tabs=portal
param([byte[]] $InputBlob, $TriggerMetadata)

# Write out the blob name and size to the information log.
Write-Host "PowerShell Blob trigger function Processed blob! Name: $($TriggerMetadata.Name) Size: $($InputBlob.Length) bytes"

$csv = [System.Text.Encoding]::UTF8.GetString($InputBlob)  |  ConvertFrom-Csv -Delimiter ','  #Convert bytes to CSV
$json = ConvertTo-Json -InputObject $csv  # Convert csv to bytes

$json | Foreach-Object { # enumerate row by row as parrarell
    Push-OutputBinding -Name Msg -Value $_ # push value to account storage queue
}
Write-Host "Done"
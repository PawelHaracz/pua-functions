# Input bindings are passed in via param block.
param($QueueItem, $TriggerMetadata)

# Write out the queue message and insertion time to the information log.

Write-Host "Queue item insertion time: $($TriggerMetadata.InsertionTime)"

$QueueItem | ForEach-Object {
    $item = @{
        id = $_.id
        email = $_.email
        gender = $_.gender
        departament = $_.departament
        company = $_.company
        firstName = $_.first_name
        lastName = $_.last_name
        jobTitle = $_.job_title
        partitionKey = $_.company
    }
    Write-Host $item['id', 'email']
    Push-OutputBinding -Name EmployeeDocument -Value $item 
} 
Write-Host "Done"
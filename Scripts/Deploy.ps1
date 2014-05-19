param ([string]$deployfrom,
[string]$deployToDir)

function Get-DateString
{
    return (get-Date).ToString("yyyyMMdd_hhmmss")
}

function Backup-OldFile([string]$backupDir)
{
    Write-host "Making backup of dir $backupDir"      
    $parent =[System.IO.Path]::GetDirectoryName( $backupDir)

    $dirName = [System.IO.Path]::GetFileName($backupDir)
    
    $backupTo = join-path $parent -ChildPath ($dirName+"_"+(Get-DateString))

    Copy-Item "$backupDir\*" -Destination $backupTo  -Container -Recurse 

}

function Deploy-Artifacts([string]$artifactDir, [string]$deployTo  )
{
    Write-Host "Copy artefacts $artifactDir to $deployTo"
    $copyFrom = join-path $artifactDir "*"
    Copy-Item $copyFrom -Destination $deployTo  -Container -Recurse 
}

function Clean-Dir ([string]$dir)
{
    Write-Host "Cleaning $dir"
    $removeDir = Join-Path $dir "*"
    Remove-Item $removeDir -Recurse
}

if(-not([string]::IsNullOrEmpty($deployfrom)) -and -not([string]::IsNullOrEmpty($deployToDir) ))
{
    Backup-OldFile $deployToDir
    Clean-Dir -dir $deployToDir
    Deploy-Artifacts -artifactDir $deployfrom -deployTo $deployToDir
}
else
{
    Write-Host "One of parameters is null or empty. "
    write-host "deployfrom: $deployfrom "
    write-host "deployToDir: $deployToDir "
}
# Documentation Organization Script
# Moves helper/development docs to docs/ folder with UTF-8 encoding

Write-Host "Moving documentation files to docs/ folder..." -ForegroundColor Cyan

# Files to move (development/helper docs)
$filesToMove = @(
    "PRE_PUBLISH_CHECKLIST.md",
    "SETUP_COMPLETE.md",
    "WORKFLOWS_GUIDE.md",
    "ENHANCEMENTS_SUMMARY.md",
    "QUICK_REFERENCE.md",
    "Orionde.MoreLookup\ICON_GUIDE.md"
)

# Files to keep in root (GitHub standard docs)
$keepInRoot = @(
    "README.md",
    "LICENSE.txt",
    "CHANGELOG.md",
    "CONTRIBUTING.md",
    "CODE_OF_CONDUCT.md",
    "SECURITY.md"
)

foreach ($file in $filesToMove) {
    if (Test-Path $file) {
        $fileName = Split-Path $file -Leaf
        $destPath = "docs\$fileName"
        
        Write-Host "  Moving: $file -> $destPath" -ForegroundColor Gray
        
        # Read content and write with UTF-8 encoding
        $content = Get-Content $file -Raw -Encoding UTF8
        Set-Content -Path $destPath -Value $content -Encoding UTF8
        
        # Remove original
        Remove-Item $file
        
        Write-Host "    ? Moved and converted to UTF-8" -ForegroundColor Green
    }
    else {
        Write-Host "  ? File not found: $file" -ForegroundColor Yellow
    }
}

Write-Host "`nDone! Documentation organized." -ForegroundColor Green
Write-Host "`nFiles in root (for GitHub):" -ForegroundColor Cyan
foreach ($file in $keepInRoot) {
    if (Test-Path $file) {
        Write-Host "  ? $file" -ForegroundColor Gray
    }
}

Write-Host "`nFiles in docs/ (development/helper):" -ForegroundColor Cyan
Get-ChildItem "docs\" -Filter "*.md" | ForEach-Object {
    Write-Host "  ? $($_.Name)" -ForegroundColor Gray
}

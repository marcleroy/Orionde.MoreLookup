# Convert Root Docs to UTF-8
$files = @(
    "README.md",
    "CHANGELOG.md", 
    "CONTRIBUTING.md",
    "CODE_OF_CONDUCT.md",
    "SECURITY.md"
)

foreach ($file in $files) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw -Encoding UTF8
        Set-Content -Path $file -Value $content -Encoding UTF8
        Write-Host "? Converted $file to UTF-8" -ForegroundColor Green
    }
}

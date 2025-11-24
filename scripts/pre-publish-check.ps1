# Pre-Publish Verification Script
# This script runs all the checks before publishing to NuGet

param(
    [switch]$SkipTests,
    [switch]$SkipBuild
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Orionde.MoreLookup Pre-Publish Checker" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$ErrorCount = 0
$WarningCount = 0

function Test-Condition {
    param(
        [string]$Name,
        [scriptblock]$Check,
        [string]$FailureMessage,
        [bool]$IsWarning = $false
    )
    
    Write-Host "Checking: $Name..." -NoNewline
    
    try {
        $result = & $Check
        if ($result) {
            Write-Host " ?" -ForegroundColor Green
            return $true
        }
        else {
            if ($IsWarning) {
                Write-Host " ? WARNING" -ForegroundColor Yellow
                Write-Host "  $FailureMessage" -ForegroundColor Yellow
                $script:WarningCount++
            }
            else {
                Write-Host " ? FAILED" -ForegroundColor Red
                Write-Host "  $FailureMessage" -ForegroundColor Red
                $script:ErrorCount++
            }
            return $false
        }
    }
    catch {
        Write-Host " ? ERROR" -ForegroundColor Red
        Write-Host "  $($_.Exception.Message)" -ForegroundColor Red
        $script:ErrorCount++
        return $false
    }
}

# Check 1: Solution file exists
Test-Condition -Name "Solution file" -Check {
    Test-Path "*.sln"
} -FailureMessage "No solution file found in current directory"

# Check 2: Project file exists
Test-Condition -Name "Project file" -Check {
    Test-Path "Orionde.MoreLookup\Orionde.MoreLookup.csproj"
} -FailureMessage "Project file not found"

# Check 3: README exists
Test-Condition -Name "README.md" -Check {
    Test-Path "Orionde.MoreLookup\README.md"
} -FailureMessage "README.md not found"

# Check 4: LICENSE exists
Test-Condition -Name "LICENSE.txt" -Check {
    Test-Path "Orionde.MoreLookup\LICENSE.txt"
} -FailureMessage "LICENSE.txt not found"

# Check 5: CHANGELOG exists
Test-Condition -Name "CHANGELOG.md" -Check {
    Test-Path "CHANGELOG.md"
} -FailureMessage "CHANGELOG.md not found" -IsWarning $true

# Check 6: Package icon exists (warning only)
Test-Condition -Name "Package icon" -Check {
    Test-Path "Orionde.MoreLookup\icon.png"
} -FailureMessage "icon.png not found - package will be published without icon" -IsWarning $true

# Check 7: .editorconfig exists
Test-Condition -Name ".editorconfig" -Check {
    Test-Path ".editorconfig"
} -FailureMessage ".editorconfig not found" -IsWarning $true

# Check 8: GitHub workflows exist
Test-Condition -Name "CI workflow" -Check {
    Test-Path ".github\workflows\ci.yml"
} -FailureMessage "CI workflow not configured" -IsWarning $true

Test-Condition -Name "Publish workflow" -Check {
    Test-Path ".github\workflows\publish-nuget.yml"
} -FailureMessage "Publish workflow not configured"

# Check 9: Project metadata
Write-Host "`nVerifying project metadata..." -ForegroundColor Cyan
$csproj = [xml](Get-Content "Orionde.MoreLookup\Orionde.MoreLookup.csproj")
$props = $csproj.Project.PropertyGroup

$version = $props.Version
Write-Host "  Version: $version" -ForegroundColor Gray

if ([string]::IsNullOrWhiteSpace($props.PackageReadmeFile)) {
    Write-Host "  ? PackageReadmeFile not set" -ForegroundColor Yellow
    $WarningCount++
}

if ([string]::IsNullOrWhiteSpace($props.PackageIcon)) {
    Write-Host "  ? PackageIcon not set" -ForegroundColor Yellow
    $WarningCount++
}

if ([string]::IsNullOrWhiteSpace($props.Description)) {
    Write-Host "  ? Description is required" -ForegroundColor Red
    $ErrorCount++
}

if ([string]::IsNullOrWhiteSpace($props.Authors)) {
    Write-Host "  ? Authors is required" -ForegroundColor Red
    $ErrorCount++
}

# Check 10: Clean build
if (-not $SkipBuild) {
    Write-Host "`nRunning clean build..." -ForegroundColor Cyan
    
    dotnet clean --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ? Clean failed" -ForegroundColor Red
        $ErrorCount++
    }
    else {
        Write-Host "  ? Clean successful" -ForegroundColor Green
    }
    
    dotnet restore --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ? Restore failed" -ForegroundColor Red
        $ErrorCount++
    }
    else {
        Write-Host "  ? Restore successful" -ForegroundColor Green
    }
    
    dotnet build --configuration Release --no-restore --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ? Build failed" -ForegroundColor Red
        $ErrorCount++
    }
    else {
        Write-Host "  ? Build successful" -ForegroundColor Green
    }
}

# Check 11: Tests
if (-not $SkipTests) {
    Write-Host "`nRunning tests..." -ForegroundColor Cyan
    
    dotnet test --configuration Release --no-build --verbosity quiet --nologo
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ? Tests failed" -ForegroundColor Red
        $ErrorCount++
    }
    else {
        Write-Host "  ? All tests passed" -ForegroundColor Green
    }
}

# Check 12: Pack test
Write-Host "`nTesting package creation..." -ForegroundColor Cyan
$packOutput = dotnet pack Orionde.MoreLookup\Orionde.MoreLookup.csproj --configuration Release --no-build --output .\test-nupkg --verbosity quiet 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ? Package created successfully" -ForegroundColor Green
    
    # Check if .nupkg exists
    $nupkg = Get-ChildItem -Path "test-nupkg\*.nupkg" | Select-Object -First 1
    if ($nupkg) {
        $sizeKB = [math]::Round($nupkg.Length / 1KB, 2)
        Write-Host "  Package size: $sizeKB KB" -ForegroundColor Gray
        Write-Host "  Package name: $($nupkg.Name)" -ForegroundColor Gray
    }
    
    # Cleanup
    Remove-Item -Path "test-nupkg" -Recurse -Force -ErrorAction SilentlyContinue
}
else {
    Write-Host "  ? Package creation failed" -ForegroundColor Red
    Write-Host "  $packOutput" -ForegroundColor Red
    $ErrorCount++
}

# Summary
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($ErrorCount -eq 0 -and $WarningCount -eq 0) {
    Write-Host "? All checks passed! Ready to publish." -ForegroundColor Green
    Write-Host "`nNext steps:" -ForegroundColor Cyan
    Write-Host "  1. Update version in .csproj if needed" -ForegroundColor Gray
    Write-Host "  2. Update CHANGELOG.md with release date" -ForegroundColor Gray
    Write-Host "  3. Commit and push changes" -ForegroundColor Gray
    Write-Host "  4. Create and push version tag: git tag v$version && git push origin v$version" -ForegroundColor Gray
    exit 0
}
elseif ($ErrorCount -eq 0) {
    Write-Host "? $WarningCount warning(s) found, but can proceed." -ForegroundColor Yellow
    Write-Host "Consider addressing warnings for a more professional package." -ForegroundColor Yellow
    exit 0
}
else {
    Write-Host "? $ErrorCount error(s) and $WarningCount warning(s) found." -ForegroundColor Red
    Write-Host "Please fix errors before publishing." -ForegroundColor Red
    exit 1
}

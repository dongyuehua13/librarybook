$baseUrl = "http://localhost:5002"
$pass = 0
$fail = 0

function Check-Url {
    param($Url, $Name)
    try {
        $resp = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 10
        if ($resp.StatusCode -eq 200) {
            Write-Host "[PASS] $Name ($Url)" -ForegroundColor Green
            return $true
        } else {
            Write-Host "[FAIL] $Name ($Url) => $($resp.StatusCode)" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "[FAIL] $Name ($Url) => $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

Write-Host "===== SMOKE TEST =====" -ForegroundColor Cyan

Write-Host "--- User Endpoints ---" -ForegroundColor Yellow
if (Check-Url "$baseUrl/" "Homepage") { $pass++ } else { $fail++ }
if (Check-Url "$baseUrl/Home/Seats" "Seats List") { $pass++ } else { $fail++ }
if (Check-Url "$baseUrl/Home/Detail/1" "Seat Detail") { $pass++ } else { $fail++ }

Write-Host "--- Admin Endpoints ---" -ForegroundColor Yellow
if (Check-Url "$baseUrl/Admin/Login" "Admin Login") { $pass++ } else { $fail++ }

Write-Host "=== Result ===" -ForegroundColor Cyan
Write-Host "Passed: $pass / Failed: $fail"
if ($fail -eq 0) {
    Write-Host "Conclusion: ALL SMOKE TESTS PASSED" -ForegroundColor Green
} else {
    Write-Host "Conclusion: SMOKE TESTS FAILED" -ForegroundColor Red
}

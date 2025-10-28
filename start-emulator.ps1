# Script para iniciar emulador Android
param(
    [string]$EmulatorName
)

$androidHome = $env:ANDROID_HOME
if (-not $androidHome) {
    $androidHome = "$env:LOCALAPPDATA\Android\Sdk"
}

$emulatorPath = "$androidHome\emulator\emulator.exe"

if (-not (Test-Path $emulatorPath)) {
    Write-Host "Erro: Emulador não encontrado!" -ForegroundColor Red
    Write-Host "Execute primeiro: .\setup-android.ps1" -ForegroundColor Yellow
    exit
}

# Listar emuladores se nenhum foi especificado
if (-not $EmulatorName) {
    Write-Host "Emuladores disponíveis:" -ForegroundColor Cyan
    & $emulatorPath -list-avds
    Write-Host ""
    Write-Host "Use: .\start-emulator.ps1 <nome-do-emulador>" -ForegroundColor Yellow
    exit
}

# Iniciar emulador
Write-Host "Iniciando emulador: $EmulatorName" -ForegroundColor Green
Start-Process -FilePath $emulatorPath -ArgumentList "-avd", $EmulatorName -WindowStyle Hidden
Write-Host "Emulador iniciado! Aguarde alguns segundos para ele abrir..." -ForegroundColor Green

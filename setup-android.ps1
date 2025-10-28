# Script para configurar Android no VS Code

Write-Host "=== CONFIGURAÇÃO ANDROID PARA VS CODE ===" -ForegroundColor Cyan
Write-Host ""

# Verificar se Android SDK está instalado
Write-Host "Verificando Android SDK..." -ForegroundColor Yellow
$androidHome = $env:ANDROID_HOME
if (-not $androidHome) {
    $androidHome = "$env:LOCALAPPDATA\Android\Sdk"
    if (Test-Path $androidHome) {
        Write-Host "✓ Android SDK encontrado em: $androidHome" -ForegroundColor Green
    } else {
        Write-Host "✗ Android SDK não encontrado!" -ForegroundColor Red
        Write-Host ""
        Write-Host "PASSOS PARA INSTALAR:" -ForegroundColor Yellow
        Write-Host "1. Baixe o Android Studio: https://developer.android.com/studio"
        Write-Host "2. Instale o Android Studio"
        Write-Host "3. Abra o Android Studio e siga o setup inicial"
        Write-Host "4. Execute este script novamente"
        exit
    }
} else {
    Write-Host "✓ Android SDK encontrado: $androidHome" -ForegroundColor Green
}

# Configurar variável de ambiente se não existir
if (-not $env:ANDROID_HOME) {
    Write-Host ""
    Write-Host "Configurando ANDROID_HOME..." -ForegroundColor Yellow
    [System.Environment]::SetEnvironmentVariable("ANDROID_HOME", $androidHome, "User")
    $env:ANDROID_HOME = $androidHome
    Write-Host "✓ ANDROID_HOME configurado!" -ForegroundColor Green
}

# Listar emuladores disponíveis
Write-Host ""
Write-Host "Listando emuladores disponíveis..." -ForegroundColor Yellow
$emulatorPath = "$androidHome\emulator\emulator.exe"
if (Test-Path $emulatorPath) {
    $avds = & $emulatorPath -list-avds 2>$null
    if ($avds) {
        Write-Host "✓ Emuladores encontrados:" -ForegroundColor Green
        $avds | ForEach-Object { Write-Host "  - $_" -ForegroundColor Cyan }
        Write-Host ""
        Write-Host "Para iniciar um emulador, use:" -ForegroundColor Yellow
        Write-Host "  .\start-emulator.ps1 <nome-do-emulador>" -ForegroundColor White
    } else {
        Write-Host "✗ Nenhum emulador criado ainda!" -ForegroundColor Red
        Write-Host ""
        Write-Host "CRIAR EMULADOR:" -ForegroundColor Yellow
        Write-Host "1. Abra o Android Studio"
        Write-Host "2. Vá em: Tools > Device Manager"
        Write-Host "3. Clique em 'Create Device'"
        Write-Host "4. Escolha 'Pixel 5' ou outro modelo"
        Write-Host "5. Selecione 'Android 13.0 (API 33)' ou superior"
        Write-Host "6. Finalize a criação"
    }
} else {
    Write-Host "✗ Emulador não encontrado em: $emulatorPath" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== PRÓXIMOS PASSOS ===" -ForegroundColor Cyan
Write-Host "1. Certifique-se de ter um emulador criado no Android Studio"
Write-Host "2. Use 'dotnet build -t:Run -f net9.0-android' para rodar no VS Code"
Write-Host "3. Ou abra o Android Studio Device Manager e inicie o emulador manualmente"
Write-Host ""

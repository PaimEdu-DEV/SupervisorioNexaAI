param(
    [int]$ApiPort = 5081,
    [int]$AiPort = 8001,
    [switch]$ForceRestart,
    [switch]$UseOllamaFallback,
    [int]$OllamaPort = 11434
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$aiDir = Join-Path $root "ai-diagnostic-service"
$venvPython = Join-Path $aiDir ".venv\Scripts\python.exe"
$logsDir = Join-Path $root ".runlogs"

New-Item -ItemType Directory -Force $logsDir | Out-Null

function Test-PortListening {
    param([int]$Port)
    return [bool](Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue)
}

function Stop-PortProcess {
    param([int]$Port)

    $connections = Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue
    foreach ($connection in $connections) {
        $processId = $connection.OwningProcess
        if ($processId -and $processId -ne $PID) {
            Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
        }
    }
}

function Start-Detached {
    param(
        [string]$FilePath,
        [string[]]$Arguments,
        [string]$WorkingDirectory,
        [string]$LogFile
    )

    $errorLog = [System.IO.Path]::ChangeExtension($LogFile, ".err.log")
    $process = Start-Process -FilePath $FilePath `
        -ArgumentList $Arguments `
        -WorkingDirectory $WorkingDirectory `
        -WindowStyle Hidden `
        -RedirectStandardOutput $LogFile `
        -RedirectStandardError $errorLog `
        -PassThru

    return $process.Id
}

Write-Host ""
Write-Host "=== Supervisorio Nexa AI ===" -ForegroundColor Cyan

if ([string]::IsNullOrWhiteSpace($env:MODEL_NAME)) {
    $env:MODEL_NAME = "qwen3:8b"
}

if ([string]::IsNullOrWhiteSpace($env:OLLAMA_MODEL)) {
    $env:OLLAMA_MODEL = $env:MODEL_NAME
}

if ($UseOllamaFallback -or $env:AI_PROVIDER -eq "ollama" -or [string]::IsNullOrWhiteSpace($env:AI_PROVIDER)) {
    $env:AI_PROVIDER = "ollama"
    if (-not (Test-PortListening $OllamaPort)) {
        Write-Host "Subindo Ollama na porta $OllamaPort..."
        Start-Process -FilePath "ollama" -ArgumentList "serve" -WindowStyle Hidden | Out-Null
        Start-Sleep -Seconds 3
    } else {
        Write-Host "Ollama ja esta rodando na porta $OllamaPort." -ForegroundColor Green
    }

    Write-Host "Garantindo modelo Ollama $env:OLLAMA_MODEL..."
    & ollama pull $env:OLLAMA_MODEL
} else {
    $env:AI_PROVIDER = "openai"
    if ([string]::IsNullOrWhiteSpace($env:OPENAI_API_KEY)) {
        Write-Host "OPENAI_API_KEY nao esta configurada neste terminal." -ForegroundColor Yellow
        Write-Host 'Antes de usar OpenAI, rode: $env:OPENAI_API_KEY="sua-chave-aqui"' -ForegroundColor Yellow
    } else {
        Write-Host "OpenAI configurada com MODEL_NAME=$env:MODEL_NAME." -ForegroundColor Green
    }
}

if ($ForceRestart) {
    Write-Host "Reiniciando servicos para aplicar provider/modelo/chave..." -ForegroundColor Cyan
    Stop-PortProcess $AiPort
    Stop-PortProcess $ApiPort
    Start-Sleep -Seconds 2
}

if (-not (Test-Path $venvPython)) {
    Write-Host "Criando ambiente Python da IA..."
    Push-Location $aiDir
    python -m venv .venv
    & $venvPython -m pip install -r requirements.txt
    Pop-Location
}

if (-not (Test-PortListening $AiPort)) {
    Write-Host "Subindo servico Python da IA na porta $AiPort..."
    $aiLog = Join-Path $logsDir "ai-service.one-click.log"
    $aiPid = Start-Detached -FilePath $venvPython -Arguments @("-m", "uvicorn", "main:app", "--host", "127.0.0.1", "--port", "$AiPort") -WorkingDirectory $aiDir -LogFile $aiLog
    Write-Host "IA Python PID $aiPid."
} else {
    Write-Host "IA Python ja esta rodando na porta $AiPort." -ForegroundColor Green
}

if (-not (Test-PortListening $ApiPort)) {
    Write-Host "Subindo API .NET na porta $ApiPort..."
    $apiLog = Join-Path $logsDir "api.one-click.log"
    $apiPid = Start-Detached -FilePath "dotnet" -Arguments @("run", "--urls", "http://localhost:$ApiPort") -WorkingDirectory $root -LogFile $apiLog
    Write-Host "API .NET PID $apiPid."
} else {
    Write-Host "API .NET ja esta rodando na porta $ApiPort." -ForegroundColor Green
}

Write-Host "Aguardando servicos responderem..."
Start-Sleep -Seconds 6

$apiOk = Test-PortListening $ApiPort
$aiOk = Test-PortListening $AiPort

Write-Host ""
Write-Host "Status:" -ForegroundColor Cyan
Write-Host "Provider: $env:AI_PROVIDER"
Write-Host "Modelo: $env:MODEL_NAME"
Write-Host "IA Python: $(if ($aiOk) { 'OK' } else { 'OFF' }) - http://127.0.0.1:$AiPort"
Write-Host "API: $(if ($apiOk) { 'OK' } else { 'OFF' }) - http://localhost:$ApiPort"
Write-Host ""
Write-Host "Agora abra o FrontEnd/index.html no Live Server e aperte Ctrl+F5." -ForegroundColor Green
Write-Host "Tambem da para abrir direto: http://localhost:$ApiPort"
Write-Host ""
Write-Host "Logs: $logsDir"

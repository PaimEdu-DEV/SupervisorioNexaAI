@echo off
cd /d "%~dp0"

set "AI_PROVIDER=ollama"
set "MODEL_NAME=qwen3:8b"
set "OLLAMA_MODEL=qwen3:8b"

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0rodar-tudo.ps1" -ForceRestart
pause

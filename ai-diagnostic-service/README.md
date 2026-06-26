# ai-diagnostic-service

Servico local gratuito de IA industrial usando Ollama.

## Requisitos

```powershell
ollama pull qwen2.5:7b
python -m venv .venv
.\.venv\Scripts\python -m pip install -r requirements.txt
```

## Rodar

Com o Ollama ativo em `http://localhost:11434`:

```powershell
.\.venv\Scripts\python -m uvicorn main:app --host 127.0.0.1 --port 8001
```

Fluxo esperado:

Front -> API C# -> ai-diagnostic-service -> Ollama -> ai-diagnostic-service -> API C# -> Front

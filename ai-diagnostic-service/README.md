# ai-diagnostic-service

Servico local da Nexa AI. Ele preserva o mesmo contrato com a API C# (`/diagnose`), mas agora usa uma camada de provider:

`AIProvider -> OllamaProvider -> Qwen3:8b`

No futuro, outros providers podem ser adicionados sem alterar o restante do sistema.

## Configuracao

O modelo principal e local via Ollama:

```powershell
$env:AI_PROVIDER="ollama"
$env:MODEL_NAME="qwen3:8b"
$env:OLLAMA_MODEL="qwen3:8b"
```

`MODEL_NAME` controla o modelo principal. Para trocar de modelo, altere apenas essa variavel.

## Instalar dependencias

```powershell
python -m venv .venv
.\.venv\Scripts\python -m pip install -r requirements.txt
```

## Rodar

```powershell
.\.venv\Scripts\python -m uvicorn main:app --host 127.0.0.1 --port 8001
```

Fluxo esperado:

Front -> API C# -> ai-diagnostic-service -> Ollama Qwen3:8b -> ai-diagnostic-service -> API C# -> Front

## Provider OpenAI opcional

O provider OpenAI continua disponivel para testes futuros, sem alterar o contrato da aplicacao:

```powershell
$env:AI_PROVIDER="openai"
$env:OPENAI_API_KEY="sua-chave-aqui"
$env:MODEL_NAME="modelo-openai"
```

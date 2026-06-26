import json
import os
from typing import Any

import httpx
from fastapi import FastAPI
from pydantic import BaseModel, Field


OLLAMA_CHAT_URL = os.getenv("OLLAMA_CHAT_URL", "http://localhost:11434/api/chat")
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", "qwen2.5:7b")
OUT_OF_CONTEXT_ANSWER = "Sou um Assistente Inteligente especializado exclusivamente nesta máquina industrial."

app = FastAPI(title="ai-diagnostic-service", version="2.0.0")


class DiagnosticRequest(BaseModel):
    question: str
    model: str | None = None
    mode: str = "diagnostic"
    intent: str = "diagnostic"
    brain: str = "diagnostic"
    expectedResponse: str = "structured_json"
    conversation: dict[str, Any] = Field(default_factory=dict)
    context: dict[str, Any] = Field(default_factory=dict)

    # Backward-compatible fields from the first integration.
    componentMap: list[dict[str, Any]] = Field(default_factory=list)
    historyStats: dict[str, Any] = Field(default_factory=dict)
    machine: list[dict[str, Any]] = Field(default_factory=list)
    communication: list[dict[str, Any]] = Field(default_factory=list)
    sensors: list[dict[str, Any]] = Field(default_factory=list)
    actuators: list[dict[str, Any]] = Field(default_factory=list)
    alarms: list[dict[str, Any]] = Field(default_factory=list)
    pendingDiagnostics: list[dict[str, Any]] = Field(default_factory=list)
    maintenanceHistory: list[dict[str, Any]] = Field(default_factory=list)
    tagHistory: list[dict[str, Any]] = Field(default_factory=list)
    lastCommands: list[dict[str, Any]] = Field(default_factory=list)


class DiagnosticResponse(BaseModel):
    answer: str
    message: str | None = None
    intent: str = "diagnostic"
    brain: str = "diagnostic"
    severity: str = "info"
    component: str | None = None
    componentId: str | None = None
    confidence: float = 0.0
    source: str = "ollama"
    showHighlightButton: bool = False
    showInspectionButton: bool = False
    showSeeButton: bool = False
    showInspectButton: bool = False
    probabilities: list[dict[str, Any]] = Field(default_factory=list)
    recommendedActions: list[str] = Field(default_factory=list)
    quickActions: list[dict[str, Any]] = Field(default_factory=list)
    missingData: list[str] = Field(default_factory=list)
    usedSources: list[str] = Field(default_factory=list)
    needsMaintenance: bool = False
    openManual: bool = False
    openHistory: bool = False


def merged_context(request: DiagnosticRequest) -> dict[str, Any]:
    if request.context:
        return request.context

    return {
        "machine": request.machine,
        "communication": request.communication,
        "sensors": request.sensors,
        "actuators": request.actuators,
        "activeAlarms": request.alarms,
        "pendingDiagnostics": request.pendingDiagnostics,
        "maintenanceHistory": request.maintenanceHistory,
        "tagHistory": request.tagHistory[:30],
        "lastCommands": request.lastCommands,
        "conversation": request.conversation,
        "componentMap": request.componentMap,
        "historyStats": request.historyStats,
    }


def valid_component_ids(context: dict[str, Any]) -> set[str]:
    ids: set[str] = set()
    for component in context.get("componentMap", []):
        component_id = component.get("id") or component.get("Id")
        if component_id:
            ids.add(str(component_id))
        tag = component.get("tag") or component.get("Tag")
        if tag:
            ids.add(str(tag))

    component = context.get("component") or {}
    if isinstance(component, dict):
        component_id = component.get("id") or component.get("Id")
        if component_id:
            ids.add(str(component_id))

    technical = context.get("technicalComponent") or {}
    if isinstance(technical, dict):
        component_id = technical.get("componentId") or technical.get("ComponentId")
        if component_id:
            ids.add(str(component_id))

    return ids


def build_messages(request: DiagnosticRequest) -> list[dict[str, str]]:
    context = merged_context(request)
    allowed_ids = sorted(valid_component_ids(context))

    system_prompt = f"""
Voce e o Assistente Inteligente de Diagnostico Industrial do sistema Nexa.
Seu nome na interface e Nexa AI.
Voce e especialista exclusivamente nesta bancada classificadora de pecas.
Voce nao e um chatbot generico.

Intencao classificada pela API: {request.intent}
Cerebro selecionado pela API: {request.brain}

Regras obrigatorias:
- Responda somente sobre maquina, sensores, atuadores, alarmes, producao, historico, diagnostico, CLP, MQTT, automacao da bancada e componentes cadastrados.
- Use apenas as informacoes enviadas no contexto.
- Nunca invente numeros, tags, enderecos, causas, diagnosticos ou historico.
- Se faltar dado, explique exatamente qual dado esta faltando em missingData.
- Nao responda tudo como diagnostico: respeite a intencao e o cerebro selecionados.
- Se a pergunta estiver fora do contexto da maquina, responda exatamente: {OUT_OF_CONTEXT_ANSWER}
- Para perguntas de funcionamento geral, explique a bancada e nao use o ultimo diagnostico como tema principal.
- Para perguntas de producao, use somente contadores, leituras e historicos enviados.
- Para perguntas tecnicas, use technicalMap, componentMap e componentes cadastrados.
- Para localizacao visual, retorne componentId e showHighlightButton=true quando o componente existir.
- Para diagnostico, explique evidencias, causas possiveis e ordem de verificacao.
- Nao use markdown.
- Responda em portugues claro e direto.

IDs validos para componentId:
{json.dumps(allowed_ids, ensure_ascii=False)}

Retorne somente JSON valido neste formato:
{{
  "message": "",
  "answer": "",
  "intent": "diagnostic | machine_info | production_query | component_info | visual_location | history | technical | out_of_scope",
  "brain": "diagnostic | machine_knowledge | database | technical",
  "severity": "info | warning | critical",
  "component": null,
  "componentId": null,
  "showHighlightButton": false,
  "showInspectionButton": false,
  "showSeeButton": false,
  "showInspectButton": false,
  "probabilities": [],
  "recommendedActions": [],
  "quickActions": [],
  "missingData": [],
  "usedSources": [],
  "needsMaintenance": false,
  "openManual": false,
  "openHistory": false,
  "confidence": 0.0
}}
""".strip()

    user_payload = {
        "question": request.question,
        "intent": request.intent,
        "brain": request.brain,
        "conversation": request.conversation,
        "context": context,
    }

    return [
        {"role": "system", "content": system_prompt},
        {"role": "user", "content": json.dumps(user_payload, ensure_ascii=False)},
    ]


def extract_json(content: str) -> dict[str, Any]:
    content = content.strip()
    if content.startswith("```"):
        content = content.strip("`")
        content = content.replace("json", "", 1).strip()

    start = content.find("{")
    end = content.rfind("}")
    if start >= 0 and end >= start:
        content = content[start : end + 1]

    return json.loads(content)


def normalize_response(parsed: dict[str, Any], request: DiagnosticRequest) -> DiagnosticResponse:
    context = merged_context(request)
    valid_ids = valid_component_ids(context)

    answer = str(parsed.get("answer") or "").strip()
    message = str(parsed.get("message") or answer or "").strip()
    if not message:
        message = "Nao existem dados suficientes para responder com seguranca usando apenas os dados disponiveis."
    if not answer:
        answer = message

    component_id = parsed.get("componentId")
    if component_id is not None and str(component_id) not in valid_ids:
        component_id = None

    if message.strip() == OUT_OF_CONTEXT_ANSWER or answer.strip() == OUT_OF_CONTEXT_ANSWER:
        component_id = None

    show_highlight = bool(parsed.get("showHighlightButton") or parsed.get("showSeeButton")) and component_id is not None
    show_inspection = bool(parsed.get("showInspectionButton") or parsed.get("showInspectButton")) and component_id is not None

    severity = str(parsed.get("severity") or "info").lower()
    if severity not in {"info", "warning", "critical"}:
        severity = "info"

    return DiagnosticResponse(
        answer=answer,
        message=message,
        intent=str(parsed.get("intent") or request.intent),
        brain=str(parsed.get("brain") or request.brain),
        severity=severity,
        component=parsed.get("component"),
        componentId=str(component_id) if component_id else None,
        confidence=max(0.0, min(1.0, float(parsed.get("confidence") or 0.0))),
        source="ollama",
        showHighlightButton=show_highlight,
        showInspectionButton=show_inspection,
        showSeeButton=show_highlight,
        showInspectButton=show_inspection,
        probabilities=parsed.get("probabilities") if isinstance(parsed.get("probabilities"), list) else [],
        recommendedActions=parsed.get("recommendedActions") if isinstance(parsed.get("recommendedActions"), list) else [],
        quickActions=parsed.get("quickActions") if isinstance(parsed.get("quickActions"), list) else [],
        missingData=parsed.get("missingData") if isinstance(parsed.get("missingData"), list) else [],
        usedSources=parsed.get("usedSources") if isinstance(parsed.get("usedSources"), list) else [],
        needsMaintenance=bool(parsed.get("needsMaintenance")),
        openManual=bool(parsed.get("openManual")),
        openHistory=bool(parsed.get("openHistory")),
    )


@app.get("/health")
async def health() -> dict[str, str]:
    return {"status": "ok", "model": OLLAMA_MODEL}


@app.post("/diagnose", response_model=DiagnosticResponse)
async def diagnose(request: DiagnosticRequest) -> DiagnosticResponse:
    if request.intent == "out_of_scope":
        return DiagnosticResponse(
            answer=OUT_OF_CONTEXT_ANSWER,
            message=OUT_OF_CONTEXT_ANSWER,
            intent="out_of_scope",
            brain=request.brain,
            source="context-guard",
        )

    model = request.model or OLLAMA_MODEL
    payload = {
        "model": model,
        "stream": False,
        "format": "json",
        "options": {
            "temperature": 0.05,
            "top_p": 0.7,
        },
        "messages": build_messages(request),
    }

    async with httpx.AsyncClient(timeout=90) as client:
        response = await client.post(OLLAMA_CHAT_URL, json=payload)
        response.raise_for_status()

    ollama_data = response.json()
    content = ollama_data.get("message", {}).get("content", "")
    parsed = extract_json(content)
    return normalize_response(parsed, request)

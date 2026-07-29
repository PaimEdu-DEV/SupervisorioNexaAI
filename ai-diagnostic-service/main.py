import json
import os
from abc import ABC, abstractmethod
from pathlib import Path
from typing import Any

import httpx
from fastapi import FastAPI
from pydantic import BaseModel, Field


OPENAI_RESPONSES_URL = os.getenv("OPENAI_RESPONSES_URL", "https://api.openai.com/v1/responses")
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY", "")
MODEL_NAME = os.getenv("MODEL_NAME", os.getenv("AI_MODEL", "qwen3:8b"))
AI_PROVIDER = os.getenv("AI_PROVIDER", "ollama").lower()
OLLAMA_CHAT_URL = os.getenv("OLLAMA_CHAT_URL", "http://localhost:11434/api/chat")
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", MODEL_NAME)
OLLAMA_TIMEOUT_SECONDS = float(os.getenv("OLLAMA_TIMEOUT_SECONDS", "360"))
OPENAI_TIMEOUT_SECONDS = float(os.getenv("OPENAI_TIMEOUT_SECONDS", "90"))
OUT_OF_CONTEXT_ANSWER = "Sou um Assistente Inteligente especializado exclusivamente nesta maquina industrial."
SYSTEM_PROMPT_PATH = Path(__file__).with_name("system_prompt.md")

app = FastAPI(title="ai-diagnostic-service", version="3.0.0")


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


class AIProvider(ABC):
    name: str

    @abstractmethod
    async def generate_json(self, request: DiagnosticRequest, system_prompt: str, user_payload: dict[str, Any]) -> dict[str, Any]:
        raise NotImplementedError


class OpenAIProvider(AIProvider):
    name = "openai"

    async def generate_json(self, request: DiagnosticRequest, system_prompt: str, user_payload: dict[str, Any]) -> dict[str, Any]:
        if not OPENAI_API_KEY:
            raise RuntimeError("OPENAI_API_KEY nao configurada.")

        model = request.model or MODEL_NAME
        payload = {
            "model": model,
            "instructions": system_prompt,
            "input": json.dumps(user_payload, ensure_ascii=False),
        }

        async with httpx.AsyncClient(timeout=OPENAI_TIMEOUT_SECONDS) as client:
            response = await client.post(
                OPENAI_RESPONSES_URL,
                headers={
                    "Authorization": f"Bearer {OPENAI_API_KEY}",
                    "Content-Type": "application/json",
                },
                json=payload,
            )
            response.raise_for_status()

        return extract_json(extract_openai_text(response.json()))


class OllamaProvider(AIProvider):
    name = "ollama"

    async def generate_json(self, request: DiagnosticRequest, system_prompt: str, user_payload: dict[str, Any]) -> dict[str, Any]:
        payload = {
            "model": request.model or OLLAMA_MODEL,
            "stream": False,
            "format": "json",
            "think": False,
            "keep_alive": os.getenv("OLLAMA_KEEP_ALIVE", "30m"),
            "options": {
                "temperature": float(os.getenv("OLLAMA_TEMPERATURE", "0.1")),
                "top_p": float(os.getenv("OLLAMA_TOP_P", "0.75")),
                "top_k": int(os.getenv("OLLAMA_TOP_K", "30")),
                "repeat_penalty": float(os.getenv("OLLAMA_REPEAT_PENALTY", "1.08")),
                "num_ctx": int(os.getenv("OLLAMA_NUM_CTX", "4096")),
                "num_predict": int(os.getenv("OLLAMA_NUM_PREDICT", "384")),
            },
            "messages": [
                {"role": "system", "content": system_prompt},
                {"role": "user", "content": "/no_think\n" + json.dumps(user_payload, ensure_ascii=False)},
            ],
        }

        async with httpx.AsyncClient(timeout=OLLAMA_TIMEOUT_SECONDS) as client:
            response = await client.post(OLLAMA_CHAT_URL, json=payload)
            response.raise_for_status()

        return extract_json(response.json().get("message", {}).get("content", ""))


def provider() -> AIProvider:
    if AI_PROVIDER == "openai":
        return OpenAIProvider()

    return OllamaProvider()


def request_model_name(active_provider: AIProvider) -> str:
    if active_provider.name == "openai":
        return MODEL_NAME

    return OLLAMA_MODEL


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


def build_system_prompt(request: DiagnosticRequest) -> str:
    context = merged_context(request)
    allowed_ids = sorted(valid_component_ids(context))
    template = SYSTEM_PROMPT_PATH.read_text(encoding="utf-8")
    return template.format(
        intent=request.intent,
        brain=request.brain,
        out_of_context_answer=OUT_OF_CONTEXT_ANSWER,
        allowed_component_ids=json.dumps(allowed_ids, ensure_ascii=False),
    )


def build_user_payload(request: DiagnosticRequest) -> dict[str, Any]:
    return {
        "question": request.question,
        "intent": request.intent,
        "brain": request.brain,
        "conversation": request.conversation,
        "context": merged_context(request),
    }


def extract_openai_text(data: dict[str, Any]) -> str:
    if isinstance(data.get("output_text"), str):
        return data["output_text"]

    parts: list[str] = []
    for item in data.get("output", []):
        for content in item.get("content", []):
            text = content.get("text")
            if isinstance(text, str):
                parts.append(text)

    return "\n".join(parts)


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


def normalize_response(parsed: dict[str, Any], request: DiagnosticRequest, source: str) -> DiagnosticResponse:
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
        source=source,
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
async def health() -> dict[str, Any]:
    active_provider = provider()
    return {
        "status": "ok",
        "provider": active_provider.name,
        "model": request_model_name(active_provider),
        "openaiApiKeyConfigured": bool(OPENAI_API_KEY),
    }


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

    active_provider = provider()
    parsed = await active_provider.generate_json(
        request=request,
        system_prompt=build_system_prompt(request),
        user_payload=build_user_payload(request),
    )
    return normalize_response(parsed, request, active_provider.name)

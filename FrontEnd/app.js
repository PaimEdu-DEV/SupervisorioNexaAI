const API_BASE = window.location.port === "5500"
  ? "http://localhost:5081"
  : window.location.origin;

const els = {
  apiSignal: document.querySelector("#apiSignal"),
  assistantButton: document.querySelector("#assistantButton"),
  diagnosticBadge: document.querySelector("#diagnosticBadge"),
  assistantDrawer: document.querySelector("#assistantDrawer"),
  assistantClose: document.querySelector("#assistantClose"),
  diagnosticAlert: document.querySelector("#diagnosticAlert"),
  chatHistory: document.querySelector("#chatHistory"),
  chatForm: document.querySelector("#chatForm"),
  chatInput: document.querySelector("#chatInput"),
  themeToggle: document.querySelector("#themeToggle"),
  themeIcon: document.querySelector("#themeIcon"),
  currentTime: document.querySelector("#currentTime"),
  currentDate: document.querySelector("#currentDate"),
  machineStatus: document.querySelector("#machineStatus"),
  machineMode: document.querySelector("#machineMode"),
  cycleTime: document.querySelector("#cycleTime"),
  plcDot: document.querySelector("#plcDot"),
  mqttDot: document.querySelector("#mqttDot"),
  plcState: document.querySelector("#plcState"),
  mqttState: document.querySelector("#mqttState"),
  totalProcessed: document.querySelector("#totalProcessed"),
  totalApproved: document.querySelector("#totalApproved"),
  totalRejected: document.querySelector("#totalRejected"),
  efficiency: document.querySelector("#efficiency"),
  runIndicator: document.querySelector("#runIndicator"),
  sensorsList: document.querySelector("#sensorsList"),
  actuatorsList: document.querySelector("#actuatorsList"),
  sensorCount: document.querySelector("#sensorCount"),
  alarmList: document.querySelector("#alarmList"),
  alarmCount: document.querySelector("#alarmCount"),
  componentHighlight: document.querySelector("#componentHighlight"),
  componentMarker: document.querySelector("#componentMarker"),
  inspectionPanel: document.querySelector("#inspectionPanel"),
  inspectionTitle: document.querySelector("#inspectionTitle"),
  inspectionTag: document.querySelector("#inspectionTag"),
  inspectionDescription: document.querySelector("#inspectionDescription"),
  inspectionStepLabel: document.querySelector("#inspectionStepLabel"),
  inspectionStepText: document.querySelector("#inspectionStepText"),
  inspectionClose: document.querySelector("#inspectionClose")
};

let pendingDiagnostics = [];
let machineComponents = {};
let currentComponentId = null;
let conversationId = localStorage.getItem("nexa-ai-conversation-id");
let dashboardInitialized = false;
let knownDiagnosticIds = new Set();
let inspectionStep = 0;
let assistantBusy = false;

if (!conversationId) {
  conversationId = crypto.randomUUID ? crypto.randomUUID() : String(Date.now());
  localStorage.setItem("nexa-ai-conversation-id", conversationId);
}

function tickClock() {
  const now = new Date();
  els.currentTime.textContent = now.toLocaleTimeString("pt-BR");
  els.currentDate.textContent = now.toLocaleDateString("pt-BR");
}

function isTrue(value) {
  const normalized = String(value ?? "").toLowerCase();
  return normalized === "true" || normalized === "1" || normalized === "on" || normalized === "ligado";
}

function setTheme(theme) {
  document.documentElement.dataset.theme = theme;
  localStorage.setItem("simmaq-theme", theme);
  els.themeIcon.textContent = theme === "light" ? "Claro" : "Escuro";
}

function setApiState(active) {
  els.apiSignal.classList.toggle("online", active);
}

function setConnection(dot, label, active) {
  dot.classList.toggle("online", active);
  label.textContent = active ? "Online" : "Offline";
}

async function loadComponentMap() {
  try {
    const response = await fetch(`${API_BASE}/api/assistant/components`, { cache: "no-store" });
    if (!response.ok) return;
    const components = await response.json();
    machineComponents = Object.fromEntries(components.map((component) => [component.id, component]));
  } catch {
    machineComponents = {};
  }
}

function renderTagList(container, tags) {
  container.innerHTML = "";

  tags.forEach((tag) => {
    const row = document.createElement("div");
    row.className = "tag-row";
    row.innerHTML = `
      <span class="dot ${isTrue(tag.currentValue) ? "online" : ""}"></span>
      <strong title="${tag.name}">${tag.displayName}</strong>
      <span class="value">${tag.currentValue}${tag.unit ? ` ${tag.unit}` : ""}</span>
    `;
    container.appendChild(row);
  });
}

function renderAlarms(alarms) {
  els.alarmCount.textContent = `${alarms.length} ativos`;
  els.alarmList.innerHTML = "";

  if (alarms.length === 0) {
    els.alarmList.innerHTML = `<div class="empty-state">Nenhum alarme ativo</div>`;
    return;
  }

  alarms.forEach((alarm) => {
    const item = document.createElement("div");
    item.className = "alarm-item";
    item.textContent = alarm.displayName;
    els.alarmList.appendChild(item);
  });
}

function renderDiagnostics(diagnostics) {
  pendingDiagnostics = diagnostics;
  els.diagnosticBadge.textContent = diagnostics.length;
  els.assistantButton.classList.toggle("has-alert", diagnostics.length > 0);

  const unseen = diagnostics.find((diagnostic) => !knownDiagnosticIds.has(diagnostic.id));
  diagnostics.forEach((diagnostic) => knownDiagnosticIds.add(diagnostic.id));

  if (dashboardInitialized && unseen) {
    addStructuredMessage("assistant", {
      message: "Novo diagnóstico disponível. Possível falha detectada pelo motor de diagnóstico industrial.",
      severity: unseen.severity,
      componentId: unseen.sourceTag,
      showSeeButton: Boolean(unseen.sourceTag),
      showInspectButton: Boolean(unseen.sourceTag),
      recommendedActions: [unseen.recommendedActions].filter(Boolean),
      quickActions: [
        { id: "see", label: "Ver na Máquina", enabled: Boolean(unseen.sourceTag) },
        { id: "inspect", label: "Iniciar Inspeção", enabled: Boolean(unseen.sourceTag) },
        { id: "full-diagnostic", label: "Diagnóstico Completo", enabled: true }
      ]
    });
  }

  if (diagnostics.length === 0) {
    els.diagnosticAlert.classList.remove("critical");
    els.diagnosticAlert.innerHTML = `
      <strong>Nenhum alerta pendente</strong>
      <span>O motor de diagnostico esta monitorando a maquina.</span>
    `;
    return;
  }

  const main = diagnostics[0];
  els.diagnosticAlert.classList.toggle("critical", main.severity === "Critical");
  els.diagnosticAlert.innerHTML = `
    <strong>${main.title}</strong>
    <span>${main.description}</span>
  `;
}

function highlightComponent(componentId) {
  if (!componentId) {
    return;
  }

  currentComponentId = componentId;
  const component = machineComponents[componentId];

  document.querySelectorAll(".tag-row.highlight").forEach((row) => row.classList.remove("highlight"));

  const rows = document.querySelectorAll(".tag-row");
  const match = Array.from(rows).find((row) => {
    const title = row.querySelector("strong")?.getAttribute("title") || "";
    return title === componentId || title.includes(componentId);
  });

  if (match) {
    match.classList.add("highlight");
    match.scrollIntoView({ block: "nearest", behavior: "smooth" });
  }

  if (component) {
    els.componentMarker.style.left = `${component.x}%`;
    els.componentMarker.style.top = `${component.y}%`;
    els.componentMarker.title = `${component.name} - ${component.description}`;
    els.componentMarker.classList.add("visible");
  }

  els.componentHighlight.classList.add("visible");
  window.setTimeout(() => {
    match?.classList.remove("highlight");
    els.componentHighlight.classList.remove("visible");
    els.componentMarker.classList.remove("visible");
  }, 4200);
}

function runQuickAction(actionId, payload = {}) {
  if (actionId === "see") {
    highlightComponent(payload.componentId);
    return;
  }

  if (actionId === "inspect") {
    startInspection(payload.componentId);
    return;
  }

  if (actionId === "refresh") {
    loadDashboard();
    return;
  }

  if (actionId === "full-diagnostic") {
    askAssistant("Faça um diagnóstico completo do estado atual da máquina.");
    return;
  }

  if (actionId === "history") {
    askAssistant("Consulte o histórico desta falha e calcule as probabilidades.");
    return;
  }

  if (actionId === "manual") {
    askAssistant("Explique a função deste componente como um manual técnico da bancada.");
  }
}

function addStructuredMessage(role, data) {
  const messageText = data.message || data.answer || data;
  const componentId = data.componentId || null;
  const message = document.createElement("div");
  message.className = `message ${role}`;

  const title = document.createElement("strong");
  title.textContent = role === "user" ? "Operador" : "Nexa AI";

  const paragraph = document.createElement("p");
  paragraph.textContent = messageText;

  message.append(title, paragraph);

  const probabilities = data.probabilities || [];
  if (probabilities.length > 0) {
    const meta = document.createElement("div");
    meta.className = "message-meta";
    meta.textContent = `Probabilidades: ${probabilities.map((item) => `${item.cause}: ${Math.round(item.percent)}%`).join(" | ")}`;
    message.appendChild(meta);
  }

  const actions = (data.quickActions || []).filter((action) => action.enabled !== false);
  if ((data.showSeeButton || data.showHighlightButton) && !actions.some((action) => action.id === "see")) {
    actions.unshift({ id: "see", label: "Ver na Máquina", enabled: true });
  }
  if ((data.showInspectButton || data.showInspectionButton) && !actions.some((action) => action.id === "inspect")) {
    actions.splice(1, 0, { id: "inspect", label: "Iniciar Inspeção", enabled: true });
  }

  if (actions.length > 0) {
    const actionBar = document.createElement("div");
    actionBar.className = "message-actions";
    actions.forEach((action) => {
      const button = document.createElement("button");
      button.type = "button";
      button.textContent = action.label;
      button.disabled = action.enabled === false;
      button.addEventListener("click", () => runQuickAction(action.id, { ...data, componentId }));
      actionBar.appendChild(button);
    });
    message.appendChild(actionBar);
  }

  if (componentId) {
    currentComponentId = componentId;
  }

  els.chatHistory.appendChild(message);
  els.chatHistory.scrollTop = els.chatHistory.scrollHeight;
}

function addMessage(role, text, componentId = null) {
  const message = document.createElement("div");
  message.className = `message ${role}`;
  message.innerHTML = `
    <strong>${role === "user" ? "Operador" : "Nexa AI"}</strong>
    <p>${text}</p>
  `;

  if (componentId) {
    const button = document.createElement("button");
    button.type = "button";
    button.className = "see-component";
    button.textContent = "Veja";
    button.addEventListener("click", () => highlightComponent(componentId));
    message.appendChild(button);
  }

  els.chatHistory.appendChild(message);
  els.chatHistory.scrollTop = els.chatHistory.scrollHeight;
}

function addThinkingMessage() {
  const message = document.createElement("div");
  message.className = "message assistant thinking";
  message.setAttribute("aria-live", "polite");
  message.innerHTML = `
    <strong>Nexa AI</strong>
    <div class="thinking-row">
      <span class="thinking-dot"></span>
      <span class="thinking-dot"></span>
      <span class="thinking-dot"></span>
      <p>Analisando dados da máquina</p>
    </div>
  `;

  els.chatHistory.appendChild(message);
  els.chatHistory.scrollTop = els.chatHistory.scrollHeight;
  return message;
}

function setAssistantBusy(active) {
  assistantBusy = active;
  els.chatInput.disabled = active;
  const submitButton = els.chatForm.querySelector("button[type='submit']");
  if (submitButton) {
    submitButton.disabled = active;
    submitButton.textContent = active ? "..." : "Enviar";
  }
}

async function askAssistant(question) {
  if (assistantBusy) return;

  addMessage("user", question);
  const thinkingMessage = addThinkingMessage();
  setAssistantBusy(true);

  try {
    const response = await fetch(`${API_BASE}/api/assistant/ask`, {
      method: "POST",
      headers: { "Content-Type": "application/json; charset=utf-8" },
      body: JSON.stringify({
        question,
        conversationId,
        componentId: currentComponentId,
        mode: "diagnostic"
      })
    });

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    const data = await response.json();
    thinkingMessage.remove();
    addStructuredMessage("assistant", data);
  } catch {
    thinkingMessage.remove();
    addStructuredMessage("assistant", {
      message: "Não consegui consultar a IA agora. Verifique se a API, o serviço Python e o Ollama estão em execução.",
      severity: "Warning",
      quickActions: [{ id: "refresh", label: "Atualizar Dados", enabled: true }]
    });
  } finally {
    setAssistantBusy(false);
    els.chatInput.focus();
  }
}

async function saveInspectionResult(componentId, realCause) {
  if (!realCause) return;

  await fetch(`${API_BASE}/api/assistant/inspection/complete`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      componentId,
      failureDescription: `Inspeção guiada concluída para ${componentId}.`,
      realCause,
      actionTaken: "Inspeção guiada pelo Assistente IA.",
      registeredBy: "Operador"
    })
  });

  addMessage("assistant", "Histórico de manutenção salvo. Usarei essa informação nas próximas probabilidades.");
}

function startInspection(componentId) {
  if (!componentId) return;

  currentComponentId = componentId;
  const component = machineComponents[componentId];
  highlightComponent(componentId);
  inspectionStep = 1;

  els.inspectionTitle.textContent = component?.name || componentId;
  els.inspectionTag.textContent = component?.tag || componentId;
  els.inspectionDescription.textContent = component?.description || "Componente relacionado ao diagnóstico atual.";
  els.inspectionStepLabel.textContent = "Passo 1";
  els.inspectionStepText.textContent = "Verifique visualmente se o LED do componente indicado está aceso e se há peça, cabo solto ou obstrução na área destacada.";
  els.inspectionPanel.classList.add("open");
  els.inspectionPanel.setAttribute("aria-hidden", "false");
}

function advanceInspection(answer) {
  inspectionStep += 1;

  if (inspectionStep === 2) {
    els.inspectionStepLabel.textContent = "Passo 2";
    els.inspectionStepText.textContent = answer === "no"
      ? "Sem resposta visual. Verifique alimentação, cabo e conector antes de substituir o sensor."
      : "Confirme se o estado do sensor no supervisório muda quando a peça passa pelo ponto indicado.";
    return;
  }

  if (inspectionStep === 3) {
    els.inspectionStepLabel.textContent = "Passo 3";
    els.inspectionStepText.textContent = "Se o estado não mudar, registre a causa real ao resolver o diagnóstico para alimentar o histórico inteligente.";
    return;
  }

  els.inspectionPanel.classList.remove("open");
  els.inspectionPanel.setAttribute("aria-hidden", "true");
  const resolved = window.confirm("Problema resolvido?");
  if (resolved) {
    const realCause = window.prompt("Qual era a causa real? Ex: Sensor, Cabo, Conector, CLP, Válvula, Cilindro, Pneumática, Elétrica, Outro");
    saveInspectionResult(currentComponentId, realCause);
  }
  askAssistant("Com base na inspeção guiada, qual a conclusão provável?");
}

async function loadDashboard() {
  try {
    const response = await fetch(`${API_BASE}/api/dashboard/overview`, { cache: "no-store" });
    if (!response.ok) throw new Error(`HTTP ${response.status}`);

    const data = await response.json();
    setApiState(true);

    els.machineStatus.textContent = data.machine.status || "Parada";
    els.machineMode.textContent = data.machine.mode || "Manual";
    els.cycleTime.textContent = data.machine.cycleTimeCurrent || "0";
    els.totalProcessed.textContent = data.production.totalProcessed ?? 0;
    els.totalApproved.textContent = data.production.totalApproved ?? 0;
    els.totalRejected.textContent = data.production.totalRejected ?? 0;
    els.efficiency.textContent = data.production.machineEfficiency ?? 0;

    const running = String(data.machine.status || "").toLowerCase() === "executando";
    els.runIndicator.classList.toggle("online", running);
    els.runIndicator.querySelector("strong").textContent = running ? "Executando" : "Standby";

    setConnection(els.plcDot, els.plcState, data.communication.plcConnected);
    setConnection(els.mqttDot, els.mqttState, data.communication.mqttConnected);

    const sensors = data.sensors || [];
    renderTagList(els.sensorsList, sensors);
    renderTagList(els.actuatorsList, data.actuators || []);
    renderAlarms(data.activeAlarms || []);
    renderDiagnostics(data.pendingDiagnostics || []);

    const activeSensors = sensors.filter((tag) => isTrue(tag.currentValue)).length;
    els.sensorCount.textContent = `${activeSensors} ativos`;
    dashboardInitialized = true;
  } catch {
    setApiState(false);
  }
}

els.themeToggle.addEventListener("click", () => {
  const current = document.documentElement.dataset.theme;
  setTheme(current === "dark" ? "light" : "dark");
});

els.assistantButton.addEventListener("click", () => {
  els.assistantDrawer.classList.add("open");
  els.assistantDrawer.setAttribute("aria-hidden", "false");

  if (pendingDiagnostics.length > 0) {
    const main = pendingDiagnostics[0];
    addMessage("assistant", `${main.title}\n\n${main.description}\n\nAcao recomendada: ${main.recommendedActions}`);
  }
});

els.assistantClose.addEventListener("click", () => {
  els.assistantDrawer.classList.remove("open");
  els.assistantDrawer.setAttribute("aria-hidden", "true");
});

els.inspectionClose.addEventListener("click", () => {
  els.inspectionPanel.classList.remove("open");
  els.inspectionPanel.setAttribute("aria-hidden", "true");
});

document.querySelectorAll("[data-inspection-answer]").forEach((button) => {
  button.addEventListener("click", () => advanceInspection(button.dataset.inspectionAnswer));
});

els.chatForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  const question = els.chatInput.value.trim();

  if (!question) {
    return;
  }

  els.chatInput.value = "";
  await askAssistant(question);
});

setTheme(localStorage.getItem("simmaq-theme") || "dark");
tickClock();
loadComponentMap();
loadDashboard();
setInterval(tickClock, 1000);
setInterval(loadDashboard, 2000);

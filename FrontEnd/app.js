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
  alarmCount: document.querySelector("#alarmCount")
};

let pendingDiagnostics = [];

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

function addMessage(role, text) {
  const message = document.createElement("div");
  message.className = `message ${role}`;
  message.innerHTML = `
    <strong>${role === "user" ? "Operador" : "Assistente IA"}</strong>
    <p>${text}</p>
  `;
  els.chatHistory.appendChild(message);
  els.chatHistory.scrollTop = els.chatHistory.scrollHeight;
}

async function askAssistant(question) {
  addMessage("user", question);
  addMessage("assistant", "Analisando maquina, sensores, atuadores, alarmes e diagnosticos pendentes...");

  const response = await fetch(`${API_BASE}/api/assistant/ask`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ question })
  });

  const data = await response.json();
  els.chatHistory.lastElementChild.remove();
  addMessage("assistant", data.answer);
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
loadDashboard();
setInterval(tickClock, 1000);
setInterval(loadDashboard, 2000);

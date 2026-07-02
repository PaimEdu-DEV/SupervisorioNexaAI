const isLiveServer = ["localhost", "127.0.0.1"].includes(window.location.hostname)
  && /^55\d\d$/.test(window.location.port);

const API_BASE = isLiveServer || window.location.protocol === "file:"
  ? "http://localhost:5081"
  : window.location.origin;

const MACHINE_OVERVIEW_IMAGE = "img/BancadaCPe%C3%A7a.png-removebg-preview.png";

const SENSOR_IMAGES = {
  left: "img/focus/left-focus.png",
  conveyor: "img/focus/conveyor-focus.png",
  right: "img/focus/right-focus.png"
};

// Cadastro central dos sensores. Para adicionar ou ajustar sensores, edite apenas este objeto.
const sensors = {
  DI0: {
    id: "DI0",
    name: "Sensor Capacitivo - Entrada",
    type: "Sensor capacitivo",
    area: "Mesa esquerda",
    componentId: "sensor_entry_capacitive",
    tagNames: ["Sensors.EntrySlotCapacitive", "Sensors.Entry.Capacitive"],
    detailImage: SENSOR_IMAGES.left,
    overviewPosition: { x: 22, y: 58 },
    position: { x: 72, y: 53 },
    description: "Detecta a presenca de peca no slot de entrada da bancada.",
    failures: ["Peca fora da area de deteccao", "Sensor desalinhado", "Cabo desconectado", "Entrada digital sem sinal"],
    recommendations: ["Verifique o LED do sensor", "Confirme o sinal DI0 no CLP", "Limpe e reposicione a peca no slot"],
    aliases: ["entrada", "slot de entrada", "sensor de entrada", "peca na entrada"]
  },
  DI1: {
    id: "DI1",
    name: "Sensor Magnetico - Eixo X Recuado",
    type: "Sensor magnetico",
    area: "Mesa esquerda",
    componentId: "sensor_axis_x_retracted",
    tagNames: ["Sensors.AxisXRetracted", "Sensors.AxisX.Retracted"],
    detailImage: SENSOR_IMAGES.left,
    overviewPosition: { x: 30, y: 42 },
    position: { x: 72, y: 55 },
    description: "Confirma que o eixo X voltou para a posicao recuada.",
    failures: ["Sensor desalinhado", "Ima do cilindro fora de posicao", "Cabo com mau contato", "Entrada digital sem sinal"],
    recommendations: ["Acione o eixo X manualmente", "Confira se o LED muda no fim de curso", "Meça DI1 no CLP"],
    aliases: ["eixo x recuado", "sensor da esquerda", "sensor indutivo da esquerda", "mesa esquerda"]
  },
  DI2: {
    id: "DI2",
    name: "Sensor Magnetico - Eixo X Avancado",
    type: "Sensor magnetico",
    area: "Mesa esquerda",
    componentId: "sensor_axis_x_advanced",
    tagNames: ["Sensors.AxisXAdvanced", "Sensors.AxisX.Advanced"],
    detailImage: SENSOR_IMAGES.left,
    overviewPosition: { x: 39, y: 42 },
    position: { x: 58, y: 44 },
    description: "Confirma que o eixo X chegou na posicao avancada.",
    failures: ["Cilindro nao avancou", "Sensor fora da faixa do ima", "Baixa pressao pneumatica", "Sinal digital interrompido"],
    recommendations: ["Verifique pressao e valvula do eixo X", "Ajuste a posicao do sensor", "Compare DI2 com o movimento real"]
  },
  DI3: {
    id: "DI3",
    name: "Sensor Magnetico - Eixo Y Recuado",
    type: "Sensor magnetico",
    area: "Mesa direita",
    componentId: "sensor_axis_y_retracted",
    tagNames: ["Sensors.AxisYRetracted", "Sensors.AxisY.Retracted"],
    detailImage: SENSOR_IMAGES.right,
    overviewPosition: { x: 61, y: 42 },
    position: { x: 80, y: 32 },
    description: "Confirma que o eixo Y esta recolhido na mesa direita.",
    failures: ["Sensor desalinhado", "Cabo solto", "Cilindro travado", "Entrada DI3 sem sinal"],
    recommendations: ["Confira o LED do sensor", "Teste o recuo do eixo Y", "Verifique o conector do sensor"],
    aliases: ["eixo y recuado"]
  },
  DI4: {
    id: "DI4",
    name: "Sensor Magnetico - Eixo Y Avancado",
    type: "Sensor magnetico",
    area: "Mesa direita",
    componentId: "sensor_axis_y_advanced",
    tagNames: ["Sensors.AxisYAdvanced", "Sensors.AxisY.Advanced"],
    detailImage: SENSOR_IMAGES.right,
    overviewPosition: { x: 70, y: 42 },
    position: { x: 58, y: 45 },
    description: "Confirma o avancamento do eixo Y durante o ciclo da bancada.",
    failures: ["Eixo Y nao chegou ao fim de curso", "Sensor fora de posicao", "Cabo com mau contato", "Entrada DI4 nao recebendo sinal"],
    recommendations: ["Acione o eixo Y em manual", "Confirme se DI4 muda de estado", "Verifique valvula, cilindro e alinhamento"],
    aliases: ["eixo y avancado", "sensor da mesa direita", "mesa direita"]
  },
  DI5: {
    id: "DI5",
    name: "Sensor Magnetico - Eixo Z Recuado",
    type: "Sensor magnetico",
    area: "Mesa direita",
    componentId: "sensor_axis_z_retracted",
    tagNames: ["Sensors.AxisZRetracted", "Sensors.AxisZ.Retracted"],
    detailImage: SENSOR_IMAGES.right,
    overviewPosition: { x: 50, y: 35 },
    position: { x: 25, y: 32 },
    description: "Confirma que o eixo Z esta na posicao recuada.",
    failures: ["Sensor deslocado", "Eixo Z preso", "Falha na valvula", "Entrada DI5 sem sinal"],
    recommendations: ["Verifique movimento do eixo Z", "Ajuste o sensor magnetico", "Confira sinal no CLP"]
  },
  DI6: {
    id: "DI6",
    name: "Sensor Magnetico - Eixo Z Avancado",
    type: "Sensor magnetico",
    area: "Mesa direita",
    componentId: "sensor_axis_z_advanced",
    tagNames: ["Sensors.AxisZAdvanced", "Sensors.AxisZ.Advanced"],
    detailImage: SENSOR_IMAGES.right,
    overviewPosition: { x: 50, y: 43 },
    position: { x: 31, y: 43 },
    description: "Confirma que o eixo Z chegou na posicao avancada.",
    failures: ["Eixo Z nao avancou", "Baixa pressao", "Sensor sem alimentacao", "Entrada DI6 travada"],
    recommendations: ["Teste a descida/subida do eixo Z", "Confira pressao pneumatica", "Valide DI6 no CLP"]
  },
  DI7: {
    id: "DI7",
    name: "Sensor Indutivo",
    type: "Sensor indutivo",
    area: "Esteira central",
    componentId: "sensor_inductive",
    tagNames: ["Sensors.Inductive", "Sensors.Classification.Inductive"],
    detailImage: SENSOR_IMAGES.conveyor,
    overviewPosition: { x: 48, y: 56 },
    position: { x: 89, y: 30 },
    description: "Detecta caracteristica metalica da peca na regiao de classificacao.",
    failures: ["Peca metalica fora da area", "Sensor desalinhado", "Distancia de deteccao incorreta", "Entrada DI7 sem sinal"],
    recommendations: ["Aproxime uma peca metalica para teste", "Confira distancia do sensor", "Verifique LED e cabo"],
    aliases: ["sensor indutivo", "indutivo"]
  },
  DI8: {
    id: "DI8",
    name: "Sensor Optico Reflexivo",
    type: "Sensor fotoeletrico",
    area: "Esteira central",
    componentId: "sensor_optical_reflective",
    tagNames: ["Sensors.OpticalReflexive", "Sensors.Classification.OpticalReflective"],
    detailImage: SENSOR_IMAGES.conveyor,
    overviewPosition: { x: 54, y: 50 },
    position: { x: 77, y: 29 },
    description: "Detecta a presenca/passagem da peca na esteira transportadora.",
    failures: ["Sensor sujo", "Reflexo insuficiente", "Peca mal posicionada", "Cabo com mau contato", "Entrada DI8 sem sinal"],
    recommendations: ["Limpe a lente", "Teste com peca na frente do sensor", "Confira alinhamento e entrada DI8"],
    aliases: ["sensor da esteira", "esteira", "transportador", "peca na esteira"]
  },
  DI9: {
    id: "DI9",
    name: "Sensor Optico com Espelho 1",
    type: "Sensor fotoeletrico com refletor",
    area: "Esteira central",
    componentId: "sensor_optical_mirror_1",
    tagNames: ["Sensors.OpticalMirror1", "Sensors.Classification.OpticalMirror1"],
    detailImage: SENSOR_IMAGES.conveyor,
    overviewPosition: { x: 39, y: 51 },
    position: { x: 63, y: 34 },
    description: "Monitora a passagem da peca usando barreira/reflexao com espelho.",
    failures: ["Espelho desalinhado", "Lente suja", "Reflexo fraco", "Entrada DI9 instavel"],
    recommendations: ["Alinhe sensor e espelho", "Limpe lente/refletor", "Observe se o LED comuta durante a passagem"]
  },
  DI10: {
    id: "DI10",
    name: "Sensor Optico com Espelho 2",
    type: "Sensor fotoeletrico com refletor",
    area: "Esteira central",
    componentId: "sensor_optical_mirror_2",
    tagNames: ["Sensors.OpticalMirror2", "Sensors.Classification.OpticalMirror2"],
    detailImage: SENSOR_IMAGES.conveyor,
    overviewPosition: { x: 63, y: 51 },
    position: { x: 27, y: 34 },
    description: "Segundo sensor optico com espelho usado na area de classificacao da esteira.",
    failures: ["Espelho fora de alinhamento", "Sensor sujo", "Peca nao interrompe o feixe", "Entrada DI10 nao acionou"],
    recommendations: ["Alinhe o refletor", "Limpe o conjunto optico", "Confira DI10 no CLP durante o ciclo"],
    aliases: ["sensor di10 nao acionou", "di10 nao acionou"]
  },
  DI11: {
    id: "DI11",
    name: "Sensor Capacitivo - Saida",
    type: "Sensor capacitivo",
    area: "Mesa direita",
    componentId: "sensor_exit_capacitive",
    tagNames: ["Sensors.ExitSlotCapacitive", "Sensors.Exit.Capacitive"],
    detailImage: SENSOR_IMAGES.right,
    overviewPosition: { x: 77, y: 58 },
    position: { x: 36, y: 67 },
    description: "Detecta a peca no slot de saida da bancada.",
    failures: ["Peca nao chegou na saida", "Sensor desalinhado", "Cabo solto", "Entrada DI11 sem sinal"],
    recommendations: ["Verifique se ha peca no slot de saida", "Confira LED do sensor", "Teste a entrada DI11"],
    aliases: ["saida", "slot de saida", "sensor de saida"]
  }
};

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
  machineView: document.querySelector("#machineView"),
  machineImage: document.querySelector("#machineImage"),
  sensorOverlay: document.querySelector("#sensorOverlay"),
  sensorBack: document.querySelector("#sensorBack"),
  sensorDetailCard: document.querySelector("#sensorDetailCard"),
  sensorDetailArea: document.querySelector("#sensorDetailArea"),
  sensorDetailTitle: document.querySelector("#sensorDetailTitle"),
  sensorDetailType: document.querySelector("#sensorDetailType"),
  sensorDetailStatus: document.querySelector("#sensorDetailStatus"),
  sensorDetailDescription: document.querySelector("#sensorDetailDescription"),
  sensorDetailFailures: document.querySelector("#sensorDetailFailures"),
  sensorDetailRecommendations: document.querySelector("#sensorDetailRecommendations"),
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
let selectedSensor = null;
let sensorStatusByTag = {};
let activeFaultSourceTags = new Set();
let selectedSensorFaultIntent = false;

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

function normalizeText(value) {
  return String(value ?? "")
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .toUpperCase();
}

function detectSensorFromMessage(message) {
  const normalized = normalizeText(message);
  const directMatch = Object.keys(sensors).find((sensorId) =>
    new RegExp(`(^|\\W)${sensorId}(\\W|$)`).test(normalized)
  );

  if (directMatch) {
    return sensors[directMatch];
  }

  const byAliasOrName = Object.values(sensors).find((sensor) =>
    (sensor.aliases || []).some((alias) => normalized.includes(normalizeText(alias))) ||
    normalized.includes(normalizeText(sensor.name))
  );

  if (byAliasOrName) {
    return byAliasOrName;
  }

  return Object.values(sensors).find((sensor) => normalized.includes(normalizeText(sensor.area)));
}

function sensorFromComponent(componentId) {
  if (!componentId) return null;
  return Object.values(sensors).find((sensor) =>
    sensor.componentId === componentId ||
    sensor.tagNames.some((tagName) => tagName === componentId) ||
    sensor.id === componentId
  );
}

function isFaultQuestion(message) {
  return normalizeText(message).match(/FALHA|PROBLEMA|ERRO|NAO ACIONOU|NÃO ACIONOU|TRAVOU|DEFEITO/) !== null;
}

function getSensorState(sensor) {
  const matchingTags = sensor.tagNames
    .map((tagName) => sensorStatusByTag[tagName])
    .filter(Boolean);
  const hasFault = selectedSensorFaultIntent ||
    sensor.tagNames.some((tagName) => activeFaultSourceTags.has(tagName)) ||
    activeFaultSourceTags.has(sensor.componentId);
  const isActive = matchingTags.some((tag) => isTrue(tag.currentValue));

  if (hasFault) {
    return { key: "fault", label: "Falha / verificar", active: isActive };
  }

  if (isActive) {
    return { key: "ok", label: "Acionado / OK", active: true };
  }

  return { key: "idle", label: "Inativo", active: false };
}

function createSensorMarker(sensor, mode) {
  const state = getSensorState(sensor);
  const position = mode === "detail" ? sensor.position : sensor.overviewPosition;
  const marker = document.createElement("button");
  marker.type = "button";
  marker.className = `sensor-marker ${state.key} ${mode === "detail" ? "selected" : ""}`;
  marker.style.left = `${position.x}%`;
  marker.style.top = `${position.y}%`;
  marker.setAttribute("aria-label", `${sensor.id} - ${sensor.name}`);
  marker.innerHTML = `<span></span><strong>${sensor.id}</strong>`;
  marker.addEventListener("click", () => {
    selectSensor(sensor, { fault: state.key === "fault", openAssistant: false });
  });
  return marker;
}

function renderSensorOverlay() {
  els.sensorOverlay.innerHTML = "";

  if (selectedSensor) {
    els.sensorOverlay.appendChild(createSensorMarker(selectedSensor, "detail"));
    return;
  }

  Object.values(sensors).forEach((sensor) => {
    els.sensorOverlay.appendChild(createSensorMarker(sensor, "overview"));
  });
}

function renderSensorDetail(sensor) {
  const state = getSensorState(sensor);
  els.sensorDetailArea.textContent = sensor.area;
  els.sensorDetailTitle.textContent = sensor.id;
  els.sensorDetailType.textContent = sensor.type;
  els.sensorDetailStatus.textContent = state.label;
  els.sensorDetailDescription.textContent = sensor.description;
  els.sensorDetailFailures.innerHTML = sensor.failures.map((item) => `<li>${item}</li>`).join("");
  els.sensorDetailRecommendations.innerHTML = sensor.recommendations.map((item) => `<li>${item}</li>`).join("");
  els.sensorDetailCard.classList.toggle("fault", state.key === "fault");
  els.sensorDetailCard.classList.toggle("ok", state.key === "ok");
  els.sensorDetailCard.setAttribute("aria-hidden", "false");
}

function selectSensor(sensor, options = {}) {
  selectedSensor = sensor;
  selectedSensorFaultIntent = Boolean(options.fault);
  currentComponentId = sensor.componentId;

  els.machineImage.src = sensor.detailImage;
  els.machineImage.alt = `${sensor.area} - ${sensor.name}`;
  els.machineView.classList.add("sensor-focus");
  els.machineView.classList.toggle("fault", getSensorState(sensor).key === "fault");
  renderSensorDetail(sensor);
  renderSensorOverlay();
}

function resetSensorView() {
  selectedSensor = null;
  selectedSensorFaultIntent = false;
  els.machineImage.src = MACHINE_OVERVIEW_IMAGE;
  els.machineImage.alt = "Bancada SIMMAQ NXA";
  els.machineView.classList.remove("sensor-focus", "fault");
  els.sensorDetailCard.classList.remove("fault", "ok");
  els.sensorDetailCard.setAttribute("aria-hidden", "true");
  renderSensorOverlay();
}

function buildSensorQuestion(question, sensor) {
  const state = getSensorState(sensor);
  return `O usuario perguntou sobre o sensor ${sensor.id}. Pergunta original: "${question}". ` +
    `Contexto visual: nome=${sensor.name}; tipo=${sensor.type}; area=${sensor.area}; status=${state.label}; funcao=${sensor.description}; ` +
    `possiveis falhas=${sensor.failures.join(", ")}; recomendacoes=${sensor.recommendations.join(", ")}. ` +
    "Responda em portugues, direto e tecnico, explicando funcao, localizacao, status atual e o que verificar.";
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

  const sensor = sensorFromComponent(componentId);
  if (sensor) {
    selectSensor(sensor, { fault: activeFaultSourceTags.has(componentId) });
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
    const sensor = sensorFromComponent(componentId);
    if (sensor) {
      selectSensor(sensor, { fault: String(data.severity || "").toLowerCase() === "critical" || String(data.severity || "").toLowerCase() === "warning" });
    }
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

  const detectedSensor = detectSensorFromMessage(question);
  if (detectedSensor) {
    selectSensor(detectedSensor, { fault: isFaultQuestion(question) });
  }

  addMessage("user", question);
  const thinkingMessage = addThinkingMessage();
  setAssistantBusy(true);

  try {
    const response = await fetch(`${API_BASE}/api/assistant/ask`, {
      method: "POST",
      headers: { "Content-Type": "application/json; charset=utf-8" },
      body: JSON.stringify({
        question: detectedSensor ? buildSensorQuestion(question, detectedSensor) : question,
        conversationId,
        componentId: detectedSensor?.componentId || currentComponentId,
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
    sensorStatusByTag = Object.fromEntries(sensors.map((tag) => [tag.name, tag]));
    activeFaultSourceTags = new Set((data.pendingDiagnostics || []).map((diagnostic) => diagnostic.sourceTag).filter(Boolean));
    renderTagList(els.sensorsList, sensors);
    renderTagList(els.actuatorsList, data.actuators || []);
    renderAlarms(data.activeAlarms || []);
    renderDiagnostics(data.pendingDiagnostics || []);
    if (selectedSensor) {
      renderSensorDetail(selectedSensor);
    }
    renderSensorOverlay();

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

els.sensorBack.addEventListener("click", resetSensorView);

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
renderSensorOverlay();
loadComponentMap();
loadDashboard();
setInterval(tickClock, 1000);
setInterval(loadDashboard, 2000);

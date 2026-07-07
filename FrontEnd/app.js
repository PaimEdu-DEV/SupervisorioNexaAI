const isLiveServer = ["localhost", "127.0.0.1"].includes(window.location.hostname)
  && /^55\d\d$/.test(window.location.port);

const API_BASE = isLiveServer || window.location.protocol === "file:"
  ? "http://localhost:5081"
  : window.location.origin;

const MACHINE_OVERVIEW_IMAGE = "img/BancadaCPe%C3%A7a.png-removebg-preview.png";

const SENSOR_IMAGES = {
  left: "img/BRA%C3%87O%20ESQ.png",
  conveyor: "img/ESTEIRA.png",
  right: "img/BRA%C3%87O%201%20DIR.png"
};

const EDITOR_PASSWORD = "1234567";
const LAYOUT_STORAGE_KEY = "simmaq-nexa-editor-layout-v1";

const IMAGE_CONTEXTS = {
  overview: { key: "overview", label: "Visao Geral", image: MACHINE_OVERVIEW_IMAGE },
  left: { key: "left", label: "Braco Esquerdo", image: SENSOR_IMAGES.left },
  conveyor: { key: "conveyor", label: "Esteira", image: SENSOR_IMAGES.conveyor },
  right: { key: "right", label: "Braco Direito", image: SENSOR_IMAGES.right }
};

// Base oficial unica. Marcadores guardam apenas sensorId + posicao; todos os dados vem daqui.
const sensorCatalog = window.SIMMAQ_SENSOR_REGISTRY || {
  "DI0": {
    "id": "DI0",
    "name": "DI0 - Sensor Capacitivo, peca no slot de entrada",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Sensor Capacitivo, peca no slot de entrada",
    "statusKey": "DI0",
    "tagNames": [
      "DI0",
      "%MW0:X0"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X0",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Capacitivo, peca no slot de entrada"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI0",
      "Sensor Capacitivo, peca no slot de entrada"
    ]
  },
  "DI1": {
    "id": "DI1",
    "name": "DI1 - Sensor Magnetico, eixo X recuado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Sensor Magnetico, eixo X recuado",
    "statusKey": "DI1",
    "tagNames": [
      "DI1",
      "%MW0:X1"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X1",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo X recuado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI1",
      "Sensor Magnetico, eixo X recuado"
    ]
  },
  "DI2": {
    "id": "DI2",
    "name": "DI2 - Sensor Magnetico, eixo X avancado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Sensor Magnetico, eixo X avancado",
    "statusKey": "DI2",
    "tagNames": [
      "DI2",
      "%MW0:X2"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X2",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo X avancado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI2",
      "Sensor Magnetico, eixo X avancado"
    ]
  },
  "DI3": {
    "id": "DI3",
    "name": "DI3 - Sensor Magnetico, eixo Y recuado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Y recuado",
    "statusKey": "DI3",
    "tagNames": [
      "DI3",
      "%MW0:X3"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X3",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Y recuado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI3",
      "Sensor Magnetico, eixo Y recuado"
    ]
  },
  "DI4": {
    "id": "DI4",
    "name": "DI4 - Sensor Magnetico, eixo Y avancado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Y avancado",
    "statusKey": "DI4",
    "tagNames": [
      "DI4",
      "%MW0:X4"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X4",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Y avancado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI4",
      "Sensor Magnetico, eixo Y avancado"
    ]
  },
  "DI5": {
    "id": "DI5",
    "name": "DI5 - Sensor Magnetico, eixo Z recuado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Z recuado",
    "statusKey": "DI5",
    "tagNames": [
      "DI5",
      "%MW0:X5"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X5",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Z recuado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI5",
      "Sensor Magnetico, eixo Z recuado"
    ]
  },
  "DI6": {
    "id": "DI6",
    "name": "DI6 - Sensor Magnetico, eixo Z avancado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Z avancado",
    "statusKey": "DI6",
    "tagNames": [
      "DI6",
      "%MW0:X6"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X6",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Z avancado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI6",
      "Sensor Magnetico, eixo Z avancado"
    ]
  },
  "DI7": {
    "id": "DI7",
    "name": "DI7 - Sensor Indutivo",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Sensor Indutivo",
    "statusKey": "DI7",
    "tagNames": [
      "DI7",
      "%MW0:X7"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X7",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Indutivo"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI7",
      "Sensor Indutivo"
    ]
  },
  "DI8": {
    "id": "DI8",
    "name": "DI8 - Sensor Otico Reflexivo",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Sensor Otico Reflexivo",
    "statusKey": "DI8",
    "tagNames": [
      "DI8",
      "%MW0:X8"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X8",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Otico Reflexivo"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI8",
      "Sensor Otico Reflexivo"
    ]
  },
  "DI9": {
    "id": "DI9",
    "name": "DI9 - Sensor Otico com Espelho Refletor",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Sensor Otico com Espelho Refletor",
    "statusKey": "DI9",
    "tagNames": [
      "DI9",
      "%MW0:X9"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X9",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Otico com Espelho Refletor"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI9",
      "Sensor Otico com Espelho Refletor"
    ]
  },
  "DI10": {
    "id": "DI10",
    "name": "DI10 - Sensor Otico com Espelho Refletor",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Sensor Otico com Espelho Refletor",
    "statusKey": "DI10",
    "tagNames": [
      "DI10",
      "%MW0:X10"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X10",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Otico com Espelho Refletor"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI10",
      "Sensor Otico com Espelho Refletor"
    ]
  },
  "DI11": {
    "id": "DI11",
    "name": "DI11 - Sensor Capacitivo, peca no slot de saida",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Capacitivo, peca no slot de saida",
    "statusKey": "DI11",
    "tagNames": [
      "DI11",
      "%MW0:X11"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X11",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Capacitivo, peca no slot de saida"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI11",
      "Sensor Capacitivo, peca no slot de saida"
    ]
  },
  "DI12": {
    "id": "DI12",
    "name": "DI12 - Sensor Magnetico, eixo X recuado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Sensor Magnetico, eixo X recuado",
    "statusKey": "DI12",
    "tagNames": [
      "DI12",
      "%MW0:X12"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X12",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo X recuado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI12",
      "Sensor Magnetico, eixo X recuado"
    ]
  },
  "DI13": {
    "id": "DI13",
    "name": "DI13 - Sensor Magnetico, eixo X avancado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Sensor Magnetico, eixo X avancado",
    "statusKey": "DI13",
    "tagNames": [
      "DI13",
      "%MW0:X13"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X13",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo X avancado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI13",
      "Sensor Magnetico, eixo X avancado"
    ]
  },
  "DI14": {
    "id": "DI14",
    "name": "DI14 - Sensor Magnetico, eixo Y recuado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Y recuado",
    "statusKey": "DI14",
    "tagNames": [
      "DI14",
      "%MW0:X14"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X14",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Y recuado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI14",
      "Sensor Magnetico, eixo Y recuado"
    ]
  },
  "DI15": {
    "id": "DI15",
    "name": "DI15 - Sensor Magnetico, eixo Y avancado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Y avancado",
    "statusKey": "DI15",
    "tagNames": [
      "DI15",
      "%MW0:X15"
    ],
    "modbusAddress": "40001.0",
    "virtualAddress": "%MW0:X15",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Y avancado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI15",
      "Sensor Magnetico, eixo Y avancado"
    ]
  },
  "DI16": {
    "id": "DI16",
    "name": "DI16 - Sensor Magnetico, eixo Z recuado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Z recuado",
    "statusKey": "DI16",
    "tagNames": [
      "DI16",
      "%MW1:X0"
    ],
    "modbusAddress": "40002.0",
    "virtualAddress": "%MW1:X0",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Z recuado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI16",
      "Sensor Magnetico, eixo Z recuado"
    ]
  },
  "DI17": {
    "id": "DI17",
    "name": "DI17 - Sensor Magnetico, eixo Z avancado",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Sensor Magnetico, eixo Z avancado",
    "statusKey": "DI17",
    "tagNames": [
      "DI17",
      "%MW1:X1"
    ],
    "modbusAddress": "40002.0",
    "virtualAddress": "%MW1:X1",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Sensor Magnetico, eixo Z avancado"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI17",
      "Sensor Magnetico, eixo Z avancado"
    ]
  },
  "DI20": {
    "id": "DI20",
    "name": "DI20 - Botao Emergencia",
    "type": "Entrada digital",
    "category": "sensor",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Botao Emergencia",
    "statusKey": "DI20",
    "tagNames": [
      "DI20",
      "%MW1:X4"
    ],
    "modbusAddress": "40002.0",
    "virtualAddress": "%MW1:X4",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Botao Emergencia"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DI20",
      "Botao Emergencia"
    ]
  },
  "DO0": {
    "id": "DO0",
    "name": "DO0 - Desloca eixo X",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Desloca eixo X",
    "statusKey": "DO0",
    "tagNames": [
      "DO0",
      "%MW2:X0"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X0",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Desloca eixo X"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO0",
      "Desloca eixo X"
    ]
  },
  "DO1": {
    "id": "DO1",
    "name": "DO1 - Desloca eixo Y",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Desloca eixo Y",
    "statusKey": "DO1",
    "tagNames": [
      "DO1",
      "%MW2:X1"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X1",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Desloca eixo Y"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO1",
      "Desloca eixo Y"
    ]
  },
  "DO2": {
    "id": "DO2",
    "name": "DO2 - Desloca eixo Z",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Desloca eixo Z",
    "statusKey": "DO2",
    "tagNames": [
      "DO2",
      "%MW2:X2"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X2",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Desloca eixo Z"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO2",
      "Desloca eixo Z"
    ]
  },
  "DO3": {
    "id": "DO3",
    "name": "DO3 - Aciona ventosa",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Aciona ventosa",
    "statusKey": "DO3",
    "tagNames": [
      "DO3",
      "%MW2:X3"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X3",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Aciona ventosa"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO3",
      "Aciona ventosa"
    ]
  },
  "DO4": {
    "id": "DO4",
    "name": "DO4 - Esteira avanca",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Esteira avanca",
    "statusKey": "DO4",
    "tagNames": [
      "DO4",
      "%MW2:X4"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X4",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Esteira avanca"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO4",
      "Esteira avanca"
    ]
  },
  "DO5": {
    "id": "DO5",
    "name": "DO5 - Esteira recua",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Esteira recua",
    "statusKey": "DO5",
    "tagNames": [
      "DO5",
      "%MW2:X5"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X5",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Esteira recua"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO5",
      "Esteira recua"
    ]
  },
  "DO6": {
    "id": "DO6",
    "name": "DO6 - Avanca cilindro de descarte 1",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Avanca cilindro de descarte 1",
    "statusKey": "DO6",
    "tagNames": [
      "DO6",
      "%MW2:X6"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X6",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Avanca cilindro de descarte 1"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO6",
      "Avanca cilindro de descarte 1"
    ]
  },
  "DO7": {
    "id": "DO7",
    "name": "DO7 - Avanca cilindro de descarte 2",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Avanca cilindro de descarte 2",
    "statusKey": "DO7",
    "tagNames": [
      "DO7",
      "%MW2:X7"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X7",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Avanca cilindro de descarte 2"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO7",
      "Avanca cilindro de descarte 2"
    ]
  },
  "DO8": {
    "id": "DO8",
    "name": "DO8 - Desloca eixo X",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Braco esquerdo",
    "description": "Desloca eixo X",
    "statusKey": "DO8",
    "tagNames": [
      "DO8",
      "%MW2:X8"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X8",
    "range": "0/1",
    "detailImageKey": "left",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Desloca eixo X"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO8",
      "Desloca eixo X"
    ]
  },
  "DO9": {
    "id": "DO9",
    "name": "DO9 - Desloca eixo Y",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Desloca eixo Y",
    "statusKey": "DO9",
    "tagNames": [
      "DO9",
      "%MW2:X9"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X9",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Desloca eixo Y"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO9",
      "Desloca eixo Y"
    ]
  },
  "DO10": {
    "id": "DO10",
    "name": "DO10 - Desloca eixo Z",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Braco direito",
    "description": "Desloca eixo Z",
    "statusKey": "DO10",
    "tagNames": [
      "DO10",
      "%MW2:X10"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X10",
    "range": "0/1",
    "detailImageKey": "right",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Desloca eixo Z"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO10",
      "Desloca eixo Z"
    ]
  },
  "DO11": {
    "id": "DO11",
    "name": "DO11 - Aciona ventosa",
    "type": "Saida digital",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Aciona ventosa",
    "statusKey": "DO11",
    "tagNames": [
      "DO11",
      "%MW2:X11"
    ],
    "modbusAddress": "40003.0",
    "virtualAddress": "%MW2:X11",
    "range": "0/1",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Aciona ventosa"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "DO11",
      "Aciona ventosa"
    ]
  },
  "AO0": {
    "id": "AO0",
    "name": "AO0 - Set de velocidade da esteira",
    "type": "Saida analogica",
    "category": "atuador",
    "machine": "Classificador De Peca",
    "area": "Esteira",
    "description": "Set de velocidade da esteira",
    "statusKey": "AO0",
    "tagNames": [
      "AO0",
      "%MW5"
    ],
    "modbusAddress": "40006.0",
    "virtualAddress": "%MW5",
    "range": "0...255",
    "detailImageKey": "conveyor",
    "applications": [
      {
        "machine": "Classificador De Peca",
        "function": "Set de velocidade da esteira"
      }
    ],
    "failures": [],
    "recommendations": [],
    "aliases": [
      "AO0",
      "Set de velocidade da esteira"
    ]
  }
};

const sensors = sensorCatalog;

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
  editorEntry: document.querySelector("#editorEntry"),
  editorToolbar: document.querySelector("#editorToolbar"),
  editorImageTabs: document.querySelector("#editorImageTabs"),
  editorMachineFilter: document.querySelector("#editorMachineFilter"),
  editorNewMarker: document.querySelector("#editorNewMarker"),
  editorSaveLayout: document.querySelector("#editorSaveLayout"),
  editorCopyJson: document.querySelector("#editorCopyJson"),
  editorExportLayout: document.querySelector("#editorExportLayout"),
  editorImportJson: document.querySelector("#editorImportJson"),
  editorExit: document.querySelector("#editorExit"),
  editorStatus: document.querySelector("#editorStatus"),
  editorCanvasHint: document.querySelector("#editorCanvasHint"),
  editorInspector: document.querySelector("#editorInspector"),
  editorSelectedTitle: document.querySelector("#editorSelectedTitle"),
  editorSensorSelect: document.querySelector("#editorSensorSelect"),
  editorPosX: document.querySelector("#editorPosX"),
  editorPosY: document.querySelector("#editorPosY"),
  editorDuplicateMarker: document.querySelector("#editorDuplicateMarker"),
  editorRemoveMarker: document.querySelector("#editorRemoveMarker"),
  editorMarkerDialog: document.querySelector("#editorMarkerDialog"),
  editorMarkerForm: document.querySelector("#editorMarkerForm"),
  editorNewSensor: document.querySelector("#editorNewSensor"),
  editorNewImage: document.querySelector("#editorNewImage"),
  editorCreateMarker: document.querySelector("#editorCreateMarker"),
  editorJsonDialog: document.querySelector("#editorJsonDialog"),
  editorJsonForm: document.querySelector("#editorJsonForm"),
  editorJsonInput: document.querySelector("#editorJsonInput"),
  editorApplyJson: document.querySelector("#editorApplyJson"),
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
let currentImageKey = "overview";
let editorActive = false;
let selectedMarkerId = null;
let editorLayout = loadEditorLayout();
let draggingMarker = null;
let editorMachineFilter = "all";

if (!conversationId) {
  conversationId = crypto.randomUUID ? crypto.randomUUID() : String(Date.now());
  localStorage.setItem("nexa-ai-conversation-id", conversationId);
}

function imageKeyForItem(item) {
  if (item.detailImageKey && IMAGE_CONTEXTS[item.detailImageKey]) return item.detailImageKey;
  if (item.detailImage === SENSOR_IMAGES.left) return "left";
  if (item.detailImage === SENSOR_IMAGES.conveyor) return "conveyor";
  if (item.detailImage === SENSOR_IMAGES.right) return "right";
  return "overview";
}

function markerId(sensorId, imageKey, suffix = "") {
  return `${sensorId}-${imageKey}${suffix}`;
}

function clampPercent(value) {
  return Math.max(0, Math.min(100, Number(value) || 0));
}

function roundPercent(value) {
  return Math.round(clampPercent(value) * 100) / 100;
}

function defaultPositionFor(item, imageKey) {
  if (imageKey === "overview") {
    return item.overviewPosition || { x: 50, y: 50 };
  }

  if (imageKey === imageKeyForItem(item)) {
    return item.position || item.overviewPosition || { x: 50, y: 50 };
  }

  return item.overviewPosition || { x: 50, y: 50 };
}

function createLayoutMarker(sensorId, imageKey, position = null) {
  const item = sensorCatalog[sensorId];
  const coordinates = position || defaultPositionFor(item, imageKey);
  return {
    id: markerId(sensorId, imageKey),
    sensorId,
    image: imageKey,
    x: roundPercent(coordinates.x),
    y: roundPercent(coordinates.y)
  };
}

function createDefaultLayout() {
  return Object.values(sensorCatalog).flatMap((item) =>
    Object.keys(IMAGE_CONTEXTS).map((imageKey) => createLayoutMarker(item.id, imageKey))
  );
}

function normalizeImportedMarker(marker, index) {
  const sensorId = marker.sensorId || marker.id;
  if (!sensorCatalog[sensorId]) {
    return null;
  }

  const imageKey = marker.image || marker.imagem || "overview";
  const image = IMAGE_CONTEXTS[imageKey] ? imageKey : "overview";
  return {
    id: marker.markerId || markerId(sensorId, image, `-${index}`),
    sensorId,
    image,
    x: roundPercent(marker.x),
    y: roundPercent(marker.y)
  };
}

function loadEditorLayout() {
  try {
    const saved = JSON.parse(localStorage.getItem(LAYOUT_STORAGE_KEY) || "null");
    if (Array.isArray(saved) && saved.length > 0) {
      const markers = saved.map(normalizeImportedMarker).filter(Boolean);
      if (markers.length > 0) return markers;
    }
  } catch {
    localStorage.removeItem(LAYOUT_STORAGE_KEY);
  }

  return createDefaultLayout();
}

function saveEditorLayout() {
  localStorage.setItem(LAYOUT_STORAGE_KEY, JSON.stringify(editorLayout));
  setEditorStatus("Layout salvo");
}

function exportEditorLayout() {
  return editorLayout
    .map((marker) => {
      const item = sensorCatalog[marker.sensorId];
      return {
        sensorId: marker.sensorId,
        tipo: item?.type || "",
        area: item?.area || "",
        x: roundPercent(marker.x),
        y: roundPercent(marker.y),
        imagem: marker.image
      };
    })
    .sort((a, b) => `${a.imagem}-${a.sensorId}`.localeCompare(`${b.imagem}-${b.sensorId}`));
}

function markersForCurrentImage() {
  return editorLayout.filter((marker) => marker.image === currentImageKey && sensorCatalog[marker.sensorId]);
}

function selectedMarker() {
  return editorLayout.find((marker) => marker.id === selectedMarkerId) || null;
}

function setEditorStatus(message) {
  els.editorStatus.textContent = message;
  window.clearTimeout(setEditorStatus.timeoutId);
  setEditorStatus.timeoutId = window.setTimeout(() => {
    els.editorStatus.textContent = editorActive ? "Editando layout" : "Layout local";
  }, 2200);
}

function setImageContext(imageKey) {
  currentImageKey = IMAGE_CONTEXTS[imageKey] ? imageKey : "overview";
  const imageContext = IMAGE_CONTEXTS[currentImageKey];
  els.machineImage.src = imageContext.image;
  els.machineImage.alt = imageContext.label;
  els.machineView.classList.toggle("sensor-focus", currentImageKey !== "overview" && !editorActive);
  renderEditorImageTabs();
  renderSensorOverlay();
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

function createSensorMarker(layoutMarker, mode) {
  const sensor = sensorCatalog[layoutMarker.sensorId];
  const state = getSensorState(sensor);
  const element = document.createElement("button");
  element.type = "button";
  element.dataset.markerId = layoutMarker.id;
  element.className = `sensor-marker ${state.key} ${sensor.category?.toLowerCase() || ""} ${mode === "detail" ? "selected" : ""} ${selectedMarkerId === layoutMarker.id ? "editor-selected" : ""}`;
  element.style.left = `${layoutMarker.x}%`;
  element.style.top = `${layoutMarker.y}%`;
  element.setAttribute("aria-label", `${sensor.id} - ${sensor.name}`);
  element.innerHTML = `<span></span><strong>${sensor.id}</strong>`;
  element.addEventListener("pointerdown", (event) => startMarkerDrag(event, layoutMarker.id));
  element.addEventListener("click", (event) => {
    event.stopPropagation();
    if (editorActive) {
      selectEditorMarker(layoutMarker.id);
      return;
    }
    selectSensor(sensor, { fault: state.key === "fault", imageKey: layoutMarker.image });
  });
  return element;
}

function renderSensorOverlay() {
  els.sensorOverlay.innerHTML = "";

  const markers = markersForCurrentImage();

  if (!editorActive && selectedSensor) {
    markers
      .filter((marker) => marker.sensorId === selectedSensor.id)
      .forEach((marker) => els.sensorOverlay.appendChild(createSensorMarker(marker, "detail")));
    return;
  }

  markers.forEach((marker) => {
    els.sensorOverlay.appendChild(createSensorMarker(marker, "overview"));
  });
}

function machineNames() {
  return Array.from(new Set(Object.values(sensorCatalog).flatMap((item) =>
    (item.applications || []).map((application) => application.machine).concat(item.machine || [])
  )))
    .filter(Boolean)
    .sort((a, b) => a.localeCompare(b, "pt-BR"));
}

function itemMatchesMachineFilter(item) {
  if (editorMachineFilter === "all") return true;
  return item.machine === editorMachineFilter ||
    (item.applications || []).some((application) => application.machine === editorMachineFilter);
}

function catalogOptionsHtml() {
  return Object.values(sensorCatalog)
    .filter(itemMatchesMachineFilter)
    .sort((a, b) => a.id.localeCompare(b.id, "pt-BR", { numeric: true }))
    .map((item) => `<option value="${item.id}">${item.id} - ${item.name}</option>`)
    .join("");
}

function imageOptionsHtml() {
  return Object.values(IMAGE_CONTEXTS)
    .map((image) => `<option value="${image.key}">${image.label}</option>`)
    .join("");
}

function populateEditorSelects() {
  const selectedSensorValue = els.editorSensorSelect.value;
  const selectedNewValue = els.editorNewSensor.value;
  const catalogHtml = catalogOptionsHtml();
  els.editorSensorSelect.innerHTML = catalogHtml;
  els.editorNewSensor.innerHTML = catalogHtml;
  els.editorNewImage.innerHTML = imageOptionsHtml();
  if (sensorCatalog[selectedSensorValue]) {
    els.editorSensorSelect.value = selectedSensorValue;
  }
  if (sensorCatalog[selectedNewValue]) {
    els.editorNewSensor.value = selectedNewValue;
  }
}

function populateMachineFilter() {
  els.editorMachineFilter.innerHTML = [
    '<option value="all">Todos</option>',
    ...machineNames().map((machine) => `<option value="${machine}">${machine}</option>`)
  ].join("");
  els.editorMachineFilter.value = editorMachineFilter;
}

function renderEditorImageTabs() {
  if (!els.editorImageTabs) return;
  els.editorImageTabs.innerHTML = "";

  Object.values(IMAGE_CONTEXTS).forEach((image) => {
    const button = document.createElement("button");
    button.type = "button";
    button.textContent = image.label;
    button.className = image.key === currentImageKey ? "active" : "";
    button.addEventListener("click", () => {
      selectedSensor = null;
      selectedSensorFaultIntent = false;
      els.sensorDetailCard.setAttribute("aria-hidden", "true");
      els.sensorDetailCard.classList.remove("fault", "ok");
      setImageContext(image.key);
    });
    els.editorImageTabs.appendChild(button);
  });
}

function updateEditorInspector() {
  const marker = selectedMarker();
  const item = marker ? sensorCatalog[marker.sensorId] : null;
  els.editorInspector.classList.toggle("open", editorActive);
  els.editorInspector.setAttribute("aria-hidden", String(!editorActive));
  els.editorSensorSelect.disabled = !marker;
  els.editorDuplicateMarker.disabled = !marker;
  els.editorRemoveMarker.disabled = !marker;
  els.editorSelectedTitle.textContent = item ? `${item.id} - ${item.name}` : "Nenhum";
  els.editorPosX.textContent = marker ? roundPercent(marker.x).toFixed(2) : "--";
  els.editorPosY.textContent = marker ? roundPercent(marker.y).toFixed(2) : "--";

  if (marker) {
    els.editorSensorSelect.value = marker.sensorId;
  }
}

function selectEditorMarker(markerId) {
  selectedMarkerId = markerId;
  const marker = selectedMarker();
  if (marker) {
    const item = sensorCatalog[marker.sensorId];
    selectedSensor = item;
    currentComponentId = item.componentId;
    renderSensorDetail(item);
  }
  updateEditorInspector();
  renderSensorOverlay();
}

function startMarkerDrag(event, markerId) {
  if (!editorActive) return;
  event.preventDefault();
  event.stopPropagation();
  selectEditorMarker(markerId);
  draggingMarker = {
    markerId,
    pointerId: event.pointerId
  };
  event.currentTarget.setPointerCapture(event.pointerId);
}

function moveDraggingMarker(event) {
  if (!draggingMarker || draggingMarker.pointerId !== event.pointerId) return;
  const marker = editorLayout.find((item) => item.id === draggingMarker.markerId);
  if (!marker) return;

  const rect = els.sensorOverlay.getBoundingClientRect();
  marker.x = roundPercent(((event.clientX - rect.left) / rect.width) * 100);
  marker.y = roundPercent(((event.clientY - rect.top) / rect.height) * 100);
  updateEditorInspector();
  renderSensorOverlay();
}

function stopDraggingMarker(event) {
  if (!draggingMarker || draggingMarker.pointerId !== event.pointerId) return;
  draggingMarker = null;
  saveEditorLayout();
}

function enterEditorMode() {
  const password = window.prompt("Senha do modo editor");
  if (password !== EDITOR_PASSWORD) {
    window.alert("Senha incorreta.");
    return;
  }

  editorActive = true;
  selectedMarkerId = null;
  selectedSensor = null;
  selectedSensorFaultIntent = false;
  els.machineView.classList.add("editor-active");
  els.editorToolbar.classList.add("open");
  els.editorToolbar.setAttribute("aria-hidden", "false");
  els.editorCanvasHint.setAttribute("aria-hidden", "false");
  resetSensorView();
  updateEditorInspector();
  setEditorStatus("Editando layout");
}

function exitEditorMode() {
  editorActive = false;
  draggingMarker = null;
  selectedMarkerId = null;
  els.machineView.classList.remove("editor-active");
  els.editorToolbar.classList.remove("open");
  els.editorToolbar.setAttribute("aria-hidden", "true");
  els.editorCanvasHint.setAttribute("aria-hidden", "true");
  updateEditorInspector();
  resetSensorView();
}

function changeSelectedMarkerSensor(sensorId) {
  const marker = selectedMarker();
  if (!marker || !sensorCatalog[sensorId]) return;
  marker.sensorId = sensorId;
  marker.id = markerId(sensorId, marker.image, `-${Date.now()}`);
  selectedMarkerId = marker.id;
  saveEditorLayout();
  selectEditorMarker(marker.id);
}

function duplicateSelectedMarker() {
  const marker = selectedMarker();
  if (!marker) return;
  const copy = {
    ...marker,
    id: markerId(marker.sensorId, marker.image, `-copy-${Date.now()}`),
    x: roundPercent(marker.x + 2),
    y: roundPercent(marker.y + 2)
  };
  editorLayout.push(copy);
  selectedMarkerId = copy.id;
  saveEditorLayout();
  updateEditorInspector();
  renderSensorOverlay();
}

function removeSelectedMarker() {
  const marker = selectedMarker();
  if (!marker) return;
  editorLayout = editorLayout.filter((item) => item.id !== marker.id);
  selectedMarkerId = null;
  saveEditorLayout();
  updateEditorInspector();
  renderSensorOverlay();
}

function openNewMarkerDialog() {
  els.editorNewSensor.value = Object.keys(sensorCatalog)[0];
  els.editorNewImage.value = currentImageKey;
  els.editorMarkerDialog.showModal();
}

function createNewMarker(sensorId, imageKey) {
  if (!sensorCatalog[sensorId] || !IMAGE_CONTEXTS[imageKey]) return;
  const marker = {
    id: markerId(sensorId, imageKey, `-new-${Date.now()}`),
    sensorId,
    image: imageKey,
    x: 50,
    y: 50
  };
  editorLayout.push(marker);
  setImageContext(imageKey);
  selectedMarkerId = marker.id;
  saveEditorLayout();
  updateEditorInspector();
  renderSensorOverlay();
}

async function copyLayoutJson() {
  const json = JSON.stringify(exportEditorLayout(), null, 2);
  await navigator.clipboard.writeText(json);
  setEditorStatus("JSON copiado");
}

function exportLayoutFile() {
  const json = JSON.stringify(exportEditorLayout(), null, 2);
  const blob = new Blob([json], { type: "application/json" });
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = url;
  link.download = "simmaq-nexa-layout.json";
  link.click();
  URL.revokeObjectURL(url);
  setEditorStatus("Layout exportado");
}

function openImportDialog() {
  els.editorJsonInput.value = "";
  els.editorJsonDialog.showModal();
}

function importLayoutFromJson(value) {
  const parsed = JSON.parse(value);
  if (!Array.isArray(parsed)) {
    throw new Error("JSON deve ser uma lista de marcadores.");
  }

  const imported = parsed.map(normalizeImportedMarker).filter(Boolean);
  if (imported.length === 0) {
    throw new Error("Nenhum marcador valido encontrado.");
  }

  editorLayout = imported;
  selectedMarkerId = null;
  saveEditorLayout();
  updateEditorInspector();
  renderSensorOverlay();
}

function renderSensorDetail(sensor) {
  const state = getSensorState(sensor);
  const hasDescription = sensor.description && sensor.description !== "Informacoes tecnicas ainda nao cadastradas.";
  const applicationSummary = (sensor.applications || [])
    .map((item) => `${item.machine}: ${item.function}`)
    .join(" | ");
  els.sensorDetailArea.textContent = sensor.area;
  els.sensorDetailTitle.textContent = sensor.id;
  els.sensorDetailType.textContent = sensor.type;
  els.sensorDetailStatus.textContent = state.label;
  els.sensorDetailDescription.textContent = hasDescription
    ? `${sensor.description}${applicationSummary ? ` (${applicationSummary})` : ""}`
    : "Informações técnicas ainda não cadastradas.";
  els.sensorDetailFailures.innerHTML = sensor.failures?.length
    ? sensor.failures.map((item) => `<li>${item}</li>`).join("")
    : "<li>Informações técnicas ainda não cadastradas.</li>";
  els.sensorDetailRecommendations.innerHTML = sensor.recommendations?.length
    ? sensor.recommendations.map((item) => `<li>${item}</li>`).join("")
    : "<li>Informações técnicas ainda não cadastradas.</li>";
  els.sensorDetailCard.classList.toggle("fault", state.key === "fault");
  els.sensorDetailCard.classList.toggle("ok", state.key === "ok");
  els.sensorDetailCard.setAttribute("aria-hidden", "false");
}

function selectSensor(sensor, options = {}) {
  selectedSensor = sensor;
  selectedSensorFaultIntent = Boolean(options.fault);
  currentComponentId = sensor.componentId;

  currentImageKey = IMAGE_CONTEXTS[options.imageKey] ? options.imageKey : imageKeyForItem(sensor);
  els.machineImage.src = IMAGE_CONTEXTS[currentImageKey].image;
  els.machineImage.alt = `${sensor.area} - ${sensor.name}`;
  els.machineView.classList.add("sensor-focus");
  els.machineView.classList.toggle("fault", getSensorState(sensor).key === "fault");
  renderSensorDetail(sensor);
  renderEditorImageTabs();
  renderSensorOverlay();
}

function resetSensorView() {
  selectedSensor = null;
  selectedSensorFaultIntent = false;
  currentImageKey = "overview";
  els.machineImage.src = IMAGE_CONTEXTS.overview.image;
  els.machineImage.alt = "Bancada SIMMAQ NXA";
  els.machineView.classList.remove("sensor-focus", "fault");
  els.sensorDetailCard.classList.remove("fault", "ok");
  els.sensorDetailCard.setAttribute("aria-hidden", "true");
  renderEditorImageTabs();
  renderSensorOverlay();
}

function buildSensorQuestion(question, sensor) {
  const state = getSensorState(sensor);
  const applications = (sensor.applications || [])
    .map((item) => `${item.machine}: ${item.function}`)
    .join("; ");
  return `O usuario perguntou sobre o sensor ${sensor.id}. Pergunta original: "${question}". ` +
    `Contexto oficial do cadastro central: id=${sensor.id}; categoria=${sensor.category}; nome=${sensor.name}; tipo=${sensor.type}; maquina=${sensor.machine}; area=${sensor.area}; statusKey=${sensor.statusKey}; tags=${sensor.tagNames.join(", ")}; endereco modbus=${sensor.modbusAddress || "nao informado"}; endereco virtual=${sensor.virtualAddress || "nao informado"}; status atual=${state.label}; funcao=${sensor.description}; aplicacoes=${applications || "nao cadastradas"}; ` +
    `possiveis falhas=${sensor.failures?.join(", ") || "nao cadastradas"}; recomendacoes=${sensor.recommendations?.join(", ") || "nao cadastradas"}. ` +
    "Responda em portugues, direto e tecnico, explicando funcao, localizacao, status atual e o que verificar.";
}

function isMachineQuestion(message) {
  const normalized = normalizeText(message);
  return /MAQUINA|BANCADA|CLASSIFICADOR|CICLO|PROCESSO/.test(normalized) &&
    !Object.keys(sensorCatalog).some((sensorId) => new RegExp(`(^|\\W)${sensorId}(\\W|$)`).test(normalized));
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
  const linkedSensor = isMachineQuestion(question)
    ? null
    : detectedSensor || selectedSensor || sensorFromComponent(currentComponentId);
  if (linkedSensor) {
    selectSensor(linkedSensor, { fault: isFaultQuestion(question) });
  }

  addMessage("user", question);
  const thinkingMessage = addThinkingMessage();
  setAssistantBusy(true);

  try {
    const response = await fetch(`${API_BASE}/api/assistant/ask`, {
      method: "POST",
      headers: { "Content-Type": "application/json; charset=utf-8" },
      body: JSON.stringify({
        question: linkedSensor ? buildSensorQuestion(question, linkedSensor) : question,
        conversationId,
        componentId: linkedSensor?.componentId || (isMachineQuestion(question) ? null : currentComponentId),
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
    const actuators = data.actuators || [];
    sensorStatusByTag = Object.fromEntries([...sensors, ...actuators].map((tag) => [tag.name, tag]));
    activeFaultSourceTags = new Set((data.pendingDiagnostics || []).map((diagnostic) => diagnostic.sourceTag).filter(Boolean));
    renderTagList(els.sensorsList, sensors);
    renderTagList(els.actuatorsList, actuators);
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

els.editorEntry.addEventListener("click", enterEditorMode);
els.editorNewMarker.addEventListener("click", openNewMarkerDialog);
els.editorSaveLayout.addEventListener("click", saveEditorLayout);
els.editorMachineFilter.addEventListener("change", () => {
  editorMachineFilter = els.editorMachineFilter.value;
  populateEditorSelects();
});
els.editorCopyJson.addEventListener("click", () => {
  copyLayoutJson().catch(() => setEditorStatus("Nao foi possivel copiar"));
});
els.editorExportLayout.addEventListener("click", exportLayoutFile);
els.editorImportJson.addEventListener("click", openImportDialog);
els.editorExit.addEventListener("click", exitEditorMode);
els.editorSensorSelect.addEventListener("change", () => changeSelectedMarkerSensor(els.editorSensorSelect.value));
els.editorDuplicateMarker.addEventListener("click", duplicateSelectedMarker);
els.editorRemoveMarker.addEventListener("click", removeSelectedMarker);
window.addEventListener("pointermove", moveDraggingMarker);
window.addEventListener("pointerup", stopDraggingMarker);
window.addEventListener("pointercancel", stopDraggingMarker);
els.sensorOverlay.addEventListener("click", () => {
  if (!editorActive) return;
  selectedMarkerId = null;
  updateEditorInspector();
  renderSensorOverlay();
});

els.editorMarkerForm.addEventListener("submit", (event) => {
  if (event.submitter?.value === "cancel") return;
  event.preventDefault();
  createNewMarker(els.editorNewSensor.value, els.editorNewImage.value);
  els.editorMarkerDialog.close();
});

els.editorJsonForm.addEventListener("submit", (event) => {
  if (event.submitter?.value === "cancel") return;
  event.preventDefault();
  try {
    importLayoutFromJson(els.editorJsonInput.value);
    els.editorJsonDialog.close();
    setEditorStatus("Layout importado");
  } catch (error) {
    window.alert(error.message || "JSON invalido.");
  }
});

els.assistantButton.addEventListener("click", () => {
  els.assistantDrawer.classList.add("open");
  els.assistantDrawer.setAttribute("aria-hidden", "false");
  document.body.classList.add("assistant-open");

  if (pendingDiagnostics.length > 0) {
    const main = pendingDiagnostics[0];
    addMessage("assistant", `${main.title}\n\n${main.description}\n\nAcao recomendada: ${main.recommendedActions}`);
  }
});

els.assistantClose.addEventListener("click", () => {
  els.assistantDrawer.classList.remove("open");
  els.assistantDrawer.setAttribute("aria-hidden", "true");
  document.body.classList.remove("assistant-open");
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
populateMachineFilter();
populateEditorSelects();
renderEditorImageTabs();
updateEditorInspector();
renderSensorOverlay();
loadComponentMap();
loadDashboard();
setInterval(tickClock, 1000);
setInterval(loadDashboard, 2000);

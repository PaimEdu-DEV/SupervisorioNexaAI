# Nexa AI - CLASSIFICADOR DE PECA

Este documento descreve somente a maquina **CLASSIFICADOR DE PECA** da planilha de enderecos SIMMAQ para CLP Schneider M221. Outras maquinas da planilha, como portao, elevador, controle de nivel/temperatura e semaforo, devem ser ignoradas pela Nexa AI.

## Objetivo

A bancada recebe uma peca no slot de entrada, movimenta a peca com eixos pneumaticos e ventosa, transporta pela esteira, identifica caracteristicas por sensores indutivo e opticos, e separa a peca por cilindros de descarte conforme a decisao do CLP.

## Fluxo Operacional

1. Operador coloca a peca no slot de entrada.
2. `DI0` confirma peca no slot de entrada.
3. O ciclo e solicitado por `DI18` quando a maquina esta pronta.
4. Os eixos X, Y e Z movimentam a peca usando comandos `DO0`, `DO1`, `DO2` ou o segundo grupo `DO8`, `DO9`, `DO10`.
5. A ventosa e acionada por `DO3` ou `DO11` para pegar ou manter a peca.
6. A esteira avanca por `DO4` ou recua por `DO5`.
7. `DI7` verifica caracteristica metalica da peca.
8. `DI8`, `DI9` e `DI10` fazem leitura optica na area de classificacao.
9. O CLP decide a classificacao da peca.
10. `DO6` ou `DO7` avanca o cilindro de descarte correspondente.
11. `DI11` confirma peca no slot de saida.
12. O ciclo termina e a bancada volta a uma condicao segura.

## Entradas Digitais

| ID | Endereco virtual | Funcao |
| --- | --- | --- |
| DI0 | `%MW0:X0` | Sensor capacitivo, peca no slot de entrada |
| DI1 | `%MW0:X1` | Sensor magnetico, eixo X recuado |
| DI2 | `%MW0:X2` | Sensor magnetico, eixo X avancado |
| DI3 | `%MW0:X3` | Sensor magnetico, eixo Y recuado |
| DI4 | `%MW0:X4` | Sensor magnetico, eixo Y avancado |
| DI5 | `%MW0:X5` | Sensor magnetico, eixo Z recuado |
| DI6 | `%MW0:X6` | Sensor magnetico, eixo Z avancado |
| DI7 | `%MW0:X7` | Sensor indutivo |
| DI8 | `%MW0:X8` | Sensor otico reflexivo |
| DI9 | `%MW0:X9` | Sensor otico com espelho refletor |
| DI10 | `%MW0:X10` | Sensor otico com espelho refletor |
| DI11 | `%MW0:X11` | Sensor capacitivo, peca no slot de saida |
| DI12 | `%MW0:X12` | Sensor magnetico, eixo X recuado |
| DI13 | `%MW0:X13` | Sensor magnetico, eixo X avancado |
| DI14 | `%MW0:X14` | Sensor magnetico, eixo Y recuado |
| DI15 | `%MW0:X15` | Sensor magnetico, eixo Y avancado |
| DI16 | `%MW1:X0` | Sensor magnetico, eixo Z recuado |
| DI17 | `%MW1:X1` | Sensor magnetico, eixo Z avancado |
| DI18 | `%MW1:X2` | Botao inicio |
| DI19 | `%MW1:X3` | Botao reset |
| DI20 | `%MW1:X4` | Botao emergencia |

## Saidas Digitais

| ID | Endereco virtual | Funcao |
| --- | --- | --- |
| DO0 | `%MW2:X0` | Desloca eixo X |
| DO1 | `%MW2:X1` | Desloca eixo Y |
| DO2 | `%MW2:X2` | Desloca eixo Z |
| DO3 | `%MW2:X3` | Aciona ventosa |
| DO4 | `%MW2:X4` | Esteira avanca |
| DO5 | `%MW2:X5` | Esteira recua |
| DO6 | `%MW2:X6` | Avanca cilindro de descarte 1 |
| DO7 | `%MW2:X7` | Avanca cilindro de descarte 2 |
| DO8 | `%MW2:X8` | Desloca eixo X |
| DO9 | `%MW2:X9` | Desloca eixo Y |
| DO10 | `%MW2:X10` | Desloca eixo Z |
| DO11 | `%MW2:X11` | Aciona ventosa |

## Sinal Analogico

| ID | Endereco virtual | Funcao |
| --- | --- | --- |
| AO0 | `%MW5` | Set de velocidade da esteira |

## Diagnosticos Base

- Se `DI20` estiver ativo, a IA deve explicar que a emergencia bloqueia os atuadores e que a causa precisa ser removida antes do reset.
- Se `DI7` detectar peca e `DO4` estiver inativo durante transporte, verificar emergencia, modo manual, intertravamentos, comando da esteira e motor/acionamento.
- Se sensores recuado e avancado do mesmo eixo ficarem ativos ao mesmo tempo, suspeitar de sensor travado, curto no sinal, ima fora de posicao ou erro de parametrizacao.
- Se uma saida de eixo estiver acionada e o fim de curso esperado nao mudar, verificar valvula, pressao pneumatica, cilindro, cabo, sensor magnetico e entrada digital do CLP.
- Se sensor optico nao acionar, limpar lente/refletor, conferir alinhamento, verificar alimentacao e testar a entrada correspondente no CLP.
- Se sensor capacitivo nao detectar peca, verificar distancia de deteccao, material da peca, posicionamento, cabo e alimentacao.

## Regra Para Respostas Da IA

A Nexa AI deve combinar tres fontes: conhecimento da maquina, estado atual do CLP/MQTT e componente selecionado. Ela nao deve responder apenas "ativo" ou "inativo"; deve explicar o papel do componente no ciclo e orientar o diagnostico como um tecnico de automacao.

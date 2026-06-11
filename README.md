# Máquinas Abstratas — AFD, Autômato de Pilha e Máquina de Turing

Trabalho final de **Fundamentos Teóricos da Computação (FTC)**. Implementa, em C# (.NET 9), três máquinas abstratas fundamentais da teoria da computação, cada uma como tradução direta da sua definição formal.

## Integrantes da equipe

| Nome completo | Matrícula |
|---------------|-----------|
| Matheus Felipe de Almeida Mota   | 72301031|
| Pedro Simão do Carmo Diniz  | 72301376 |
| Yuri Leal de Sousa  | 72301341 |

## Estrutura do repositório

```
.
├── Parte1/                          # Simulador genérico de AFD (L1: cadeias terminadas em "ab")
│   ├── Models/                      # Estruturas que espelham a definição formal
│   │   ├── AutomatoFinitoDeterministico.cs   # A 5-tupla (Q, Σ, δ, q0, F)
│   │   └── ResultadoSimulacao.cs             # Resultado + rastro de estados
│   ├── Services/                    # Lógica de simulação e construção
│   │   ├── AfdFactory.cs                      # Constrói o AFD de L1
│   │   ├── AfdConfiguracao.cs                 # DTOs do esquema afd.json
│   │   └── AfdJsonLoader.cs                   # Carrega um AFD genérico do JSON
│   ├── Dados/                       # Arquivos de entrada
│   │   ├── entradas.txt
│   │   └── afd.json
│   ├── Program.cs                   # Camada de aplicação (entrada/saída)
│   └── Parte1.csproj
│
├── Parte2/                          # Autômato de Pilha por pilha vazia (L2: aⁿbⁿ; L3: palíndromos)
│   ├── Models/
│   │   ├── AutomatoPilha.cs                   # A 7-tupla do AP
│   │   ├── TransicaoPilha.cs                  # Destino de uma transição (estado + empilhar)
│   │   ├── ConfiguracaoInstantanea.cs         # Snapshot (estado, entrada, pilha)
│   │   └── ResultadoSimulacaoPilha.cs         # Resultado + caminho percorrido
│   ├── Services/
│   │   ├── ApFactory.cs                        # Constrói os APs de L2 e L3
│   │   └── SimuladorPilha.cs                   # Simulação não determinística (DFS)
│   ├── Dados/
│   │   ├── entradas_ap.txt                     # Casos de L2
│   │   └── entradas_l3.txt                     # Casos de L3
│   ├── Program.cs
│   └── Parte2.csproj
|
├── Parte3/                          # Máquina de Turing (L4: aⁿbⁿcⁿ; f(n)=n+1 unário)
│   ├── Models/
│   │   ├── MaquinaTuring.cs                    # A 7-tupla da MT
│   │   ├── TransicaoTuring.cs                  # (novoEstado, novoSímbolo, direção)
│   │   ├── Fita.cs                             # Fita dinâmica (Dictionary<int,char>)
│   │   └── ResultadoExecucao.cs               # Status, passos e fita final
│   ├── Services/
│   │   ├── MaquinaTuringFactory.cs            # Constrói a MT de L4 e a de f(n)=n+1
│   │   └── SimuladorTuring.cs                 # Simulação passo a passo com limite
│   ├── Dados/
│   │   ├── entradas_mt.txt                     # Casos de L4
│   │   └── entradas_incremento.txt            # Casos de f(n)=n+1
│   ├── Program.cs
│   └── Parte3.csproj
│
├── docs/                            # Relatório técnico
│   ├── relatorio.tex                # Fonte LaTeX do relatório (padrão ABNT)
│   └── relatorio.pdf                # Relatório compilado
│
├── .gitignore
└── README.md
```

Cada parte segue uma separação em camadas:

- **Models/** — estruturas que espelham a definição matemática da máquina (a 5/7-tupla, fita, pilha, configurações e resultados).
- **Services/** — lógica de simulação, carregamento de configuração e construção das máquinas (factories).
- **Dados/** — arquivos de entrada (`.txt`) e de configuração (`afd.json`) lidos por cada projeto.
- **Program.cs** — camada de aplicação (entrada/saída e orquestração).

## Pré-requisitos

- **.NET 6 ou superior** (SDK). Os projetos miram `net6.0` e usam `<RollForward>Major</RollForward>`, então compilam com qualquer SDK ≥ 6 e executam em qualquer runtime instalado (6, 7, 8, 9, 10...), mesmo que o runtime exato do alvo não esteja presente.

## Como compilar e executar

A partir da raiz do repositório:

```bash
# Parte 1 — Autômato Finito Determinístico (L1)
dotnet run --project Parte1

# Parte 2 — Autômato de Pilha (L2 e desafio L3)
dotnet run --project Parte2

# Parte 3 — Máquina de Turing (L4 e desafio f(n) = n + 1)
dotnet run --project Parte3

# Parte 3 com limite de passos customizado (padrão: 1000)
dotnet run --project Parte3 -- 5000
```

Cada projeto lê seus casos de teste de arquivos na respectiva pasta `Dados/`:

| Parte | Arquivos de entrada |
|-------|---------------------|
| Parte1 | `entradas.txt`, `afd.json` (AFD genérico via JSON) |
| Parte2 | `entradas_ap.txt` (L2), `entradas_l3.txt` (L3) |
| Parte3 | `entradas_mt.txt` (L4), `entradas_incremento.txt` (f(n)=n+1) |


## Resumo de cada parte

### Parte 1 — AFD
Reconhece `L1 = { w ∈ {a,b}* | w termina com "ab" }`. Modela a 5-tupla `(Q, Σ, δ, q0, F)`, com `δ` como `Dictionary<(string,char), string>`. Exibe o diagrama de transições (tabela `δ` com molduras), simula cada cadeia em uma **tabela-resumo** alinhada (colunas `CADEIA`, `STATUS` — `ACEITA`/`REJEITA` colorido — e `RASTRO`) com uma linha final de totais, e ainda carrega qualquer AFD a partir de `afd.json`.

> Observação: a cadeia `aab` é **ACEITA** (termina com o sufixo `ab`), conforme a definição formal de `L1`. A tabela de casos de teste do enunciado sugere `REJEITA` para `aab`, o que é inconsistente com a própria definição; a implementação segue a definição. Detalhes na "Nota sobre `aab`" do relatório.

### Parte 2 - AP
Reconhece `L2 = { aⁿbⁿ | n ≥ 1 }` **por pilha vazia**, com a pilha implementada como `Stack<char>` e λ-movimentos (`'\0'`). Para cada cadeia exibe o status (`ACEITA`/`REJEITA` colorido) e a sequência de configurações instantâneas em uma **tabela com molduras** (colunas `PASSO`, `ESTADO`, `ENTRADA`, `PILHA (topo->base)` e `TRANSICAO`), encerrando cada linguagem com uma linha de totais. O desafio adiciona um AP não determinístico para `L3 = palíndromos sobre {a,b}`.

### Parte 3 — Máquina de Turing
Reconhece `L4 = { aⁿbⁿcⁿ | n ≥ 1 }` usando uma fita dinâmica (`Dictionary<int,char>`) e a estratégia de marcação `X/Y/Z`. Exibe a execução passo a passo em uma **tabela com molduras** (colunas `PASSO`, `ESTADO`, `CABECOTE` e `FITA` — esta com o símbolo sob o cabeçote entre colchetes), com contador e limite de passos, status colorido (`ACEITA`/`REJEITA`/`LIMITE`) e uma linha de totais ao final. O desafio implementa uma segunda MT que **computa** `f(n) = n + 1` em unário.

## Vídeo de defesa

Link: [TODO] gravar e editar vídeo

## Relatório técnico

O relatório completo está em [`docs/relatorio.pdf`](docs/relatorio.pdf). O fonte LaTeX ([`docs/relatorio.tex`](docs/relatorio.tex)) também está versionado no repositório.

TODOS:

- Gravar Vídeo

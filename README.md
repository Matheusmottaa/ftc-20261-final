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
├── docs/                            # Relatório técnico
│   └── relatorio.pdf                # Relatório compilado (TODO A ser feito)
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

- Desenvolvido com .NET 9

## Como compilar e executar

A partir da raiz do repositório:

```bash
# Parte 1 — Autômato Finito Determinístico (L1)
dotnet run --project Parte1

Parte 3 — Máquina de Turing (L4 e desafio f(n) = n + 1)
dotnet run --project Parte3
```

Cada projeto lê seus casos de teste de arquivos na respectiva pasta `Dados/`:

| Parte | Arquivos de entrada |
|-------|---------------------|
| Parte1 | `entradas.txt`, `afd.json` (AFD genérico via JSON) |

## Resumo de cada parte

### Parte 1 — AFD
Reconhece `L1 = { w ∈ {a,b}* | w termina com "ab" }`. Modela a 5-tupla `(Q, Σ, δ, q0, F)`, com `δ` como `Dictionary<(string,char), string>`. Exibe o diagrama de transições, simula cada cadeia mostrando o rastro de estados e ainda carrega qualquer AFD a partir de `afd.json`.

### Parte 2 - AP
Reconhece L2 = { aⁿbⁿ | n ≥ 1 } por pilha vazia, com a pilha implementada como Stack<char> e λ-movimentos ('\0'). Exibe a configuração instantânea (estado, entrada restante, conteúdo da pilha) a cada passo. O desafio adiciona um AP não determinístico para L3 = palíndromos sobre {a,b}.

### Parte 3 — Máquina de Turing
Reconhece L4 = { aⁿbⁿcⁿ | n ≥ 1 } usando uma fita dinâmica (Dictionary<int,char>) e a estratégia de marcação X/Y/Z. Exibe estado, fita completa (com o cabeçote entre colchetes) e posição a cada passo, com contador e limite de passos. O desafio implementa uma segunda MT que computa f(n) = n + 1 em unário.

## Vídeo de defesa

Link: [TODO] gravar e editar vídeo

## Relatório técnico

O relatório completo está em : [TODO: Fazer Relatório] 


TODOS: 

- Gravar Vídeo 
- Fazer Relatório 
- Implementar Automato de pilha
- Implementar Máquina de Turing

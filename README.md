# Máquinas Abstratas — AFD, Autômato de Pilha e Máquina de Turing

Trabalho final de **Fundamentos Teóricos da Computação (FTC)**. Implementa, em C# (.NET 9), três máquinas abstratas fundamentais da teoria da computação, cada uma como tradução direta da sua definição formal.

## Integrantes da equipe

| Nome completo | Matrícula |
|---------------|-----------|
| Matheus Felipe de Almeida Mota   | 72301031|
| _preencher_   | _preencher_ |
| _preencher_   | _preencher_ |

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

```

Cada projeto lê seus casos de teste de arquivos na respectiva pasta `Dados/`:

| Parte | Arquivos de entrada |
|-------|---------------------|
| Parte1 | `entradas.txt`, `afd.json` (AFD genérico via JSON) |

## Resumo de cada parte

### Parte 1 — AFD
Reconhece `L1 = { w ∈ {a,b}* | w termina com "ab" }`. Modela a 5-tupla `(Q, Σ, δ, q0, F)`, com `δ` como `Dictionary<(string,char), string>`. Exibe o diagrama de transições, simula cada cadeia mostrando o rastro de estados e ainda carrega qualquer AFD a partir de `afd.json`.

## Vídeo de defesa

Link: [TODO] gravar e editar vídeo

## Relatório técnico

O relatório completo está em : [TODO: Fazer Relatório] 


TODOS: 

- Gravar Vídeo 
- Fazer Relatório 
- Implementar Automato de pilha
- Implementar Máquina de Turing

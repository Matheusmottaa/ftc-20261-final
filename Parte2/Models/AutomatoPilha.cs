namespace Parte2.Models;

/// <summary>
/// Automato de Pilha modelado como a 7-tupla M = (Q, Sigma, Gamma, delta, q0, Z0, /).
/// O conjunto de estados de aceitacao e vazio: a aceitacao e exclusivamente por
/// pilha vazia.
/// </summary>
public sealed class AutomatoPilha
{
    /// <summary>Simbolo usado para representar lambda-movimentos (vazio).</summary>
    public const char Lambda = '\0';

    /// <summary>Q - conjunto finito de estados.</summary>
    public IReadOnlySet<string> Estados { get; }

    /// <summary>Sigma - alfabeto de entrada.</summary>
    public IReadOnlySet<char> Alfabeto { get; }

    /// <summary>Gamma - alfabeto da pilha.</summary>
    public IReadOnlySet<char> AlfabetoPilha { get; }

    /// <summary>
    /// delta - funcao de transicao (estado, entrada ou lambda, topo da pilha)
    /// para um conjunto de pares (novo estado, simbolos a empilhar).
    /// </summary>
    public IReadOnlyDictionary<(string Estado, char Entrada, char Topo), List<TransicaoPilha>> Transicao { get; }

    /// <summary>q0 - estado inicial.</summary>
    public string EstadoInicial { get; }

    /// <summary>Z0 - simbolo inicial da pilha.</summary>
    public char SimboloInicialPilha { get; }

    public AutomatoPilha(
        IReadOnlySet<string> estados,
        IReadOnlySet<char> alfabeto,
        IReadOnlySet<char> alfabetoPilha,
        IReadOnlyDictionary<(string, char, char), List<TransicaoPilha>> transicao,
        string estadoInicial,
        char simboloInicialPilha)
    {
        Estados = estados;
        Alfabeto = alfabeto;
        AlfabetoPilha = alfabetoPilha;
        Transicao = transicao;
        EstadoInicial = estadoInicial;
        SimboloInicialPilha = simboloInicialPilha;
    }

    /// <summary>Retorna as transicoes aplicaveis para a chave dada, ou lista vazia.</summary>
    public IReadOnlyList<TransicaoPilha> ObterTransicoes(string estado, char entrada, char topo)
    {
        List<TransicaoPilha> lista;
        if (Transicao.TryGetValue((estado, entrada, topo), out lista))
        {
            return lista;
        }
        return Array.Empty<TransicaoPilha>();
    }
}

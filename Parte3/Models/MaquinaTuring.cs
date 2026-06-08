namespace Parte3.Models;

/// <summary>
/// Maquina de Turing modelada como a 7-tupla M = (Q, Sigma, Gamma, delta, q0, qaccept, qreject).
/// </summary>
public sealed class MaquinaTuring
{
    /// <summary>Q - conjunto finito de estados.</summary>
    public IReadOnlySet<string> Estados { get; }

    /// <summary>Sigma - alfabeto de entrada (nao contem o branco).</summary>
    public IReadOnlySet<char> Alfabeto { get; }

    /// <summary>Gamma - alfabeto da fita (contem Sigma e o branco).</summary>
    public IReadOnlySet<char> AlfabetoFita { get; }

    /// <summary>delta - funcao de transicao (estado, simbolo) -> (novo estado, novo simbolo, direcao).</summary>
    public IReadOnlyDictionary<(string Estado, char Simbolo), TransicaoTuring> Transicao { get; }

    /// <summary>q0 - estado inicial.</summary>
    public string EstadoInicial { get; }

    /// <summary>qaccept - estado de aceitacao.</summary>
    public string EstadoAceitacao { get; }

    /// <summary>qreject - estado de rejeicao.</summary>
    public string EstadoRejeicao { get; }

    public MaquinaTuring(
        IReadOnlySet<string> estados,
        IReadOnlySet<char> alfabeto,
        IReadOnlySet<char> alfabetoFita,
        IReadOnlyDictionary<(string, char), TransicaoTuring> transicao,
        string estadoInicial,
        string estadoAceitacao,
        string estadoRejeicao)
    {
        if (estadoAceitacao == estadoRejeicao)
        {
            throw new ArgumentException("qaccept e qreject devem ser distintos.");
        }

        Estados = estados;
        Alfabeto = alfabeto;
        AlfabetoFita = alfabetoFita;
        Transicao = transicao;
        EstadoInicial = estadoInicial;
        EstadoAceitacao = estadoAceitacao;
        EstadoRejeicao = estadoRejeicao;
    }

    public bool TentarObterTransicao(string estado, char simbolo, out TransicaoTuring transicao)
    {
        return Transicao.TryGetValue((estado, simbolo), out transicao);
    }
}

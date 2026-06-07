namespace Parte2.Models;

/// <summary>
/// Resultado de uma transicao do AP: o estado de destino e a cadeia de simbolos
/// que deve ser empilhada (o primeiro caractere da cadeia fica no topo da pilha).
/// Uma cadeia vazia ("") significa apenas desempilhar o topo.
/// </summary>
public sealed class TransicaoPilha
{
    public string NovoEstado { get; }

    /// <summary>Sequencia de simbolos a empilhar (substitui o topo consumido).</summary>
    public string Empilhar { get; }

    public TransicaoPilha(string novoEstado, string empilhar)
    {
        NovoEstado = novoEstado;
        Empilhar = empilhar;
    }
}

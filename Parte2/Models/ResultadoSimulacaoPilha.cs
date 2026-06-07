namespace Parte2.Models;

/// <summary>
/// Resultado da simulacao de uma cadeia: indica se foi aceita por pilha vazia
/// e guarda a sequencia de configuracoes instantaneas do caminho percorrido.
/// </summary>
public sealed class ResultadoSimulacaoPilha
{
    public string Cadeia { get; }
    public bool Aceita { get; }
    public IReadOnlyList<ConfiguracaoInstantanea> Caminho { get; }

    public ResultadoSimulacaoPilha(string cadeia, bool aceita, IReadOnlyList<ConfiguracaoInstantanea> caminho)
    {
        Cadeia = cadeia;
        Aceita = aceita;
        Caminho = caminho;
    }
}

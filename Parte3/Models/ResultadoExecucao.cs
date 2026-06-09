namespace Parte3.Models;

public enum StatusExecucao
{
    Aceita,
    Rejeita,
    LimiteAtingido,
}

/// <summary>
/// Resultado da execucao de uma MT sobre uma entrada: status final, numero de
/// passos consumidos e o conteudo util da fita ao final (relevante para MTs que
/// computam funcoes).
/// </summary>
public sealed class ResultadoExecucao
{
    public string Entrada { get; }
    public StatusExecucao Status { get; }
    public int Passos { get; }
    public string FitaFinal { get; }
    public IReadOnlyList<PassoExecucao> Historico { get; }

    public ResultadoExecucao(
        string entrada,
        StatusExecucao status,
        int passos,
        string fitaFinal,
        IReadOnlyList<PassoExecucao> historico)
    {
        Entrada = entrada;
        Status = status;
        Passos = passos;
        FitaFinal = fitaFinal;
        Historico = historico;
    }
}

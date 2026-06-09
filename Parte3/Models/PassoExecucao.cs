namespace Parte3.Models;

/// <summary>
/// Snapshot de um passo da execucao da MT: o numero do passo, o estado corrente,
/// a posicao do cabecote e a representacao da fita (com o simbolo sob o cabecote
/// entre colchetes).
/// </summary>
public sealed class PassoExecucao
{
    public int Passo { get; }
    public string Estado { get; }
    public int Cabecote { get; }
    public string Fita { get; }

    public PassoExecucao(int passo, string estado, int cabecote, string fita)
    {
        Passo = passo;
        Estado = estado;
        Cabecote = cabecote;
        Fita = fita;
    }
}

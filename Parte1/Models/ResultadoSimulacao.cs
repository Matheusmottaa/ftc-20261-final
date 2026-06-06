namespace Parte1.Models;

public sealed class ResultadoSimulacao
{
    public string Cadeia { get; }
    public IReadOnlyList<string> RastroEstados { get; }
    public bool Aceita { get; }
    public string MotivoRejeicao { get; }

    public ResultadoSimulacao(
        string cadeia,
        IReadOnlyList<string> rastroEstados,
        bool aceita,
        string motivoRejeicao = null)
    {
        Cadeia = cadeia;
        RastroEstados = rastroEstados;
        Aceita = aceita;
        MotivoRejeicao = motivoRejeicao;
    }
}

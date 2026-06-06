namespace Parte1.Services;

public sealed class AfdConfiguracao
{
    public List<string> Estados { get; set; } = new();
    public List<string> Alfabeto { get; set; } = new();
    public string EstadoInicial { get; set; } = string.Empty;
    public List<string> EstadosAceitacao { get; set; } = new();
    public List<TransicaoConfiguracao> Transicoes { get; set; } = new();
}

public sealed class TransicaoConfiguracao
{
    public string Origem { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
}

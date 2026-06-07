namespace Parte2.Models;

/// <summary>
/// Configuracao instantanea (snapshot) do AP em um passo da computacao:
/// estado corrente, restante da cadeia de entrada e conteudo da pilha.
/// </summary>
public sealed class ConfiguracaoInstantanea
{
    public string Estado { get; }
    public string EntradaRestante { get; }
    public string Pilha { get; }
    public string Descricao { get; }

    public ConfiguracaoInstantanea(string estado, string entradaRestante, string pilha, string descricao)
    {
        Estado = estado;

        if (entradaRestante.Length == 0)
        {
            EntradaRestante = "e";
        }
        else
        {
            EntradaRestante = entradaRestante;
        }

        if (pilha.Length == 0)
        {
            Pilha = "(vazia)";
        }
        else
        {
            Pilha = pilha;
        }

        Descricao = descricao;
    }
}

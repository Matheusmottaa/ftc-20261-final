using System.Text.Json;
using Parte1.Models;

namespace Parte1.Services;

public static class AfdJsonLoader
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static AutomatoFinitoDeterministico CarregarDeArquivo(string caminho)
    {
        string json = File.ReadAllText(caminho);
        AfdConfiguracao config = JsonSerializer.Deserialize<AfdConfiguracao>(json, Opcoes);
        if (config == null)
        {
            throw new InvalidDataException($"Nao foi possivel ler o AFD de '{caminho}'.");
        }

        return Construir(config);
    }

    private static AutomatoFinitoDeterministico Construir(AfdConfiguracao config)
    {
        HashSet<string> estados = config.Estados.ToHashSet();
        HashSet<char> alfabeto = config.Alfabeto.Select(s => s.Single()).ToHashSet();
        HashSet<string> estadosAceitacao = config.EstadosAceitacao.ToHashSet();

        Dictionary<(string, char), string> transicao = new Dictionary<(string, char), string>();
        foreach (TransicaoConfiguracao t in config.Transicoes)
        {
            transicao[(t.Origem, t.Simbolo.Single())] = t.Destino;
        }

        return new AutomatoFinitoDeterministico(
            estados,
            alfabeto,
            transicao,
            config.EstadoInicial,
            estadosAceitacao);
    }
}

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
        if (!File.Exists(caminho))
        {
            throw new FileNotFoundException($"Arquivo de configuracao nao encontrado: '{caminho}'.");
        }

        string json = File.ReadAllText(caminho);
        AfdConfiguracao config = JsonSerializer.Deserialize<AfdConfiguracao>(json, Opcoes);
        if (config == null)
        {
            throw new InvalidDataException($"Nao foi possivel ler o AFD de '{caminho}'.");
        }

        Validar(config);
        return Construir(config);
    }

    private static void Validar(AfdConfiguracao config)
    {
        if (config.Estados == null || config.Estados.Count == 0)
        {
            throw new InvalidDataException("Campo 'estados' ausente ou vazio.");
        }
        if (config.Alfabeto == null || config.Alfabeto.Count == 0)
        {
            throw new InvalidDataException("Campo 'alfabeto' ausente ou vazio.");
        }
        if (string.IsNullOrWhiteSpace(config.EstadoInicial))
        {
            throw new InvalidDataException("Campo 'estadoInicial' ausente.");
        }
        if (config.EstadosAceitacao == null)
        {
            throw new InvalidDataException("Campo 'estadosAceitacao' ausente.");
        }
        if (config.Transicoes == null)
        {
            throw new InvalidDataException("Campo 'transicoes' ausente.");
        }

        foreach (string simbolo in config.Alfabeto)
        {
            if (simbolo == null || simbolo.Length != 1)
            {
                throw new InvalidDataException(
                    $"Simbolo de alfabeto invalido: '{simbolo}' (deve ter exatamente 1 caractere).");
            }
        }

        HashSet<string> estados = config.Estados.ToHashSet();
        HashSet<string> alfabeto = config.Alfabeto.ToHashSet();

        foreach (TransicaoConfiguracao t in config.Transicoes)
        {
            if (!estados.Contains(t.Origem))
            {
                throw new InvalidDataException(
                    $"Transicao com origem '{t.Origem}' que nao pertence a Q.");
            }
            if (!estados.Contains(t.Destino))
            {
                throw new InvalidDataException(
                    $"Transicao ({t.Origem}, '{t.Simbolo}') com destino '{t.Destino}' que nao pertence a Q.");
            }
            if (t.Simbolo == null || t.Simbolo.Length != 1 || !alfabeto.Contains(t.Simbolo))
            {
                throw new InvalidDataException(
                    $"Transicao de '{t.Origem}' com simbolo '{t.Simbolo}' que nao pertence a Sigma.");
            }
        }
    }

    private static AutomatoFinitoDeterministico Construir(AfdConfiguracao config)
    {
        HashSet<string> estados = config.Estados.ToHashSet();
        HashSet<char> alfabeto = config.Alfabeto.Select(s => s.Single()).ToHashSet();
        HashSet<string> estadosAceitacao = config.EstadosAceitacao.ToHashSet();

        Dictionary<(string, char), string> transicao = new Dictionary<(string, char), string>();
        foreach (TransicaoConfiguracao t in config.Transicoes)
        {
            (string, char) chave = (t.Origem, t.Simbolo.Single());
            if (transicao.ContainsKey(chave))
            {
                throw new InvalidDataException(
                    $"Transicao duplicada para ({t.Origem}, '{t.Simbolo}'): um AFD exige destino unico.");
            }
            transicao[chave] = t.Destino;
        }

        return new AutomatoFinitoDeterministico(
            estados,
            alfabeto,
            transicao,
            config.EstadoInicial,
            estadosAceitacao);
    }
}

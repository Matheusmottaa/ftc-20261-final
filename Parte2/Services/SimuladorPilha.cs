using System.Text;
using Parte2.Models;

namespace Parte2.Services;

/// <summary>
/// Simulador (possivelmente nao-deterministico) de Automato de Pilha com
/// aceitacao por pilha vazia. Explora as computacoes por busca em profundidade,
/// considerando movimentos por simbolo de entrada e lambda-movimentos.
/// </summary>
public sealed class SimuladorPilha
{
    private readonly AutomatoPilha _automato;
    private readonly int _limitePassos;

    public SimuladorPilha(AutomatoPilha automato, int limitePassos = 10_000)
    {
        _automato = automato;
        _limitePassos = limitePassos;
    }

    private List<ConfiguracaoInstantanea> _melhorCaminho = new();

    public ResultadoSimulacaoPilha Simular(string cadeia)
    {
        Stack<char> pilhaInicial = new Stack<char>();
        pilhaInicial.Push(_automato.SimboloInicialPilha);

        List<ConfiguracaoInstantanea> caminho = new List<ConfiguracaoInstantanea>
        {
            CriarConfiguracao(_automato.EstadoInicial, cadeia, 0, pilhaInicial, "configuracao inicial"),
        };

        _melhorCaminho = new List<ConfiguracaoInstantanea>(caminho);
        HashSet<string> visitados = new HashSet<string>();
        int passos = 0;

        bool aceita = Explorar(_automato.EstadoInicial, cadeia, 0, pilhaInicial, caminho, visitados, ref passos);

        List<ConfiguracaoInstantanea> caminhoFinal;
        if (aceita)
        {
            caminhoFinal = caminho;
        }
        else
        {
            caminhoFinal = _melhorCaminho;
        }

        return new ResultadoSimulacaoPilha(cadeia, aceita, caminhoFinal);
    }

    private bool Explorar(
        string estado,
        string cadeia,
        int posicao,
        Stack<char> pilha,
        List<ConfiguracaoInstantanea> caminho,
        HashSet<string> visitados,
        ref int passos)
    {
        if (passos++ > _limitePassos)
        {
            return false;
        }

        // Aceitacao por pilha vazia: toda a entrada consumida e pilha vazia.
        if (posicao == cadeia.Length && pilha.Count == 0)
        {
            return true;
        }

        string chaveEstado = $"{estado}|{posicao}|{ConteudoPilha(pilha)}";
        if (!visitados.Add(chaveEstado))
        {
            return false;
        }

        if (pilha.Count == 0)
        {
            return false; // Sem topo nao ha transicao possivel.
        }

        char topo = pilha.Peek();

        // 1) Movimentos que consomem um simbolo da entrada.
        if (posicao < cadeia.Length)
        {
            char simbolo = cadeia[posicao];
            foreach (TransicaoPilha transicao in _automato.ObterTransicoes(estado, simbolo, topo))
            {
                if (TentarTransicao(estado, cadeia, posicao, pilha, caminho, visitados,
                        transicao, simbolo, posicao + 1, ref passos))
                {
                    return true;
                }
            }
        }

        // 2) Lambda-movimentos (nao consomem entrada).
        foreach (TransicaoPilha transicao in _automato.ObterTransicoes(estado, AutomatoPilha.Lambda, topo))
        {
            if (TentarTransicao(estado, cadeia, posicao, pilha, caminho, visitados,
                    transicao, AutomatoPilha.Lambda, posicao, ref passos))
            {
                return true;
            }
        }

        return false;
    }

    private bool TentarTransicao(
        string estado,
        string cadeia,
        int posicao,
        Stack<char> pilha,
        List<ConfiguracaoInstantanea> caminho,
        HashSet<string> visitados,
        TransicaoPilha transicao,
        char simboloLido,
        int novaPosicao,
        ref int passos)
    {
        Stack<char> novaPilha = new Stack<char>(pilha.Reverse());
        novaPilha.Pop();
        for (int i = transicao.Empilhar.Length - 1; i >= 0; i--)
        {
            novaPilha.Push(transicao.Empilhar[i]);
        }

        string descricao = DescreverTransicao(estado, simboloLido, pilha.Peek(), transicao);
        ConfiguracaoInstantanea configuracao = CriarConfiguracao(transicao.NovoEstado, cadeia, novaPosicao, novaPilha, descricao);
        caminho.Add(configuracao);

        if (caminho.Count > _melhorCaminho.Count)
        {
            _melhorCaminho = new List<ConfiguracaoInstantanea>(caminho);
        }

        if (Explorar(transicao.NovoEstado, cadeia, novaPosicao, novaPilha, caminho, visitados, ref passos))
        {
            return true;
        }

        caminho.RemoveAt(caminho.Count - 1);
        return false;
    }

    private static ConfiguracaoInstantanea CriarConfiguracao(
        string estado, string cadeia, int posicao, Stack<char> pilha, string descricao)
    {
        string restante;
        if (posicao >= cadeia.Length)
        {
            restante = string.Empty;
        }
        else
        {
            restante = cadeia[posicao..];
        }

        return new ConfiguracaoInstantanea(estado, restante, ConteudoPilha(pilha), descricao);
    }

    /// <summary>Conteudo da pilha do topo para a base, como string legivel.</summary>
    private static string ConteudoPilha(Stack<char> pilha)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in pilha)
        {
            sb.Append(c);
        }
        return sb.ToString();
    }

    private static string DescreverTransicao(string estado, char simbolo, char topo, TransicaoPilha transicao)
    {
        string entrada;
        if (simbolo == AutomatoPilha.Lambda)
        {
            entrada = "e";
        }
        else
        {
            entrada = simbolo.ToString();
        }

        string empilhar;
        if (transicao.Empilhar.Length == 0)
        {
            empilhar = "e";
        }
        else
        {
            empilhar = transicao.Empilhar;
        }

        return $"d({estado}, {entrada}, {topo}) -> ({transicao.NovoEstado}, {empilhar})";
    }
}

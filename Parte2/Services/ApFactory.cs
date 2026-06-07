using Parte2.Models;

namespace Parte2.Services;

/// <summary>
/// Constroi os Automatos de Pilha usados na Parte 2.
/// </summary>
public static class ApFactory
{
    private const char Lambda = AutomatoPilha.Lambda;

    /// <summary>
    /// AP que reconhece L2 = { a^n b^n | n >= 1 } por pilha vazia.
    /// Estrategia: empilha um 'A' para cada 'a' e desempilha um 'A' para cada 'b';
    /// ao final remove o simbolo inicial Z por lambda-movimento.
    /// </summary>
    public static AutomatoPilha CriarApL2()
    {
        HashSet<string> estados = new HashSet<string> { "q0", "q1" };
        HashSet<char> alfabeto = new HashSet<char> { 'a', 'b' };
        HashSet<char> alfabetoPilha = new HashSet<char> { 'Z', 'A' };

        Dictionary<(string, char, char), List<TransicaoPilha>> transicao = new Dictionary<(string, char, char), List<TransicaoPilha>>
        {
            // Leitura dos 'a': empilha um A.
            { ("q0", 'a', 'Z'), new() { new("q0", "AZ") } },
            { ("q0", 'a', 'A'), new() { new("q0", "AA") } },

            // Primeiro 'b': inicia o desempilhamento e troca de estado.
            { ("q0", 'b', 'A'), new() { new("q1", "") } },

            // Demais 'b': continua desempilhando.
            { ("q1", 'b', 'A'), new() { new("q1", "") } },

            // Entrada consumida e apenas Z na pilha: remove Z (esvazia a pilha).
            { ("q1", Lambda, 'Z'), new() { new("q1", "") } },
        };

        return new AutomatoPilha(estados, alfabeto, alfabetoPilha, transicao,
            estadoInicial: "q0", simboloInicialPilha: 'Z');
    }

    /// <summary>
    /// AP que reconhece L3 = { w in {a,b}* | w = w^R, |w| >= 1 } (palindromos) por pilha vazia.
    /// Estrategia nao-deterministica: empilha a primeira metade, "adivinha" o meio e
    /// compara a segunda metade desempilhando.
    /// </summary>
    public static AutomatoPilha CriarApL3()
    {
        HashSet<string> estados = new HashSet<string> { "q0", "q1" };
        HashSet<char> alfabeto = new HashSet<char> { 'a', 'b' };
        HashSet<char> alfabetoPilha = new HashSet<char> { 'Z', 'A', 'B' };

        Dictionary<(string, char, char), List<TransicaoPilha>> transicao = new Dictionary<(string, char, char), List<TransicaoPilha>>();

        void Add(string estado, char entrada, char topo, string novoEstado, string empilhar)
        {
            (string, char, char) chave = (estado, entrada, topo);
            List<TransicaoPilha> lista;
            if (!transicao.TryGetValue(chave, out lista))
            {
                lista = new List<TransicaoPilha>();
                transicao[chave] = lista;
            }
            lista.Add(new TransicaoPilha(novoEstado, empilhar));
        }

        char[] topos = new[] { 'Z', 'A', 'B' };

        // Fase de empilhamento (q0): empilha o simbolo lido sobre o topo atual.
        foreach (char topo in topos)
        {
            Add("q0", 'a', topo, "q0", "A" + topo);
            Add("q0", 'b', topo, "q0", "B" + topo);
        }

        // Meio impar: le o simbolo central e nao altera a pilha (vai para q1).
        foreach (char topo in topos)
        {
            Add("q0", 'a', topo, "q1", topo.ToString());
            Add("q0", 'b', topo, "q1", topo.ToString());
        }

        // Meio par: muda para a fase de comparacao por lambda-movimento.
        Add("q0", Lambda, 'A', "q1", "A");
        Add("q0", Lambda, 'B', "q1", "B");

        // Fase de comparacao (q1): casa o simbolo lido com o topo e desempilha.
        Add("q1", 'a', 'A', "q1", "");
        Add("q1", 'b', 'B', "q1", "");

        // Entrada consumida e apenas Z na pilha: remove Z (esvazia a pilha).
        Add("q1", Lambda, 'Z', "q1", "");

        return new AutomatoPilha(estados, alfabeto, alfabetoPilha, transicao,
            estadoInicial: "q0", simboloInicialPilha: 'Z');
    }
}

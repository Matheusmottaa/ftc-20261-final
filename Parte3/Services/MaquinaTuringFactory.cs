using Parte3.Models;

namespace Parte3.Services;

/// <summary>
/// Constroi as Maquinas de Turing usadas na Parte 3.
/// </summary>
public static class MaquinaTuringFactory
{
    private const char R = TransicaoTuring.Direita;
    private const char L = TransicaoTuring.Esquerda;
    private const char Branco = Fita.Branco;

    /// <summary>
    /// MT que reconhece L4 = { a^n b^n c^n | n >= 1 }.
    /// Estrategia de marcacao: a cada passagem marca um 'a' (->X), um 'b' (->Y) e
    /// um 'c' (->Z), retornando a esquerda e repetindo ate que todos estejam marcados.
    /// </summary>
    public static MaquinaTuring CriarMtL4()
    {
        HashSet<string> estados = new HashSet<string> { "q0", "q1", "q2", "q3", "q4", "qaccept", "qreject" };
        HashSet<char> alfabeto = new HashSet<char> { 'a', 'b', 'c' };
        HashSet<char> alfabetoFita = new HashSet<char> { 'a', 'b', 'c', 'X', 'Y', 'Z', Branco };

        Dictionary<(string, char), TransicaoTuring> transicao = new Dictionary<(string, char), TransicaoTuring>
        {
            // q0: procura o proximo 'a' nao marcado.
            { ("q0", 'a'), new("q1", 'X', R) },
            { ("q0", 'Y'), new("q4", 'Y', R) }, // nao ha mais 'a': inicia verificacao final.

            // q1: avanca ate o primeiro 'b' nao marcado (pula 'a' e 'Y').
            { ("q1", 'a'), new("q1", 'a', R) },
            { ("q1", 'Y'), new("q1", 'Y', R) },
            { ("q1", 'b'), new("q2", 'Y', R) },

            // q2: avanca ate o primeiro 'c' nao marcado (pula 'b' e 'Z').
            { ("q2", 'b'), new("q2", 'b', R) },
            { ("q2", 'Z'), new("q2", 'Z', R) },
            { ("q2", 'c'), new("q3", 'Z', L) },

            // q3: retorna a esquerda ate o ultimo 'X' e reinicia o ciclo.
            { ("q3", 'a'), new("q3", 'a', L) },
            { ("q3", 'b'), new("q3", 'b', L) },
            { ("q3", 'c'), new("q3", 'c', L) },
            { ("q3", 'Y'), new("q3", 'Y', L) },
            { ("q3", 'Z'), new("q3", 'Z', L) },
            { ("q3", 'X'), new("q0", 'X', R) },

            // q4: todos os 'a' marcados; confere que so restam 'Y' e 'Z' ate o branco.
            { ("q4", 'Y'), new("q4", 'Y', R) },
            { ("q4", 'Z'), new("q4", 'Z', R) },
            { ("q4", Branco), new("qaccept", Branco, R) },
        };

        return new MaquinaTuring(estados, alfabeto, alfabetoFita, transicao,
            estadoInicial: "q0", estadoAceitacao: "qaccept", estadoRejeicao: "qreject");
    }

    /// <summary>
    /// MT que computa f(n) = n + 1 em unario: a entrada e composta por n simbolos '1'
    /// e, ao final, a fita contem n + 1 simbolos '1'.
    /// Estrategia: caminha ate o primeiro branco a direita e o substitui por '1'.
    /// </summary>
    public static MaquinaTuring CriarMtIncrementoUnario()
    {
        HashSet<string> estados = new HashSet<string> { "q0", "qaccept", "qreject" };
        HashSet<char> alfabeto = new HashSet<char> { '1' };
        HashSet<char> alfabetoFita = new HashSet<char> { '1', Branco };

        Dictionary<(string, char), TransicaoTuring> transicao = new Dictionary<(string, char), TransicaoTuring>
        {
            { ("q0", '1'), new("q0", '1', R) },
            { ("q0", Branco), new("qaccept", '1', R) },
        };

        return new MaquinaTuring(estados, alfabeto, alfabetoFita, transicao,
            estadoInicial: "q0", estadoAceitacao: "qaccept", estadoRejeicao: "qreject");
    }
}

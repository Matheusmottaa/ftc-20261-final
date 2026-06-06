using Parte1.Models;

namespace Parte1.Services;

public static class AfdFactory
{
    public static AutomatoFinitoDeterministico CriarAfdL1()
    {
        HashSet<string> estados = new HashSet<string> { "q0", "q1", "q2" };
        HashSet<char> alfabeto = new HashSet<char> { 'a', 'b' };

        Dictionary<(string, char), string> transicao = new Dictionary<(string, char), string>
        {
            { ("q0", 'a'), "q1" },
            { ("q0", 'b'), "q0" },
            { ("q1", 'a'), "q1" },
            { ("q1", 'b'), "q2" },
            { ("q2", 'a'), "q1" },
            { ("q2", 'b'), "q0" },
        };

        HashSet<string> estadosAceitacao = new HashSet<string> { "q2" };

        return new AutomatoFinitoDeterministico(
            estados,
            alfabeto,
            transicao,
            estadoInicial: "q0",
            estadosAceitacao);
    }
}

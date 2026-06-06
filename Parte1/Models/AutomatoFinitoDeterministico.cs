namespace Parte1.Models;

public sealed class AutomatoFinitoDeterministico
{
    public IReadOnlySet<string> Estados { get; }

    public IReadOnlySet<char> Alfabeto { get; }

    public IReadOnlyDictionary<(string Estado, char Simbolo), string> Transicao { get; }

    public string EstadoInicial { get; }

    public IReadOnlySet<string> EstadosAceitacao { get; }

    public AutomatoFinitoDeterministico(
        IReadOnlySet<string> estados,
        IReadOnlySet<char> alfabeto,
        IReadOnlyDictionary<(string, char), string> transicao,
        string estadoInicial,
        IReadOnlySet<string> estadosAceitacao)
    {
        Estados = estados;
        Alfabeto = alfabeto;
        Transicao = transicao;
        EstadoInicial = estadoInicial;
        EstadosAceitacao = estadosAceitacao;

        ValidarConsistencia();
    }

    public ResultadoSimulacao Simular(string cadeia)
    {
        List<string> rastro = new List<string> { EstadoInicial };
        string estadoAtual = EstadoInicial;

        foreach (char simbolo in cadeia)
        {
            if (!Alfabeto.Contains(simbolo))
            {
                return new ResultadoSimulacao(
                    cadeia,
                    rastro,
                    aceita: false,
                    motivoRejeicao: $"Simbolo '{simbolo}' fora do alfabeto.");
            }

            (string, char) chave = (estadoAtual, simbolo);
            string proximoEstado;
            if (!Transicao.TryGetValue(chave, out proximoEstado))
            {
                return new ResultadoSimulacao(
                    cadeia,
                    rastro,
                    aceita: false,
                    motivoRejeicao: $"Transicao indefinida para ({estadoAtual}, '{simbolo}').");
            }

            estadoAtual = proximoEstado;
            rastro.Add(estadoAtual);
        }

        bool aceita = EstadosAceitacao.Contains(estadoAtual);
        return new ResultadoSimulacao(cadeia, rastro, aceita);
    }

    public bool Aceitar(string cadeia) => Simular(cadeia).Aceita;

    public void ExibirDiagrama()
    {
        Console.WriteLine("=== Diagrama de Transicoes do AFD ===");
        Console.WriteLine($"Estados (Q):           {{{string.Join(", ", Estados)}}}");
        Console.WriteLine($"Alfabeto (Sigma):      {{{string.Join(", ", Alfabeto)}}}");
        Console.WriteLine($"Estado inicial (q0):   {EstadoInicial}");
        Console.WriteLine($"Estados finais (F):    {{{string.Join(", ", EstadosAceitacao)}}}");
        Console.WriteLine();

        List<char> alfabetoOrdenado = Alfabeto.OrderBy(c => c).ToList();
        const int largura = 12;

        Console.Write("delta".PadRight(largura));
        foreach (char simbolo in alfabetoOrdenado)
        {
            Console.Write($"| {simbolo}".PadRight(largura));
        }
        Console.WriteLine();
        Console.WriteLine(new string('-', largura * (alfabetoOrdenado.Count + 1)));

        foreach (string estado in Estados.OrderBy(e => e))
        {
            string marcador;
            if (estado == EstadoInicial)
            {
                marcador = "->";
            }
            else
            {
                marcador = "  ";
            }

            string aceitacao;
            if (EstadosAceitacao.Contains(estado))
            {
                aceitacao = "*";
            }
            else
            {
                aceitacao = " ";
            }

            Console.Write($"{marcador}{aceitacao}{estado}".PadRight(largura));

            foreach (char simbolo in alfabetoOrdenado)
            {
                string destino;
                if (!Transicao.TryGetValue((estado, simbolo), out destino))
                {
                    destino = "-";
                }
                Console.Write($"| {destino}".PadRight(largura));
            }
            Console.WriteLine();
        }
        Console.WriteLine("(-> estado inicial, * estado de aceitacao)");
    }

    private void ValidarConsistencia()
    {
        if (!Estados.Contains(EstadoInicial))
        {
            throw new ArgumentException($"Estado inicial '{EstadoInicial}' nao pertence a Q.");
        }

        foreach (string aceitacao in EstadosAceitacao)
        {
            if (!Estados.Contains(aceitacao))
            {
                throw new ArgumentException($"Estado de aceitacao '{aceitacao}' nao pertence a Q.");
            }
        }
    }
}

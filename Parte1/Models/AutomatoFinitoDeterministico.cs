using System.Text;

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
        int colunas = alfabetoOrdenado.Count + 1;

        string[] cabecalho = new string[colunas];
        cabecalho[0] = "delta";
        for (int i = 0; i < alfabetoOrdenado.Count; i++)
        {
            cabecalho[i + 1] = alfabetoOrdenado[i].ToString();
        }

        List<string[]> linhas = new List<string[]>();
        foreach (string estado in Estados.OrderBy(e => e))
        {
            string marcador = estado == EstadoInicial ? "->" : "  ";
            string aceitacao = EstadosAceitacao.Contains(estado) ? "*" : " ";

            string[] celulas = new string[colunas];
            celulas[0] = $"{marcador}{aceitacao}{estado}";
            for (int i = 0; i < alfabetoOrdenado.Count; i++)
            {
                if (!Transicao.TryGetValue((estado, alfabetoOrdenado[i]), out string destino))
                {
                    destino = "-";
                }
                celulas[i + 1] = destino;
            }
            linhas.Add(celulas);
        }

        int[] larguras = new int[colunas];
        for (int c = 0; c < colunas; c++)
        {
            larguras[c] = cabecalho[c].Length;
            foreach (string[] celulas in linhas)
            {
                larguras[c] = Math.Max(larguras[c], celulas[c].Length);
            }
        }

        string borda = MontarBorda(larguras);
        Console.WriteLine(borda);
        Console.WriteLine(MontarLinha(cabecalho, larguras));
        Console.WriteLine(borda);
        foreach (string[] celulas in linhas)
        {
            Console.WriteLine(MontarLinha(celulas, larguras));
        }
        Console.WriteLine(borda);
        Console.WriteLine("(-> estado inicial, * estado de aceitacao)");
    }

    private static string MontarBorda(IReadOnlyList<int> larguras)
    {
        StringBuilder construtor = new StringBuilder("+");
        foreach (int largura in larguras)
        {
            construtor.Append(new string('-', largura + 2)).Append('+');
        }
        return construtor.ToString();
    }

    private static string MontarLinha(IReadOnlyList<string> celulas, IReadOnlyList<int> larguras)
    {
        StringBuilder construtor = new StringBuilder("|");
        for (int c = 0; c < celulas.Count; c++)
        {
            construtor.Append(' ').Append(celulas[c].PadRight(larguras[c])).Append(" |");
        }
        return construtor.ToString();
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

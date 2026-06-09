using System.Text;
using Parte2.Models;
using Parte2.Services;

namespace Parte2;

/// <summary>
/// Ponto de entrada da Parte 2 - Automato de Pilha com aceitacao por pilha vazia.
/// L2 = { a^n b^n | n >= 1 } e, como desafio, L3 = palindromos sobre {a,b}.
/// </summary>
public static class Program
{
    public static void Main()
    {
        string pastaDados = Path.Combine(AppContext.BaseDirectory, "Dados");

        Console.WriteLine("##############################################");
        Console.WriteLine("# Parte 2 - AP para L2 = { a^n b^n | n >= 1 }");
        Console.WriteLine("##############################################\n");
        SimularArquivo(ApFactory.CriarApL2(), Path.Combine(pastaDados, "entradas_ap.txt"));

        Console.WriteLine("\n##############################################");
        Console.WriteLine("# Desafio: AP para L3 = palindromos sobre {a,b}");
        Console.WriteLine("##############################################\n");
        SimularArquivo(ApFactory.CriarApL3(), Path.Combine(pastaDados, "entradas_l3.txt"));
    }

    private static void SimularArquivo(AutomatoPilha automato, string caminho)
    {
        if (!File.Exists(caminho))
        {
            Console.WriteLine($"Arquivo de entradas nao encontrado: {caminho}");
            return;
        }

        SimuladorPilha simulador = new SimuladorPilha(automato);
        List<ResultadoSimulacaoPilha> resultados = new List<ResultadoSimulacaoPilha>();
        foreach (string linha in File.ReadAllLines(caminho))
        {
            ResultadoSimulacaoPilha resultado = simulador.Simular(linha);
            ExibirResultado(resultado);
            resultados.Add(resultado);
        }

        ExibirSumario(resultados);
    }

    private static void ExibirResultado(ResultadoSimulacaoPilha resultado)
    {
        string cadeia = resultado.Cadeia.Length == 0 ? "e (cadeia vazia)" : resultado.Cadeia;

        Console.Write($"Cadeia: {cadeia}  ->  ");
        EscreverStatus(resultado.Aceita);

        if (resultado.Aceita)
        {
            Console.WriteLine("Computacao aceitadora (configuracoes instantaneas):");
        }
        else
        {
            Console.WriteLine("Nenhuma computacao leva a pilha vazia com a entrada consumida.");
            Console.WriteLine("Caminho explorado mais profundo:");
        }

        ExibirCaminho(resultado.Caminho);
        Console.WriteLine();
    }

    private static void ExibirCaminho(IReadOnlyList<ConfiguracaoInstantanea> caminho)
    {
        string[] cabecalho = { "PASSO", "ESTADO", "ENTRADA", "PILHA (topo->base)", "TRANSICAO" };

        List<string[]> linhas = new List<string[]>();
        int passo = 0;
        foreach (ConfiguracaoInstantanea config in caminho)
        {
            linhas.Add(new[]
            {
                passo.ToString(),
                config.Estado,
                config.EntradaRestante,
                config.Pilha,
                config.Descricao,
            });
            passo++;
        }

        int colunas = cabecalho.Length;
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
    }

    private static void ExibirSumario(IReadOnlyList<ResultadoSimulacaoPilha> resultados)
    {
        int total = resultados.Count;
        int aceitas = resultados.Count(r => r.Aceita);
        int rejeitadas = total - aceitas;
        Console.WriteLine($"Resumo: {total} cadeia(s) | {aceitas} aceita(s) | {rejeitadas} rejeitada(s)");
    }

    private static void EscreverStatus(bool aceita)
    {
        ConsoleColor corOriginal = Console.ForegroundColor;
        Console.ForegroundColor = aceita ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(aceita ? "ACEITA" : "REJEITA");
        Console.ForegroundColor = corOriginal;
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
}

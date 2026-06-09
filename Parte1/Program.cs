using System.Text;
using Parte1.Models;
using Parte1.Services;

namespace Parte1;

public static class Program
{
    public static void Main()
    {
        string pastaDados = Path.Combine(AppContext.BaseDirectory, "Dados");
        string caminhoEntradas = Path.Combine(pastaDados, "entradas.txt");
        string caminhoJson = Path.Combine(pastaDados, "afd.json");

        Console.WriteLine("##############################################");
        Console.WriteLine("# Parte 1 - AFD para L1 (cadeias terminadas em \"ab\")");
        Console.WriteLine("##############################################\n");

        AutomatoFinitoDeterministico afd = AfdFactory.CriarAfdL1();
        afd.ExibirDiagrama();

        Console.WriteLine("\n--- Simulacao das cadeias de entradas.txt ---\n");
        SimularArquivo(afd, caminhoEntradas);

        Console.WriteLine("\n##############################################");
        Console.WriteLine("# Desafio: carregando AFD generico de afd.json");
        Console.WriteLine("##############################################\n");

        try
        {
            AutomatoFinitoDeterministico afdJson = AfdJsonLoader.CarregarDeArquivo(caminhoJson);
            afdJson.ExibirDiagrama();
            Console.WriteLine("\n--- Simulacao com o AFD carregado do JSON ---\n");
            SimularArquivo(afdJson, caminhoEntradas);
        }
        catch (Exception excecao) when (
            excecao is InvalidDataException
            or FileNotFoundException
            or ArgumentException
            or System.Text.Json.JsonException)
        {
            Console.WriteLine($"Configuracao invalida em afd.json: {excecao.Message}");
        }
    }

    private static void SimularArquivo(AutomatoFinitoDeterministico afd, string caminhoEntradas)
    {
        if (!File.Exists(caminhoEntradas))
        {
            Console.WriteLine($"Arquivo de entradas nao encontrado: {caminhoEntradas}");
            return;
        }

        string[] linhas = File.ReadAllLines(caminhoEntradas);
        List<ResultadoSimulacao> resultados = new List<ResultadoSimulacao>();
        foreach (string linha in linhas)
        {
            resultados.Add(afd.Simular(linha));
        }

        ExibirTabelaResultados(resultados);
        ExibirSumario(resultados);
    }

    private static void ExibirTabelaResultados(IReadOnlyList<ResultadoSimulacao> resultados)
    {
        const string colCadeia = "CADEIA";
        const string colStatus = "STATUS";
        const string colRastro = "RASTRO";
        const string colMotivo = "MOTIVO";

        bool temMotivo = resultados.Any(r => !string.IsNullOrEmpty(r.MotivoRejeicao));

        int larguraCadeia = colCadeia.Length;
        int larguraStatus = colStatus.Length;
        int larguraRastro = colRastro.Length;
        int larguraMotivo = colMotivo.Length;

        foreach (ResultadoSimulacao resultado in resultados)
        {
            larguraCadeia = Math.Max(larguraCadeia, FormatarCadeia(resultado.Cadeia).Length);
            larguraStatus = Math.Max(larguraStatus, FormatarStatus(resultado.Aceita).Length);
            larguraRastro = Math.Max(larguraRastro, string.Join(" -> ", resultado.RastroEstados).Length);
            larguraMotivo = Math.Max(larguraMotivo, (resultado.MotivoRejeicao ?? string.Empty).Length);
        }

        string borda = temMotivo
            ? MontarBorda(larguraCadeia, larguraStatus, larguraRastro, larguraMotivo)
            : MontarBorda(larguraCadeia, larguraStatus, larguraRastro);

        Console.WriteLine(borda);
        if (temMotivo)
        {
            Console.WriteLine($"| {colCadeia.PadRight(larguraCadeia)} | {colStatus.PadRight(larguraStatus)} | {colRastro.PadRight(larguraRastro)} | {colMotivo.PadRight(larguraMotivo)} |");
        }
        else
        {
            Console.WriteLine($"| {colCadeia.PadRight(larguraCadeia)} | {colStatus.PadRight(larguraStatus)} | {colRastro.PadRight(larguraRastro)} |");
        }
        Console.WriteLine(borda);

        foreach (ResultadoSimulacao resultado in resultados)
        {
            string cadeia = FormatarCadeia(resultado.Cadeia);
            string status = FormatarStatus(resultado.Aceita);
            string rastro = string.Join(" -> ", resultado.RastroEstados);

            Console.Write($"| {cadeia.PadRight(larguraCadeia)} | ");

            ConsoleColor corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = resultado.Aceita ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(status.PadRight(larguraStatus));
            Console.ForegroundColor = corOriginal;

            Console.Write($" | {rastro.PadRight(larguraRastro)} |");
            if (temMotivo)
            {
                Console.Write($" {(resultado.MotivoRejeicao ?? string.Empty).PadRight(larguraMotivo)} |");
            }
            Console.WriteLine();
        }

        Console.WriteLine(borda);
    }

    private static void ExibirSumario(IReadOnlyList<ResultadoSimulacao> resultados)
    {
        int total = resultados.Count;
        int aceitas = resultados.Count(r => r.Aceita);
        int rejeitadas = total - aceitas;
        Console.WriteLine($"Resumo: {total} cadeia(s) | {aceitas} aceita(s) | {rejeitadas} rejeitada(s)");
    }

    private static string FormatarCadeia(string cadeia)
    {
        return cadeia.Length == 0 ? "e (vazia)" : cadeia;
    }

    private static string FormatarStatus(bool aceita)
    {
        return aceita ? "ACEITA" : "REJEITA";
    }

    private static string MontarBorda(params int[] larguras)
    {
        StringBuilder construtor = new StringBuilder("+");
        foreach (int largura in larguras)
        {
            construtor.Append(new string('-', largura + 2)).Append('+');
        }
        return construtor.ToString();
    }
}

using System.Text;
using Parte3.Models;
using Parte3.Services;

namespace Parte3;

/// <summary>
/// Ponto de entrada da Parte 3 - Maquina de Turing.
/// Reconhece L4 = { a^n b^n c^n | n >= 1 } e, como desafio, computa f(n) = n + 1
/// em representacao unaria.
/// </summary>
public static class Program
{
    private const int LimitePassosPadrao = 1_000;

    public static void Main(string[] args)
    {
        int limitePassos = LerLimitePassos(args);
        string pastaDados = Path.Combine(AppContext.BaseDirectory, "Dados");

        Console.WriteLine("##############################################");
        Console.WriteLine("# Parte 3 - MT para L4 = { a^n b^n c^n | n >= 1 }");
        Console.WriteLine("##############################################");
        Console.WriteLine($"(limite de passos: {limitePassos})\n");
        ReconhecerL4(Path.Combine(pastaDados, "entradas_mt.txt"), limitePassos);

        Console.WriteLine("\n##############################################");
        Console.WriteLine("# Desafio: MT que computa f(n) = n + 1 (unario)");
        Console.WriteLine("##############################################\n");
        ComputarIncremento(Path.Combine(pastaDados, "entradas_incremento.txt"), limitePassos);
    }

    private static int LerLimitePassos(string[] args)
    {
        if (args.Length == 0)
        {
            return LimitePassosPadrao;
        }

        int limite;
        if (!int.TryParse(args[0], out limite) || limite <= 0)
        {
            Console.WriteLine(
                $"Argumento '{args[0]}' invalido para o limite de passos; " +
                $"usando o padrao ({LimitePassosPadrao}).\n");
            return LimitePassosPadrao;
        }

        return limite;
    }

    private static void ReconhecerL4(string caminho, int limitePassos)
    {
        MaquinaTuring maquina = MaquinaTuringFactory.CriarMtL4();
        List<ResultadoExecucao> resultados = new List<ResultadoExecucao>();
        foreach (string linha in LerLinhas(caminho))
        {
            string entrada = linha.Length == 0 ? "e (cadeia vazia)" : linha;
            Console.WriteLine($"--- Entrada: {entrada} ---");

            SimuladorTuring simulador = new SimuladorTuring(maquina, limitePassos);
            ResultadoExecucao resultado = simulador.Executar(linha);

            ExibirHistorico(resultado.Historico);
            EscreverLinhaStatus(resultado.Status, $" | passos: {resultado.Passos}");
            Console.WriteLine();
            resultados.Add(resultado);
        }

        ExibirSumarioReconhecimento(resultados);
    }

    private static void ComputarIncremento(string caminho, int limitePassos)
    {
        MaquinaTuring maquina = MaquinaTuringFactory.CriarMtIncrementoUnario();
        int total = 0;
        foreach (string linha in LerLinhas(caminho))
        {
            if (linha.Length == 0)
            {
                continue;
            }

            Console.WriteLine($"--- Entrada: {linha} (n = {linha.Length}) ---");

            SimuladorTuring simulador = new SimuladorTuring(maquina, limitePassos);
            ResultadoExecucao resultado = simulador.Executar(linha);

            ExibirHistorico(resultado.Historico);
            EscreverLinhaStatus(
                resultado.Status,
                $" | passos: {resultado.Passos} | fita final: {resultado.FitaFinal} (n + 1 = {resultado.FitaFinal.Length})");
            Console.WriteLine();
            total++;
        }

        Console.WriteLine($"Resumo: {total} entrada(s) computada(s)");
    }

    private static void ExibirHistorico(IReadOnlyList<PassoExecucao> historico)
    {
        string[] cabecalho = { "PASSO", "ESTADO", "CABECOTE", "FITA" };

        List<string[]> linhas = new List<string[]>();
        foreach (PassoExecucao passo in historico)
        {
            linhas.Add(new[]
            {
                passo.Passo.ToString(),
                passo.Estado,
                passo.Cabecote.ToString(),
                passo.Fita,
            });
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

    private static void ExibirSumarioReconhecimento(IReadOnlyList<ResultadoExecucao> resultados)
    {
        int total = resultados.Count;
        int aceitas = resultados.Count(r => r.Status == StatusExecucao.Aceita);
        int rejeitadas = resultados.Count(r => r.Status == StatusExecucao.Rejeita);
        int limite = resultados.Count(r => r.Status == StatusExecucao.LimiteAtingido);

        string resumo = $"Resumo: {total} cadeia(s) | {aceitas} aceita(s) | {rejeitadas} rejeitada(s)";
        if (limite > 0)
        {
            resumo += $" | {limite} no limite de passos";
        }
        Console.WriteLine(resumo);
    }

    private static void EscreverLinhaStatus(StatusExecucao status, string sufixo)
    {
        Console.Write("Status: ");
        ConsoleColor corOriginal = Console.ForegroundColor;
        Console.ForegroundColor = CorDoStatus(status);
        Console.Write(Traduzir(status));
        Console.ForegroundColor = corOriginal;
        Console.WriteLine(sufixo);
    }

    private static ConsoleColor CorDoStatus(StatusExecucao status) => status switch
    {
        StatusExecucao.Aceita => ConsoleColor.Green,
        StatusExecucao.Rejeita => ConsoleColor.Red,
        StatusExecucao.LimiteAtingido => ConsoleColor.Yellow,
        _ => Console.ForegroundColor,
    };

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

    private static string[] LerLinhas(string caminho)
    {
        if (!File.Exists(caminho))
        {
            Console.WriteLine($"Arquivo de entradas nao encontrado: {caminho}");
            return Array.Empty<string>();
        }
        return File.ReadAllLines(caminho);
    }

    private static string Traduzir(StatusExecucao status) => status switch
    {
        StatusExecucao.Aceita => "ACEITA",
        StatusExecucao.Rejeita => "REJEITA",
        StatusExecucao.LimiteAtingido => "LIMITE DE PASSOS ATINGIDO",
        _ => "DESCONHECIDO",
    };
}

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
        foreach (string linha in LerLinhas(caminho))
        {
            string entrada;
            if (linha.Length == 0)
            {
                entrada = "e (cadeia vazia)";
            }
            else
            {
                entrada = linha;
            }

            Console.WriteLine($"--- Entrada: {entrada} ---");

            SimuladorTuring simulador = new SimuladorTuring(maquina, limitePassos, exibirPassos: true);
            ResultadoExecucao resultado = simulador.Executar(linha);

            Console.WriteLine($"Status: {Traduzir(resultado.Status)} | passos: {resultado.Passos}");
            Console.WriteLine();
        }
    }

    private static void ComputarIncremento(string caminho, int limitePassos)
    {
        MaquinaTuring maquina = MaquinaTuringFactory.CriarMtIncrementoUnario();
        foreach (string linha in LerLinhas(caminho))
        {
            if (linha.Length == 0)
            {
                continue;
            }

            Console.WriteLine($"--- Entrada: {linha} (n = {linha.Length}) ---");

            SimuladorTuring simulador = new SimuladorTuring(maquina, limitePassos, exibirPassos: true);
            ResultadoExecucao resultado = simulador.Executar(linha);

            Console.WriteLine(
                $"Status: {Traduzir(resultado.Status)} | passos: {resultado.Passos} | " +
                $"fita final: {resultado.FitaFinal} (n + 1 = {resultado.FitaFinal.Length})");
            Console.WriteLine();
        }
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

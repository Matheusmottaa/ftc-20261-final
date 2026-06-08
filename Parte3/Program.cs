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
    public static void Main()
    {
        string pastaDados = Path.Combine(AppContext.BaseDirectory, "Dados");

        Console.WriteLine("##############################################");
        Console.WriteLine("# Parte 3 - MT para L4 = { a^n b^n c^n | n >= 1 }");
        Console.WriteLine("##############################################\n");
        ReconhecerL4(Path.Combine(pastaDados, "entradas_mt.txt"));

        Console.WriteLine("\n##############################################");
        Console.WriteLine("# Desafio: MT que computa f(n) = n + 1 (unario)");
        Console.WriteLine("##############################################\n");
        ComputarIncremento(Path.Combine(pastaDados, "entradas_incremento.txt"));
    }

    private static void ReconhecerL4(string caminho)
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

            SimuladorTuring simulador = new SimuladorTuring(maquina, limitePassos: 1_000, exibirPassos: true);
            ResultadoExecucao resultado = simulador.Executar(linha);

            Console.WriteLine($"Status: {Traduzir(resultado.Status)} | passos: {resultado.Passos}");
            Console.WriteLine();
        }
    }

    private static void ComputarIncremento(string caminho)
    {
        MaquinaTuring maquina = MaquinaTuringFactory.CriarMtIncrementoUnario();
        foreach (string linha in LerLinhas(caminho))
        {
            if (linha.Length == 0)
            {
                continue;
            }

            Console.WriteLine($"--- Entrada: {linha} (n = {linha.Length}) ---");

            SimuladorTuring simulador = new SimuladorTuring(maquina, limitePassos: 1_000, exibirPassos: true);
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

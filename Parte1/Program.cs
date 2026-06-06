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

        AutomatoFinitoDeterministico afdJson = AfdJsonLoader.CarregarDeArquivo(caminhoJson);
        afdJson.ExibirDiagrama();
        Console.WriteLine("\n--- Simulacao com o AFD carregado do JSON ---\n");
        SimularArquivo(afdJson, caminhoEntradas);
    }

    private static void SimularArquivo(AutomatoFinitoDeterministico afd, string caminhoEntradas)
    {
        if (!File.Exists(caminhoEntradas))
        {
            Console.WriteLine($"Arquivo de entradas nao encontrado: {caminhoEntradas}");
            return;
        }

        string[] linhas = File.ReadAllLines(caminhoEntradas);
        foreach (string linha in linhas)
        {
            ResultadoSimulacao resultado = afd.Simular(linha);
            ExibirResultado(resultado);
        }
    }

    private static void ExibirResultado(ResultadoSimulacao resultado)
    {
        string representacaoCadeia;
        if (resultado.Cadeia.Length == 0)
        {
            representacaoCadeia = "e (cadeia vazia)";
        }
        else
        {
            representacaoCadeia = resultado.Cadeia;
        }

        string status;
        if (resultado.Aceita)
        {
            status = "ACEITA";
        }
        else
        {
            status = "REJEITA";
        }

        string rastro = string.Join(" -> ", resultado.RastroEstados);

        Console.WriteLine($"Cadeia : {representacaoCadeia}");
        Console.WriteLine($"Rastro : {rastro}");
        Console.WriteLine($"Status : {status}");
        if (!resultado.Aceita && resultado.MotivoRejeicao != null)
        {
            Console.WriteLine($"Motivo : {resultado.MotivoRejeicao}");
        }
        Console.WriteLine();
    }
}

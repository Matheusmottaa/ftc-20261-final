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
        foreach (string linha in File.ReadAllLines(caminho))
        {
            ResultadoSimulacaoPilha resultado = simulador.Simular(linha);
            ExibirResultado(resultado);
        }
    }

    private static void ExibirResultado(ResultadoSimulacaoPilha resultado)
    {
        string cadeia;
        if (resultado.Cadeia.Length == 0)
        {
            cadeia = "e (cadeia vazia)";
        }
        else
        {
            cadeia = resultado.Cadeia;
        }

        Console.WriteLine($"Cadeia: {cadeia}");

        if (resultado.Aceita)
        {
            Console.WriteLine("Computacao aceitadora (configuracoes instantaneas):");
            ExibirCaminho(resultado.Caminho);
        }
        else
        {
            Console.WriteLine("Nenhuma computacao leva a pilha vazia com a entrada consumida.");
            Console.WriteLine("Caminho explorado mais profundo:");
            ExibirCaminho(resultado.Caminho);
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

        Console.WriteLine($"Status: {status}");
        Console.WriteLine();
    }

    private static void ExibirCaminho(IReadOnlyList<ConfiguracaoInstantanea> caminho)
    {
        int passo = 0;
        foreach (ConfiguracaoInstantanea config in caminho)
        {
            Console.WriteLine(
                $"  [{passo,2}] estado={config.Estado,-3} | entrada={config.EntradaRestante,-8} | " +
                $"pilha(topo->base)={config.Pilha,-10} | {config.Descricao}");
            passo++;
        }
    }
}

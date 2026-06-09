using Parte3.Models;

namespace Parte3.Services;

/// <summary>
/// Simulador da Maquina de Turing. Executa passo a passo, registrando a cada passo
/// o estado atual, o conteudo completo da fita (com o cabecote destacado) e a
/// posicao do cabecote. Possui contador e limite de passos para evitar loops
/// infinitos. A exibicao do historico fica a cargo da camada de aplicacao.
/// </summary>
public sealed class SimuladorTuring
{
    private readonly MaquinaTuring _maquina;
    private readonly int _limitePassos;

    public SimuladorTuring(MaquinaTuring maquina, int limitePassos = 1_000)
    {
        _maquina = maquina;
        _limitePassos = limitePassos;
    }

    public ResultadoExecucao Executar(string entrada)
    {
        Fita fita = new Fita(entrada);
        string estado = _maquina.EstadoInicial;
        int posicao = 0;
        int passos = 0;

        List<PassoExecucao> historico = new List<PassoExecucao>();
        RegistrarPasso(historico, passos, estado, fita, posicao);

        while (estado != _maquina.EstadoAceitacao && estado != _maquina.EstadoRejeicao)
        {
            if (passos >= _limitePassos)
            {
                return new ResultadoExecucao(
                    entrada, StatusExecucao.LimiteAtingido, passos, fita.ConteudoUtil(), historico);
            }

            char simboloAtual = fita.Ler(posicao);
            TransicaoTuring transicao;
            if (!_maquina.TentarObterTransicao(estado, simboloAtual, out transicao))
            {
                // Transicao indefinida e tratada como rejeicao explicita.
                estado = _maquina.EstadoRejeicao;
                break;
            }

            fita.Escrever(posicao, transicao.NovoSimbolo);
            if (transicao.Direcao == TransicaoTuring.Direita)
            {
                posicao += 1;
            }
            else
            {
                posicao -= 1;
            }
            estado = transicao.NovoEstado;
            passos++;

            RegistrarPasso(historico, passos, estado, fita, posicao);
        }

        StatusExecucao status;
        if (estado == _maquina.EstadoAceitacao)
        {
            status = StatusExecucao.Aceita;
        }
        else
        {
            status = StatusExecucao.Rejeita;
        }

        return new ResultadoExecucao(entrada, status, passos, fita.ConteudoUtil(), historico);
    }

    private static void RegistrarPasso(List<PassoExecucao> historico, int passo, string estado, Fita fita, int posicao)
    {
        historico.Add(new PassoExecucao(passo, estado, posicao, fita.Representar(posicao)));
    }
}

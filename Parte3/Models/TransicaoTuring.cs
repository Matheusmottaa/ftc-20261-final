namespace Parte3.Models;

/// <summary>
/// Resultado de uma transicao da MT: o novo estado, o simbolo a escrever na
/// celula atual e a direcao de movimento do cabecote ('L' ou 'R').
/// </summary>
public sealed class TransicaoTuring
{
    public const char Esquerda = 'L';
    public const char Direita = 'R';

    public string NovoEstado { get; }
    public char NovoSimbolo { get; }
    public char Direcao { get; }

    public TransicaoTuring(string novoEstado, char novoSimbolo, char direcao)
    {
        if (direcao != Esquerda && direcao != Direita)
        {
            throw new ArgumentException($"Direcao invalida '{direcao}'. Use 'L' ou 'R'.");
        }

        NovoEstado = novoEstado;
        NovoSimbolo = novoSimbolo;
        Direcao = direcao;
    }
}

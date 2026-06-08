using System.Text;

namespace Parte3.Models;

/// <summary>
/// Fita infinita da Maquina de Turing implementada como estrutura dinamica
/// (Dictionary&lt;int,char&gt;), onde a chave e a posicao. Celulas nao escritas
/// valem o simbolo branco '_'.
/// </summary>
public sealed class Fita
{
    public const char Branco = '_';

    private readonly Dictionary<int, char> _celulas = new();
    private int _menorIndice;
    private int _maiorIndice;

    public Fita(string conteudoInicial)
    {
        if (string.IsNullOrEmpty(conteudoInicial))
        {
            _celulas[0] = Branco;
            _menorIndice = 0;
            _maiorIndice = 0;
            return;
        }

        for (int i = 0; i < conteudoInicial.Length; i++)
        {
            _celulas[i] = conteudoInicial[i];
        }
        _menorIndice = 0;
        _maiorIndice = conteudoInicial.Length - 1;
    }

    public char Ler(int posicao)
    {
        char simbolo;
        if (_celulas.TryGetValue(posicao, out simbolo))
        {
            return simbolo;
        }
        return Branco;
    }

    public void Escrever(int posicao, char simbolo)
    {
        _celulas[posicao] = simbolo;
        if (posicao < _menorIndice) _menorIndice = posicao;
        if (posicao > _maiorIndice) _maiorIndice = posicao;
    }

    /// <summary>
    /// Representa a fita do menor ao maior indice utilizado, destacando o simbolo
    /// sob o cabecote com colchetes, por exemplo: X Y [Z] _.
    /// </summary>
    public string Representar(int posicaoCabecote)
    {
        int inicio = Math.Min(_menorIndice, posicaoCabecote);
        int fim = Math.Max(_maiorIndice, posicaoCabecote);

        StringBuilder sb = new StringBuilder();
        for (int i = inicio; i <= fim; i++)
        {
            char simbolo = Ler(i);
            if (i == posicaoCabecote)
            {
                sb.Append($"[{simbolo}]");
            }
            else
            {
                sb.Append($" {simbolo} ");
            }
        }
        return sb.ToString();
    }

    /// <summary>Conteudo da fita sem espacos em branco nas bordas (resultado do calculo).</summary>
    public string ConteudoUtil()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = _menorIndice; i <= _maiorIndice; i++)
        {
            sb.Append(Ler(i));
        }
        return sb.ToString().Trim(Branco);
    }
}

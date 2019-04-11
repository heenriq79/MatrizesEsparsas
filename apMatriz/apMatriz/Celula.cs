// RA: 16163 - Davi Oliveira da Silva
// RA: 16196 - Vitor Menezes Bartier dos Anjos
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apMatriz
{
    // classe Celula substitui a NoLista, se fosse uma Lista Simples
    class Celula
    {
        private double? valor;
        private int linha, coluna;
        private Celula direita, abaixo;


        // cria uma célula com um valor em uma linha e coluna
        public Celula(double? valor, int linha, int coluna)
        {
            Valor = valor;
            Linha = linha;
            Coluna = coluna;
        }

        // o valor que a célula armazena
        public double? Valor
        {
            get => valor;
            set => valor = value;
        }

        // acesso da linha que a célula está
        public int Linha
        {
            get => linha;
            set => linha = value;
        }

        // acesso da coluna que a célula está
        public int Coluna
        {
            get => coluna;
            set => coluna = value;
        }

        // acesso da célula da direita
        public Celula Direita
        {
            get => direita;
            set => direita = value;
        }

        // acesso da celula debaixo
        public Celula Abaixo
        {
            get => abaixo;
            set => abaixo = value;
        }

        // retorna no padrão do arquivo (valor, linha, coluna)
        public override string ToString()
        {
            String ret = "(";

            if (this.Valor == null)
                ret += 0 + ",";
            else
                ret += this.Valor + ",";

            ret += this.Linha + "," + this.Coluna + ")";

            return ret;
        }

        public string ParaArquivo()
        {
            return this.Valor + ", " + this.Linha + ", " + this.Coluna;
        }
    }
}
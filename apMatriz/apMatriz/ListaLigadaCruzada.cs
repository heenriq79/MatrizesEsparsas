// Henrique Costa Dionísio - 18205
// Maia de Souza Ugarte    - 18182

using System;
using System.IO;
using System.Windows.Forms;

namespace apMatriz
{
    class ListaLigadaCruzada
    {
        int linhas;
        int colunas;
        Celula cabecaPrincipal;

        // numero de linhas que a matriz tem -> não permite alterar fora da classe
        public int Linhas { get => linhas;}

        // número de colunas que a matriz esparsa contém -> não permite alterar fora da classe
        public int Colunas { get => colunas;}

        // se 'não existe' matriz (não tem nada, ou seja, cabecaPrincipal == null), retorna true
        public bool EstaVazia
        { get => cabecaPrincipal == null; }



        // construtor da matriz esparsa -> pega linhas e colunas e cria as celulas para cada uma delas
        public ListaLigadaCruzada(int linhas, int colunas)
        {
            if (linhas <= 0 || colunas <= 0)
                throw new Exception("Erro: não é possível criar matrizes com linhas ou colunas menores que 0");

            this.linhas  = linhas;
            this.colunas = colunas;

            cabecaPrincipal = new Celula(null, -1, -1);  // inicia na posição -1, onde não serão guardados 
                                                         // valores nos proximos índices

            // irá adicionar as células à matriz
            Celula caminho = cabecaPrincipal;   // início do caminho é a cabeça

            // primeiro as linhas
            for (int i = 0; i < linhas; i++)
            {
                Celula linhaCabeca  = new Celula(null, i, -1);

                caminho.Abaixo  = linhaCabeca;
                caminho.Direita = caminho;
                caminho         = caminho.Abaixo;
            }
            caminho.Abaixo  = cabecaPrincipal;
            caminho.Direita = caminho;

            // inicia outro caminho para criar as colunas da matriz
            caminho = cabecaPrincipal;

            for (int i = 0; i < colunas; i++)
            {
                Celula colunaCabeca = new Celula(null, -1, i);

                // verifica se é a cabecaPrincipal para não perder
                // a lista das linhas
                if (caminho != cabecaPrincipal)
                    caminho.Abaixo = caminho;

                caminho.Direita = colunaCabeca;
                caminho         = caminho.Direita;
            }
            caminho.Abaixo  = caminho;
            caminho.Direita = cabecaPrincipal;
        }


        // insere um elemento na linha e coluna do parâmetro
        public void Inserir(double valor, int linha, int coluna)
        {
            if (linha < 0 || linha >= this.Linhas || coluna < 0 || coluna >= this.Colunas)
                throw new Exception("Inserção fora dos índices da matriz");


            Celula colunaCabeca = cabecaPrincipal;
            Celula linhaCabeca = cabecaPrincipal;

            // percorre as linhas até chegar no índice desejado
            for (int l = 0; l <= linha; l++)
                linhaCabeca = linhaCabeca.Abaixo;

            // percorre as colunas até chegar no índice desejado
            for (int c = 0; c <= coluna; c++)
                colunaCabeca = colunaCabeca.Direita;

            Celula anterior = linhaCabeca;
            Celula atual = linhaCabeca.Direita;

            while (atual.Coluna < coluna && atual.Coluna != -1)
            {
                anterior = atual;
                atual = atual.Direita;
            }

            if (ValorDe(linha, coluna) == 0)
            {
                Celula inserida = new Celula(valor, linha, coluna);

                anterior.Direita = inserida;
                inserida.Direita = atual;

                Celula colunaAnterior = colunaCabeca;
                Celula caminhoColuna = colunaCabeca.Abaixo;

                while (caminhoColuna.Abaixo != colunaCabeca && caminhoColuna.Linha < linha)
                {
                    colunaAnterior = caminhoColuna;
                    caminhoColuna = caminhoColuna.Abaixo;
                }
                inserida.Abaixo = caminhoColuna;
                colunaAnterior.Abaixo = inserida;
            }
            else
                atual.Valor = valor; // apenas altera o valor da célula já existente
        }


        // retorna o valor da linha e colunida fornecidas
        public double ValorDe(int linha, int coluna)
        {
            if (linha < 0 || coluna < 0 || linha >= Linhas || coluna >= Colunas)
                throw new Exception("Procura fora dos índices da matriz");

            Celula linhaCabeca = cabecaPrincipal;

            for (int i = 0; i <= linha; i++)
                linhaCabeca = linhaCabeca.Abaixo;

            // percorre as colunas da linha para posicionar na célula e retornar o valor correspondente
            Celula caminhoColuna = linhaCabeca.Direita;

            while (caminhoColuna.Coluna < coluna && caminhoColuna.Direita != linhaCabeca)
                caminhoColuna = caminhoColuna.Direita;

            if (caminhoColuna.Coluna != coluna)
                return 0;  // valor padrão

            return (double)caminhoColuna.Valor;
        }


        // remove o valor que está na linha e coluna fornecida
        public bool RemoverEm(int linha, int coluna)
        {
            if (linha  < 0  || linha  >= Linhas || coluna < 0  || coluna >= Colunas)
                throw new Exception("Exclusão fora dos índices da matriz");

            if (ValorDe(linha, coluna) == 0) // se for o valor padrão, retornará false
                return false;

            Celula colunaCabeca = cabecaPrincipal;
            Celula linhaCabeca  = cabecaPrincipal;

            for (int j = 0; j <= coluna; j++)
                colunaCabeca = colunaCabeca.Direita;

            for (int i = 0; i <= linha; i++)
                linhaCabeca = linhaCabeca.Abaixo;

            Celula anterior = linhaCabeca;
            Celula atual    = linhaCabeca.Direita; // atual posicionará na célula que será removida

            while (atual.Coluna < coluna && atual.Coluna != -1)
            {
                anterior = atual;
                atual = atual.Direita;
            }

            anterior.Direita = atual.Direita;

            // percorre as colunas para acertar as ligações necessárias
            Celula caminhoColuna = colunaCabeca;

            while (caminhoColuna.Abaixo != atual)
                caminhoColuna = caminhoColuna.Abaixo;

            caminhoColuna.Abaixo = atual.Abaixo;

            return true;
        }

        // soma um valor a todas as linhas de uma coluna
        public void SomarNaColuna(double valor, int coluna)
        {
            if (coluna < 0 || coluna >= Colunas)
                throw new Exception("Coluna inválida");

            if (valor == 0)
                throw new Exception("Verifique se o valor digitado é diferente de 0");

            double elementoAtual = 0;
            for (int i=0; i<Linhas; i++)
            {
                elementoAtual = ValorDe(i, coluna);
                if (elementoAtual + valor == 0)
                {
                    RemoverEm(i, coluna);
                    continue;
                }
                Inserir(elementoAtual + valor, i, coluna);
            }
        }

        // exibe a matriz um datagridview (dgv's)
        public void ExibirDataGridView(DataGridView gridView)
        {
            if (gridView == null)
                throw new Exception("GridView inválido");

            // reseta o gridview
            gridView.Columns.Clear(); // colunas
            gridView.Rows.Clear();    // linhas

            // cria o cabeçalho das colunas
            for (int i = 0; i < this.Colunas; i++)
                gridView.Columns.Add(i.ToString(), i.ToString());

            string[] linhaMatriz = new string[this.Colunas];

            // percorre as linhas da matriz e insere no gridview
            for (int j = 0; j < this.Linhas; j++)
            {
                for (int k = 0; k < this.Colunas; k++)
                {
                    linhaMatriz[k] = this.ValorDe(j, k).ToString();
                }
                gridView.Rows.Add(linhaMatriz);
                gridView.Rows[j].HeaderCell.Value = j.ToString(); // adiciona cabeçalho da linha
            }
            gridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
            gridView.AutoResizeColumns();
        }

        // apaga / desaloca a matriz
        public void ApagarMatriz()
        {
            cabecaPrincipal = null;
            linhas          = 0;
            colunas         = 0;
        }

        // soma duas matrizes utilizando os principios matematicos
        public ListaLigadaCruzada SomarMatrizes(ListaLigadaCruzada outraMatriz)
        {
            if (this.linhas != outraMatriz.linhas || this.colunas != outraMatriz.colunas)
                throw new ArgumentException("Ambas as matrizes devem ter a mesma dimensão!");

            // retornará essa matriz
            ListaLigadaCruzada soma = new ListaLigadaCruzada(this.linhas, this.colunas);

            // atual para percorrer a matriz
            Celula atual = this.cabecaPrincipal.Abaixo.Direita;

            // copia esta matriz (this) na matriz soma
            for (int l = 0; l < this.linhas; l++)
            {
                for (int c = atual.Coluna; c >= 0; c = atual.Coluna)
                {
                    if (atual.Valor != null)
                    {
                        atual = atual.Direita;
                        soma.Inserir(this.ValorDe(l, c), l, c);
                    }
                }
                atual = atual.Abaixo.Direita;
            }

            atual = outraMatriz.cabecaPrincipal.Abaixo.Direita;

            for (int l = 0; l < outraMatriz.linhas; l++)
            {
                for (int c = atual.Coluna; c >= 0; c = atual.Coluna)
                    if (atual.Valor != null)
                    {
                        double elem = soma.ValorDe(l, c) != 0 ? soma.ValorDe(l, c) + outraMatriz.ValorDe(l, c) : outraMatriz.ValorDe(l, c);

                        soma.Inserir(elem, l, c);
                        atual = atual.Direita;
                    }
                atual = atual.Abaixo.Direita;
            }

            return soma;
        }

        // multiplica duas matrizes utilizando os principios matematicos
        public ListaLigadaCruzada MultiplicarMatrizes(ListaLigadaCruzada outraMatriz)
        {
            if (this.Colunas != outraMatriz.Linhas)
                throw new ArgumentException("Número de colunas de uma matriz deve ser igual o número de linhas da outra!");

            ListaLigadaCruzada ret;

            ret = new ListaLigadaCruzada(this.Linhas, outraMatriz.Colunas);


            double total = 0;
            for (int i = 0; i < this.Linhas; i++)
            {
                for (int j = 0; j < outraMatriz.Colunas; j++)
                {
                    total = 0;
                    int k;
                    for (k = 0; k < this.Colunas; k++)
                    {
                        total += this.ValorDe(i, k) * outraMatriz.ValorDe(k, j);
                    }
                    if (total != 0)
                    {
                        ret.Inserir(total, i, j);
                    }
                }
            }

            return ret;
        }

        public ListBox ToList()
        {
            ListBox ret = new ListBox();

            Celula linhaCabeca = cabecaPrincipal.Abaixo;

            while (linhaCabeca != cabecaPrincipal)
            {
                Celula caminhoLinha = linhaCabeca.Direita;

                while (caminhoLinha != linhaCabeca)
                {
                    ret.Items.Add(caminhoLinha.ToString());

                    caminhoLinha = caminhoLinha.Direita;
                }

                linhaCabeca = linhaCabeca.Abaixo;
            }
            return ret;
        }

        public void SalvarEmArquivo(StreamWriter arq)
        {
            StreamWriter arquivo = arq;
            arquivo.WriteLine(Linhas + " " + Colunas);

            Celula linhaCabeca = cabecaPrincipal.Abaixo;

            while (linhaCabeca != cabecaPrincipal)
            {
                Celula caminhoLinha = linhaCabeca.Direita;

                while (caminhoLinha != linhaCabeca)
                {
                    arquivo.WriteLine(caminhoLinha.ParaArquivo());
                    caminhoLinha = caminhoLinha.Direita;
                }
                linhaCabeca = linhaCabeca.Abaixo;
            }
            arquivo.Close();
        }
    }
}
